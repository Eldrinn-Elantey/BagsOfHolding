using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JPANsBagsOfHoldingMod.Items;
using JPANsBagsOfHoldingMod.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace JPANsBagsOfHoldingMod
{
    class BagPlayer: ModPlayer
    {

        public bool checkAccessories = true;
        public bool checkVanity = false;
        public bool checkHotbar = false;
        public bool checkInventory = false;
        public bool tryPlace = true;

        public override void OnEnterWorld(Player player)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                checkAccessories = BagsOfHoldingMod.checkAccessories;
                checkVanity = BagsOfHoldingMod.checkVanity;
                checkHotbar = BagsOfHoldingMod.checkHotbar;
                checkInventory = BagsOfHoldingMod.checkInventory;
                tryPlace = BagsOfHoldingMod.tryPlace;
            }
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket pk = mod.GetPacket();
                pk.Write((byte)3);
                pk.Write((byte)(
                            (checkAccessories ? 1 : 0) |
                            (checkVanity ? 2 : 0) |
                            (checkHotbar ? 4 : 0) |
                            (checkInventory ? 8 : 0)|
                            (tryPlace ? 16 : 0) 
                            ));
                pk.Send(player.whoAmI);
            }
        }

        public override void PreUpdate()
        {
            reloadOmnibag();
            
        }

        public static void reloadOmnibag()
        {
            if (TrueOmniBag.contents2 == null)
            { TrueOmniBag.contents2 = new List<string>(); }
            if (TrueOmniBag.contents2.Count == 0)
            {
                try
                {

                    Item itm = new Item();
                    for (int i = 1; i < ItemID.Count; i++)
                    {
                        itm.SetDefaults(i);
                        if (itm.maxStack > 1)
                        {
                            TrueOmniBag.contents2.Add("" + i);
                        }
                    }
                    for (int i = ItemID.Count; i < ItemLoader.ItemCount; i++)
                    {
                        itm.SetDefaults(i);
                        if (itm.maxStack > 1)
                        {
                            TrueOmniBag.contents2.Add(itm.modItem.mod.Name + ":" + itm.modItem.Name);
                        }

                    }
                    if (TrueOmniBag.blacklist == null)
                    {
                        new TrueOmniBag().loadBagInfoFromConfig();
                    }
                    foreach (string k in TrueOmniBag.blacklist)
                    {
                        while (TrueOmniBag.contents2.Contains(k))
                        {
                            TrueOmniBag.contents2.Remove(k);
                        }
                    }

                    TrueOmniBag.isInit = true;
                }
                catch (Exception e)
                {
                    ErrorLogger.Log(e.ToString());
                }
            }
        }

        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            if ((context != ItemSlot.Context.InventoryItem && context != ItemSlot.Context.InventoryCoin && context != ItemSlot.Context.InventoryAmmo) || slot == 58)
            {
                return false;
            }
            if (GenericBagUI.visible)
            {
                GenericHoldingBag bag = GenericBagUI.openBag;
                if(bag != null)
                {
                    Item selected = inventory[slot];
                    int oldStack = selected.stack;
                    bag.addItem(selected);
                    
                    if (selected.stack <= 0)
                    {
                        inventory[slot] = new Item();
                    }
                    if(oldStack != selected.stack)
                    {
                        GenericBagUI.buildItem();
                    }
                    return true;
                }
            }
            return base.ShiftClickSlot(inventory, context, slot);
        }
    }
}
