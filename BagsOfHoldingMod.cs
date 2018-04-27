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
using Terraria.IO;
using Terraria.ModLoader.IO;

namespace JPANsBagsOfHoldingMod
{
    public class BagsOfHoldingMod : Mod
    {
        public static bool checkAccessories = true;
        public static bool checkVanity = false;
        public static bool checkHotbar = false;
        public static bool checkInventory = false;
        public static bool tooltipDisplay = false;

        public static bool tryPlace = true;
        public static bool playSoundOnPickup = false;

        public static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", "JPANsBagsOfHolding");
        public static string BagsConfigPath = Path.Combine(Main.SavePath, "Mod Configs", "JPANsBagsOfHolding", "Bags");

        static Preferences config = new Preferences(Path.Combine(ConfigPath, "main.json"));

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
            loadConfig();

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

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            int type = reader.ReadByte();

            if(type == 1)
            {
                int playerID = reader.ReadByte();
                int slot = reader.ReadByte();
                int chest = reader.ReadInt32();
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();
                TagCompound bagContents = TagIO.Read(reader);
                if (Main.netMode == NetmodeID.Server)
                {
                    GenericHoldingBag bag = Main.player[playerID].inventory[slot].modItem as GenericHoldingBag;
                    if(bag == null)
                        bag = new TrueOmniBag();

                    bag.items = bagContents;
                    if(chest > -1)
                    {
                        int trueChest = GenericHoldingBag.getChestAtTarget(x, y);
                        if (Main.chest[trueChest] != null && !GenericHoldingBag.IsPlayerInChest(trueChest) && !Chest.isLocked(Main.chest[trueChest].x, Main.chest[trueChest].y))
                        {
                            bag.emptyBagOnInventory(Main.player[playerID], Main.chest[trueChest].item, trueChest);
                        }
                    }else {
                        bag.emptyBagOnMagicStorage(Main.player[playerID], x, y);
                    }

                    bagContents = bag.items;
                }
                ModPacket p = GetPacket();
                p.Write((byte)2);
                p.Write((byte)playerID);
                p.Write((byte)slot);
                TagIO.Write(bagContents,p);
                p.Send(playerID);
                return;
            }

            if(type == 2)
            {
                int playerID = reader.ReadByte();
                int slot = reader.ReadByte();
                TagCompound bagContents = TagIO.Read(reader);
                GenericHoldingBag bag = Main.player[playerID].inventory[slot].modItem as GenericHoldingBag;
                if(bag != null)
                {
                    bag.items.Clear();
                    foreach (KeyValuePair<string, object> k in bagContents)
                    {
                         bag.items.Add(k);
                    }
                }
            }

            if(type == 3)
            {
                if(Main.netMode == NetmodeID.MultiplayerClient)
                {
                    int flags = reader.ReadByte();
                    BagPlayer pl = Main.player[Main.myPlayer].GetModPlayer<BagPlayer>();
                    pl.checkAccessories = (flags & 1) == 1;
                    pl.checkVanity = (flags & 2) == 2;
                    pl.checkHotbar = (flags & 4) == 4;
                    pl.checkInventory = (flags & 8) == 1;
                    pl.tryPlace = (flags & 16) == 1;
                }
            }
            /*
            if(type == 4) // check Order lists with server
            {
                if (Main.netMode == NetmodeID.Server) {
                    int player = reader.ReadByte();

                    ulong dirtBagHash = reader.ReadUInt64();
                    ulong oreBagHash = reader.ReadUInt64();
                    ulong gemBagHash = reader.ReadUInt64();

                    GenericHoldingBag bg = new DirtBag();
                    bool anyBag = false;
                    if (dirtBagHash != bg.getOrderHashCode())
                    {
                        sendBagOrder(player, bg);
                        anyBag = true;
                    }
                    bg = new OreBag();
                    if(oreBagHash != bg.getOrderHashCode())
                    {
                        sendBagOrder(player, bg);
                        anyBag = true;
                    }
                    bg = new GemBag();
                    if (gemBagHash != bg.getOrderHashCode())
                    {
                        sendBagOrder(player, bg);
                        anyBag = true;
                    }

                    if (anyBag)
                    {
                        ModPacket pk = GetPacket();
                        pk.Write((byte)5);
                        pk.Write((byte)new OmniBag().bagID);
                        pk.Send(player);
                    }

                }
                


            }

            if(type == 5) // Clear order and not pickup list for this bag
            {
                if(Main.netMode == NetmodeID.MultiplayerClient)
                {
                    GenericHoldingBag bg = getBagFromID(reader.ReadByte());
                    bg.order.Clear();
                    bg.preventPickup.Clear();
                }
            }
            if(type == 6) // add String to order list;
            {

            }
            if(type == 7) // add String to not pickup
            {

            }
            */
        }

        public void sendBagOrder(int player, GenericHoldingBag bg)
        {
            ModPacket pk = GetPacket();
            pk.Write((byte)5);
            pk.Write((byte)bg.bagID);
            pk.Send(player);
            for (int i = 0; i < bg.order.Count; i++)
            {
                pk = GetPacket();
                pk.Write((byte)6);
                pk.Write((byte)bg.bagID);
                pk.Write(bg.order[i]);
                pk.Send(player);
            }
            for(int i = 0; i < bg.preventPickup.Count; i++)
            {
                pk = GetPacket();
                pk.Write((byte)7);
                pk.Write((byte)bg.bagID);
                pk.Write(bg.preventPickup[i]);
                pk.Send(player);
            }
        }

        public GenericHoldingBag getBagFromID(int id)
        {
            switch (id)
            {
                case 1: return new DirtBag();
                case 2: return new OreBag();
                case 3: return new GemBag();
                case 4: return new OmniBag();
                case 5: return new BaitBag();
                case 6: return new CrateBag();
                case 7: return new FishBag();
                case 8: return new FishingBag();
                case 9: return new PlantBag();
                case 10: return new WoodBag();
                case 11: return new MushroomBag();
                case 12: return new DyeMaterialBag();
                case 13: return new NatureBag();
                case 14: return new CoinBag();
                case 15: return new TrueOmniBag();
                case 16: return new ChunkBag();
            }
            return null;
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

        public void loadConfig()
        {
            if (!System.IO.Directory.Exists(ConfigPath))
            {
                System.IO.Directory.CreateDirectory(ConfigPath);
            }
            if (!config.Load())
            {
                config.Put("checkNormalAccessoriesForBags", true);
                config.Put("checkSocialAccessoriesForBags", false);
                config.Put("checkHotbarForBags", false);
                config.Put("checkInventoryForBags", false);
                config.Put("tryPlaceItemInInventory", true);
                config.Put("playSoundOnPickup", false);
                config.Put("displayTagOnTooltip", false);
                config.Save(true);
            }
            checkAccessories = config.Get<bool>("checkNormalAccessoriesForBags", true);
            checkVanity = config.Get<bool>("checkSocialAccessoriesForBags", false);
            checkHotbar = config.Get<bool>("checkHotbarForBags", false);
            checkInventory = config.Get<bool>("checkInventoryForBags", false);
            tryPlace = config.Get<bool>("tryPlaceItemInInventory", true);
            playSoundOnPickup = config.Get<bool>("playSoundOnPickup", false);
            tooltipDisplay = config.Get<bool>("displayTagOnTooltip", false);

        }

        public override object Call(params object[] args) {

            /*if (args == null || args.Length < 3) {
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
            return true;*/
            return false;
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
        
        public static List<string> getStringListFromConfig(Preferences configuration, string tokenID)
        {
            List<string> ans = new List<string>();
            Newtonsoft.Json.Linq.JArray o = configuration.Get<Newtonsoft.Json.Linq.JArray>(tokenID, null);
            if (o != null)
            {
                foreach (Newtonsoft.Json.Linq.JToken j in o)
                {
                    ans.Add(j.ToString());
                }
            }
            return ans;
        }
	}
}