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

        public override bool ItemSpace(Item item, Player player)
        {
            string itmTag = GenericHoldingBag.ItemToTag(item);
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                if (player.armor[i] != null && player.armor[i].type != 0 && player.armor[i].modItem != null && player.armor[i].modItem is GenericHoldingBag)
                {
                    GenericHoldingBag bag = ((GenericHoldingBag)(player.armor[i].modItem));
                    if (bag.order.Contains(itmTag)){
                        if (bag.items.ContainsKey(itmTag))
                        {
                            return ((long)(bag.items[itmTag])) != Int64.MaxValue;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override bool OnPickup(Item item, Player player){
			try{
			for(int i = 3; i < 8 + player.extraAccessorySlots; i++){
				if(player.armor[i] != null && player.armor[i].type != 0 && player.armor[i].modItem != null && player.armor[i].modItem is GenericHoldingBag){
                        Item cln = new Item();
                        cln.SetDefaults(item.type);
                        long total = ((GenericHoldingBag)(player.armor[i].modItem)).addItem(item);
                        if(total > 0)
                        {
                            cln.position = new Microsoft.Xna.Framework.Vector2(item.position.X, item.position.Y + 30);
                            cln.stack = (int)total;
                            ItemText.NewText(cln, (int)total);
                        }
					if(item.stack <= 0)
						return false;
				}
			}
			
			/*cheatSheet extra accessory slots support*/
			
		/*	Mod cs = ModLoader.GetMod("CheatSheet");
			if(cs != null){
				ModPlayer pl = player.GetModPlayer(cs, "CheatSheetPlayer");
				int extraAccessories = (int)(pl.GetType().GetField("numberExtraAccessoriesEnabled").GetValue(pl));
				Item[] access = (Item[])(pl.GetType().GetField("ExtraAccessories").GetValue(pl));
				
				for(int i = 0; i < extraAccessories; i++){
					if(access[i] != null && access[i].type != 0 && access[i].modItem != null && access[i].modItem is GenericHoldingBag){
						((GenericHoldingBag)(access[i].modItem)).addItem(item);
						if(item.stack <= 0)
							return false;
					}
				}
			}
			*/
			return true;
			} catch (Exception ex){
				BagsOfHoldingMod.debugChat("Error: " + ex);
				return true;
			}
		}
	}
}