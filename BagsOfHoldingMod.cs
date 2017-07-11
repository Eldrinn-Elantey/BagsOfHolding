using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Achievements;

using JPANsBagsOfHoldingMod.Items;
using JPANsBagsOfHoldingMod.UI;
using Terraria.Localization;
using System.Reflection;

namespace JPANsBagsOfHoldingMod
{
    public class BagsOfHoldingMod : Mod
    {
        public BagsOfHoldingMod()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
            };
        }

        private static UserInterface bagUI;
        public static GenericBagUI bagUIState;
        public override void Load()
        {
            base.Load();
            if (!Main.dedServ)
            {
                bagUIState = new GenericBagUI(this);
                bagUIState.Activate();
                bagUI = new UserInterface();
                bagUI.SetState(bagUIState);
            }
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));

            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "BagsOfHoldingMod: Bag UI",
                    delegate
                    {
                        if (GenericBagUI.visible && Main.recBigList == false)
                        {
                            bagUI.Update(Main._drawInterfaceGameTime);
                            bagUIState.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

            }
        }

        public override object Call(params object[] args) {

            if (args == null || args.Length < 3) {
                return false;
            }

            string call = args[0].ToString();
            string bag = args[1].ToString();

            List<string> items = new List<string>();
            //string[] items = new string[args.Length - 2];

            for (int i = 2; i < args.Length; i++) {
                if (args[i] is IEnumerable<Item>)
                {
                    foreach (Item itm in (args[i] as IEnumerable<Item>))
                    {
                        items.Add(GenericHoldingBag.ItemToTag(itm));
                    }
                    items.Add("");
                } else if (args[i] is IEnumerable<ModItem>)
                {
                    foreach (ModItem itm in (args[i] as IEnumerable<ModItem>))
                    {
                        items.Add(GenericHoldingBag.ItemToTag(itm.item));
                    }
                    items.Add("");
                }
                else if (args[i] is IEnumerable<string>)
                {
                    foreach (string itm in (args[i] as IEnumerable<string>))
                    {
                        items.Add(itm);
                    }
                    items.Add("");
                } else if (args[i] is string)
                {
                    items.Add((string)args[i]);
                } else if (args[i] is Item) {
                    items.Add(GenericHoldingBag.ItemToTag((Item)args[i]));
                } else if (args[i] is ModItem) {
                    items.Add(GenericHoldingBag.ItemToTag(((ModItem)args[i]).item));
                } else {
                    items[i] = "0";
                }
            }
            List<string> contents = null;
            try
            {
                Type bagClass = TypeInfo.GetType(bag);
                FieldInfo contInfo = null;
                try
                {
                    contInfo = bagClass.GetField("originalContents", BindingFlags.Public | BindingFlags.Static);
                    if (contInfo == null)
                        contInfo = bagClass.GetField("contents", BindingFlags.Public | BindingFlags.Static);
                }
                catch (ArgumentNullException anex)
                {
                    try
                    {
                        contInfo = bagClass.GetField("contents", BindingFlags.Public | BindingFlags.Static);
                    }
                    catch (ArgumentNullException anex2)
                    {
                        ErrorLogger.Log("BagsOfHolding Call error: Bag has no (static) contents field. Exception:\n" + anex2.ToString());
                        return false;
                    }
                    if (contInfo == null) return false;

                    contents = (List<string>)contInfo.GetValue(null);
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Log("BagsOfHolding Call error: Bag could not be found. Exception:\n" + e.ToString());
                return false;
            }
            if (contents == null)
            {
                ErrorLogger.Log("BagsOfHolding Call error: Bag has no (static) contents field (yet).");
                return false;
            }

            if (call == "addItem")
            {
                addItemToContents(contents, contents.Count, items);
            }
            else if (call == "addItemBefore")
            {
                int i = 0;
                while (i < items.Count)
                {
                    List<string> toAdd = new List<string>();
                    while (i < items.Count && items[i] != "")
                    {
                        toAdd.Add(items[i]);
                        i++;
                    }
                    while (i < items.Count && items[i] == "")
                    {
                        i++;
                    }
                    if (i >= items.Count)
                    {
                        ErrorLogger.Log("BagsOfHolding Call error: Add Item Before Has no \"Before\" item.");
                        return false;
                    }
                    string bookmark = items[i];
                    addItemToContents(contents, contents.IndexOf(bookmark), items);
                    while (i < items.Count && items[i] == "")
                    {
                        i++;
                    }
                }
            }
            else if (call == "addItemAfter")
            {
                int i = 0;
                while (i < items.Count)
                {
                    List<string> toAdd = new List<string>();
                    while (i < items.Count && items[i] != "")
                    {
                        toAdd.Add(items[i]);
                        i++;
                    }
                    while (i < items.Count && items[i] == "")
                    {
                        i++;
                    }
                    if (i >= items.Count)
                    {
                        ErrorLogger.Log("BagsOfHolding Call error: Add Item Before Has no \"Before\" item.");
                        return false;
                    }
                    string bookmark = items[i];
                    addItemToContents(contents, contents.IndexOf(bookmark), items);
                    while (i < items.Count && items[i] == "")
                    {
                        i++;
                    }
                }
            }
            else if (call == "removeItem")
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] != "" && items[i] != "0")
                        contents.Remove(items[i]);
                }
            }
            return true;
        }

        public void addItemToContents(List<string> contents, int index, List<string> items)
        {
            if (contents == null) return;
            if(index < 0)
            {
                index = contents.Count;
            }
            if(index != contents.Count)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    contents.Insert(index, items[i]);
                    index++;
                }
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    contents.Add(items[i]);
                }
            }
        }

        public static void sendChat(string msg){
			sendChat(msg, Color.White);
		}
		
		public static void debugChat(string msg){
			sendChat(msg, Color.Yellow);
		}
		
		public static void sendChatToAll(string msg){
			sendChatToAll(msg, Color.White);
		}
		
		public static void sendChat(string msg, Color c){
			if (Main.netMode == 0 || Main.netMode == 1) // Single Player
			{
				string[] toSend = msg.Split('\n');
				for(int i = 0; i< toSend.Length; i++){
					toSend[i] = toSend[i].Trim();
					if(toSend[i] != "")
						Main.NewText(toSend[i], c.R, c.G, c.B);
				}
			}else{
				Console.WriteLine(msg);
			}
		}
		
		public static void sendChatToAll(string msg, Color c){
			if (Main.netMode == 2) // Server
			{
			string[] toSend = msg.Split('\n');
				for(int i = 0; i< toSend.Length; i++){
					toSend[i] = toSend[i].Trim();
                    if (toSend[i] != "")
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(toSend[i]),c);
				}
				
			}else if (Main.netMode == 1)
            {
                string[] toSend = msg.Split('\n');
                for (int i = 0; i < toSend.Length; i++)
                {
                    toSend[i] = toSend[i].Trim();
                    if (toSend[i] != "")
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(toSend[i]), c, Main.myPlayer);
                }
            }
			else if (Main.netMode == 0) // Single Player
			{
				string[] toSend = msg.Split('\n');
				for(int i = 0; i< toSend.Length; i++){
					toSend[i] = toSend[i].Trim();
					if(toSend[i] != "")
						Main.NewText(toSend[i], c.R, c.G, c.B);
				}
			}
		}		
	}
}