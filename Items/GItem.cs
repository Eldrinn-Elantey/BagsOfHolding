using System;
using System.Collections.Generic;	
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JPANsBagsOfHoldingMod.Items
{
	public class GItem : GlobalItem
	{
        public override bool InstancePerEntity
        {
            get
            {
                return false;
            }
        }

        public override bool CloneNewInstances
        {
            get
            {
                return false;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (BagsOfHoldingMod.tooltipDisplay)
            {
                tooltips.Add(new TooltipLine(this.mod, "tagID", "<" + GenericHoldingBag.ItemToTag(item) + ">"));
            }
        }

        public override bool ItemSpace(Item item, Player player)
        {
            string itmTag = GenericHoldingBag.ItemToTag(item);
            BagPlayer bpl = player.GetModPlayer<BagPlayer>();
            if (bpl.checkAccessories)
            {
                for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                {
                    if (player.armor[i] != null && player.armor[i].type != 0 && player.armor[i].modItem != null && player.armor[i].modItem is GenericHoldingBag)
                    {
                        if (((GenericHoldingBag)(player.armor[i].modItem)).canPickupItem(itmTag))
                        {
                            return true;
                        }
                    }
                }
            }
            if (bpl.checkVanity)
            {
                for (int i = 13; i < 18 + player.extraAccessorySlots; i++)
                {
                    if (player.armor[i] != null && player.armor[i].type != 0 && player.armor[i].modItem != null && player.armor[i].modItem is GenericHoldingBag)
                    {
                        if (((GenericHoldingBag)(player.armor[i].modItem)).canPickupItem(itmTag))
                        {
                            return true;
                        }
                    }
                }
            }
            if (bpl.checkInventory)
            {
                for (int i = 49; i >= 10; i--)
                {
                    if (player.inventory[i] != null && player.inventory[i].type != 0 && player.inventory[i].modItem != null && player.inventory[i].modItem is GenericHoldingBag)
                    {
                        if (((GenericHoldingBag)(player.inventory[i].modItem)).canPickupItem(itmTag))
                        {
                            return true;
                        }
                    }
                }
            }
            if (bpl.checkHotbar)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (player.inventory[i] != null && player.inventory[i].type != 0 && player.inventory[i].modItem != null && player.inventory[i].modItem is GenericHoldingBag)
                    {
                        if (((GenericHoldingBag)(player.inventory[i].modItem)).canPickupItem(itmTag))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void addItemIfPossible(Item item, GenericHoldingBag bag)
        {
            if (!bag.canPickupItem(GenericHoldingBag.ItemToTag(item)))
            {
                //ErrorLogger.Log("Could not get in the bag: <" + GenericHoldingBag.ItemToTag(item) + "> order:" + bag.order.Contains(GenericHoldingBag.ItemToTag(item)) + "; preventPickup:" + bag.preventPickup.Contains(GenericHoldingBag.ItemToTag(item)));
                return;
            }

            Item cln = new Item();
            cln.SetDefaults(item.type);
            long total = bag.addItem(item);
            if (total > 0)
            {
                cln.position = new Microsoft.Xna.Framework.Vector2(item.position.X, item.position.Y + 30);
                cln.stack = (int)total;
                ItemText.NewText(cln, (int)total);
            }
        }

        public override bool OnPickup(Item item, Player player){
			try{
                int quantityRemoved = 0;
                if (BagsOfHoldingMod.tryPlace)
                {
                    quantityRemoved = tryPlaceInInventory(item, player);
                    if(item.stack <= 0)
                    {
                        item.stack = quantityRemoved;
                        return true;
                    }
                }

                if (BagsOfHoldingMod.checkAccessories)
                {
                    for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                    {
                        if (player.armor[i] != null && player.armor[i].type != 0 && player.armor[i].modItem != null && player.armor[i].modItem is GenericHoldingBag)
                        {
                            addItemIfPossible(item, player.armor[i].modItem as GenericHoldingBag);
                            if (item.stack <= 0)
                            {
                                if(quantityRemoved > 0)
                                {
                                    item.stack = quantityRemoved;
                                    return true;
                                }
                                if (BagsOfHoldingMod.playSoundOnPickup)
                                {
                                    playSound(item,player);
                                }
                                return false;
                            }
                                
                        }
                    }
                }
                if (BagsOfHoldingMod.checkVanity)
                {
                    for (int i = 13; i < 18 + player.extraAccessorySlots; i++)
                    {
                        if (player.armor[i] != null && player.armor[i].type != 0 && player.armor[i].modItem != null && player.armor[i].modItem is GenericHoldingBag)
                        {
                            addItemIfPossible(item, player.armor[i].modItem as GenericHoldingBag);
                            if (item.stack <= 0)
                            {
                                if (quantityRemoved > 0)
                                {
                                    item.stack = quantityRemoved;
                                    return true;
                                }
                                if (BagsOfHoldingMod.playSoundOnPickup)
                                {
                                    playSound(item, player);
                                }
                                return false;
                            }
                        }
                    }
                }
                if (BagsOfHoldingMod.checkInventory)
                {
                    for (int i = 49; i >= 10; i--)
                    {
                        if (player.inventory[i] != null && player.inventory[i].type != 0 && player.inventory[i].modItem != null && player.inventory[i].modItem is GenericHoldingBag)
                        {
                            addItemIfPossible(item, player.inventory[i].modItem as GenericHoldingBag);
                            if (item.stack <= 0)
                            {
                                if (quantityRemoved > 0)
                                {
                                    item.stack = quantityRemoved;
                                    return true;
                                }
                                if (BagsOfHoldingMod.playSoundOnPickup)
                                {
                                    playSound(item, player);
                                }
                                return false;
                            }
                        }
                    }
                }
                if (BagsOfHoldingMod.checkHotbar)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (player.inventory[i] != null && player.inventory[i].type != 0 && player.inventory[i].modItem != null && player.inventory[i].modItem is GenericHoldingBag)
                        {
                            addItemIfPossible(item, player.inventory[i].modItem as GenericHoldingBag);
                            if (item.stack <= 0)
                            {
                                if (quantityRemoved > 0)
                                {
                                    item.stack = quantityRemoved;
                                    return true;
                                }
                                if (BagsOfHoldingMod.playSoundOnPickup)
                                {
                                    playSound(item, player);
                                }
                                return false;
                            }
                        }
                    }
                }
                item.stack += quantityRemoved;
                return true;
			} catch (Exception ex){
				BagsOfHoldingMod.debugChat("Error: " + ex);
				return true;
			}
		}

        private void playSound(Item item, Player player)
        {
            if (item.type == ItemID.CopperCoin ||
                item.type == ItemID.SilverCoin ||
                item.type == ItemID.GoldCoin ||
                item.type == ItemID.PlatinumCoin)
            {
                Mod rupee = ModLoader.GetMod("RupeeCoinSound");
                if (rupee != null)
                {
                    rupee.GetGlobalItem("Coin").OnPickup(item, player);
                }
                else
                {
                    Main.PlaySound(SoundID.CoinPickup, player.position);
                }
            }
            else
            {
                Main.PlaySound(SoundID.Grab, player.position);
            }
        }

        private int tryPlaceInInventory(Item item, Player player)
        {
            int ans = 0;
            for(int i = 0; i < player.inventory.Length && item.stack > 0; i++)
            {
                if(player.inventory[i].type == item.type && player.inventory[i].stack < item.maxStack)
                {
                    ans = Math.Min(item.stack, item.maxStack - player.inventory[i].stack);
                    item.stack -= ans;
                }
            }
            return ans;
        }
    }
}