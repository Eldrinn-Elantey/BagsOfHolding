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
using Terraria.UI;

namespace JPANsBagsOfHoldingMod
{
    class BagPlayer: ModPlayer
    {

        public override void PreUpdate()
        {
            if (TrueOmniBag.contents == null)
            { TrueOmniBag.contents = new List<string>(); }
            if(TrueOmniBag.contents.Count == 0) { 
                try
                {
                    
                    Item itm = new Item();
                    for (int i = 1; i < ItemID.Count; i++)
                    {
                        itm.SetDefaults(i);
                        if (itm.maxStack > 1)
                        {
                            TrueOmniBag.contents.Add("" + i);
                        }
                    }
                    for (int i = ItemID.Count; i < ItemLoader.ItemCount; i++)
                    {
                        itm.SetDefaults(i);
                        if (itm.maxStack > 1)
                        {
                            TrueOmniBag.contents.Add(itm.modItem.mod.Name + ":" + itm.modItem.Name);
                        }

                    }
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
