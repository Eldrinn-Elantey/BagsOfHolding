using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using JPANsBagsOfHoldingMod.UI;

using System.Reflection;
using Terraria.IO;

namespace JPANsBagsOfHoldingMod.Items
{
	public abstract class GenericHoldingBag : ModItem
	{
		
		public override bool CloneNewInstances{
			get{return true;}
		}

        public int bagID = 0;
        public Preferences config;

		public List<string> order;
        public List<string> preventPickup;

        public bool disableBag = false;
        public bool leftClickOnFloor = true;
        public bool leftClickOnChest = true;
        public bool leftClickOnPiggyBank = true;

        public TagCompound items;

        public override void SetStaticDefaults()
        {
           DisplayName.SetDefault("Generic Holding Bag");
            
        }

        public override void SetDefaults()
		{
            item.width = 24;
            item.height = 24;
            item.maxStack = 1;
            //	AddTooltip("Automatically stores some items in the bag for you if equipped as an accessory.");
            //	AddTooltip2("Use to retrieve the items.");
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 2;
            item.accessory = true;
            item.autoReuse = true;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 1;
            setupItemList();
		}
		
		public virtual void setupItemList(){
            basestSeupItemList();
			/*	
			items = new TagCompound();
			foreach(string s in order){
				if(!items.HasTag(s))
					items[s] = (long)0L;
			}*/
		}

        public void basestSeupItemList()
        {
            if (order == null)
                order = new List<string>();
            if (preventPickup == null)
                preventPickup = new List<string>();
            if (items == null)
                items = new TagCompound();
            loadBagInfoFromConfig();
        }

        public virtual void remakeFromConfig()
        {

        }

        

        public override void UpdateAccessory(Player player, bool hideVisual){
		
		}
		
		public override bool CanUseItem(Player player){
			return true;
		}
		
		public bool HasContent(){
			if(items != null){
				foreach(KeyValuePair<string, object> itm in items){
					if(((long)itm.Value) > 0)
						return true;
				}
			}
			return false;
		}
		
		public override bool UseItem(Player p){
            //if(p.selectedItem == 58)
            //return false;
            //Player owner = Main.player[item.owner];

            if (Main.netMode == NetmodeID.Server || p.whoAmI != Main.myPlayer)
                return true;

			if(order == null)
				setupItemList();
			
			if(p.altFunctionUse == 2){
				if(GenericBagUI.visible){
					BagsOfHoldingMod.bagUIState.close(false);
				}else{
					BagsOfHoldingMod.bagUIState.open(p, this, false);
				}
			}else if((leftClickOnPiggyBank || leftClickOnChest || leftClickOnFloor) && HasContent()){

                if (leftClickOnPiggyBank || leftClickOnChest)
                {
                    int chest = getChestAtTarget(p);
                    if (chest >= 0)
                    {
                        ErrorLogger.Log("Chest is no. " + chest);
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            ModPacket pack = mod.GetPacket();
                            pack.Write((byte)1);
                            pack.Write((byte)p.whoAmI);
                            pack.Write((byte)p.selectedItem);
                            pack.Write((int)chest);
                            pack.Write((int)Player.tileTargetX);
                            pack.Write((int)Player.tileTargetY);
                            TagIO.Write(items, pack);
                            pack.Send();
                        }
                        else if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            if (Main.chest[chest] != null && !IsPlayerInChest(chest) && !Chest.isLocked(Main.chest[chest].x, Main.chest[chest].y))
                            {
                                emptyBagOnChest(p, chest);
                            }
                        }
                        return true;
                    }
                    else if (chest < -1 && chest > -5)
                    {
                        if (p.chest != chest)
                        {
                            if (Main.netMode != NetmodeID.Server)
                            {
                                emptyBagOnBank(p, chest);
                            }
                            return true;
                        }
                    }
                    else if (chest == Int32.MinValue)
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            ModPacket pack = mod.GetPacket();
                            pack.Write((byte)1);
                            pack.Write((byte)p.whoAmI);
                            pack.Write((byte)p.selectedItem);
                            pack.Write((int)chest);
                            pack.Write((int)Player.tileTargetX);
                            pack.Write((int)Player.tileTargetY);
                            TagIO.Write(items, pack);
                            pack.Send();
                        }
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            emptyBagOnMagicStorage(p, Player.tileTargetX, Player.tileTargetY);
                        }
                    }
                    else
                    {
                        if (leftClickOnFloor)
                        {
                            emptyBagOnFloor(p);
                        }
                    }
                }else
                {
                    if (leftClickOnFloor)
                    {
                        emptyBagOnFloor(p);
                    }
                }

				if(GenericBagUI.visible && GenericBagUI.openBag == this)
					GenericBagUI.buildItem();
			}	
			return true;
		}
		
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }




        public static int getChestAtTarget(Player p)
        {
            Player.tileTargetX = (int)(((float)Main.mouseX + Main.screenPosition.X) / 16f);
            Player.tileTargetY = (int)(((float)Main.mouseY + Main.screenPosition.Y) / 16f);
            if (p.gravDir == -1f)
            {
                Player.tileTargetY = (int)((Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY) / 16f);
            }
            int x = Player.tileTargetX;
            int y = Player.tileTargetY;
            int pxCenter = (int)((p.position.X + (float)(p.width / 2)) / 16f);
            int pyCenter = (int)((p.position.Y + (float)(p.height / 2)) / 16f);

            int distanceX = x - pxCenter;
            distanceX = distanceX < 0 ? -distanceX : distanceX;
            int distanceY = y - pyCenter;
            distanceY = distanceY < 0 ? -distanceY : distanceY;

            if (Player.tileRangeX >= distanceX && Player.tileRangeY >= distanceY)
            {
                return getChestAtTarget(x, y);
            }
            return -1;
        }

        public static int getChestAtTarget(int x, int y)
        {
           
           if (Main.tile[x, y].type == TileID.PiggyBank)
           {
                return -2;
           }
           if (Main.tile[x, y].type == TileID.Safes)
           {
               return -3;
           }
           if (Main.tile[x, y].type == TileID.DefendersForge)
           {
               return -4;
           }
            int chest = Chest.FindChest(x, y);
            if (chest == -1)
            {
                chest = Chest.FindChest(x - 1, y);
            }
            if (chest == -1)
            {
                chest = Chest.FindChest(x - 1, y - 1);
            }
            if (chest == -1)
            {
                chest = Chest.FindChest(x, y - 1);
            }
            Mod magicStorage = ModLoader.GetMod("MagicStorage");
            if (magicStorage != null)
            {
                ModTile t = TileLoader.GetTile(Main.tile[x, y].type);
                if (t != null && ((t.GetType() == magicStorage.GetTile("StorageAccess").GetType()) || t.GetType().IsSubclassOf(magicStorage.GetTile("StorageAccess").GetType())))
                {
                    return Int32.MinValue;
                }
            }
            
            return chest;			
		}
		
		
		public virtual void emptyBagOnFloor(Player p){
			int stackSize = 0;
			
			/*TagCompound result = new TagCompound();*/
			long remainingOre = 0;
			int ffs = firstFreeItemSlot();
			if (ffs >= 396)
				return;
			foreach(String key in order){
				if(items.ContainsKey(key)){
					remainingOre = items.GetAsLong(key);
					Item i = getItemFromTag(key);
					if(i.type != 0){
						while(remainingOre > 0 && ( ffs < 396)){
							stackSize = (int)Math.Min(remainingOre, i.maxStack);
							p.QuickSpawnItem(i.type, stackSize);
							remainingOre -= stackSize;
							ffs++;
						}
					items.Remove(key);
					if(remainingOre > 0)
						items[key] = (long)remainingOre;
					}
				}
			}
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket pack = mod.GetPacket();
                pack.Write((byte)2);
                pack.Write((byte)p.whoAmI);
                pack.Write((byte)p.selectedItem);
                TagIO.Write(items, pack);
                pack.Send();
            }
            recalculateValue();
		}
		
        public virtual void emptyBagOnInventory(Player p, Item[] inv, int chest)
        {
            

            //TagCompound result = new TagCompound();
            

            foreach (String key in order)
            {
                placeItemInInventory(key, inv, chest);
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket pack = mod.GetPacket();
                pack.Write((byte)2);
                pack.Write((byte)p.whoAmI);
                pack.Write((byte)p.selectedItem);
                TagIO.Write(items, pack);
                pack.Send();
            }
            recalculateValue();
        }

        public virtual void placeItemInInventory(string key, Item[] inv, int chest)
        {
            long remainingOre = 0;
            int stackSize = 0;

            if (items.ContainsKey(key))
            {
                remainingOre = items.GetAsLong(key);
                Item itm = getItemFromTag(key);
                if (itm.type != 0)
                {
                    for (int i = 0; i < 40 && remainingOre > 0; i++)
                    {
                        if (inv[i] != null && inv[i].type == itm.type)
                        {
                            stackSize = (int)Math.Min(remainingOre, itm.maxStack - inv[i].stack);
                            remainingOre -= stackSize;
                            inv[i].stack += stackSize;
                            if (Main.netMode != NetmodeID.SinglePlayer && chest > -1)
                            {
                                NetMessage.SendData(32, -1, -1, null, chest, (float)i, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                    for (int i = 0; i < 40 && remainingOre > 0; i++)
                    {
                        if (inv[i] == null || inv[i].type == 0)
                        {
                            inv[i] = getItemFromTag(key);
                            stackSize = (int)Math.Min(remainingOre, itm.maxStack);
                            remainingOre -= stackSize;
                            inv[i].stack = stackSize;
                            if (Main.netMode != NetmodeID.SinglePlayer && chest > -1)
                            {
                                NetMessage.SendData(32, -1, -1, null, chest, (float)i, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                    items.Remove(key);
                    if (remainingOre > 0)
                        items[key] = (long)remainingOre;
                }
            }
        }

        public virtual void emptyBagOnMagicStorage(Player p, int x, int y)
        {
            
            Mod magicStorage = ModLoader.GetMod("MagicStorage");
            if (magicStorage != null)
            {
                try
                {
                    if (Main.tile[x, y].frameX % 36 == 18)
                    {
                        x--;
                    }
                    if (Main.tile[x, y].frameY % 36 == 18)
                    {
                        y--;
                    }
                    ModTile t = TileLoader.GetTile(Main.tile[x, y].type);
                    MethodInfo getHeart = (t.GetType()).GetMethod("GetHeart", BindingFlags.Public | BindingFlags.Instance,null,new Type[] { typeof(int), typeof(int)},null);
                    if (getHeart == null)
                    {
                        BagsOfHoldingMod.debugChat("GetHeart() is null. Report to author.");
                        ErrorLogger.Log("GetHeart() is null on " + (Main.netMode == NetmodeID.MultiplayerClient ? "client" : "server"));
                        return;
                    }
                    object[] param = new object[2];
                    param[0] = x;
                    param[1] = y;
                        TileEntity heart = (TileEntity)getHeart.Invoke(t,param);
                    if (heart == null)
                    {
                        BagsOfHoldingMod.sendChat("This Access does not have an associated Storage Heart.");
                        return;
                    }
                        Type[] depCal = new Type[1];
                        depCal[0] = typeof(Item);
                        MethodInfo deposit = heart.GetType().GetMethod("DepositItem", BindingFlags.Public | BindingFlags.Instance,null, depCal,null);
                        if (deposit == null)
                        {
                            BagsOfHoldingMod.debugChat("DepositItem(Item) is null. Report to author.");
                            ErrorLogger.Log("DepositItem(Item) is null on " + (Main.netMode == NetmodeID.MultiplayerClient ? "client" : "server"));
                        return;
                        }

                        for (int i = 0; i < order.Count; i++)
                        {
                            if (items.ContainsKey(order[i]))
                            {
                                long remain = items.GetAsLong(order[i]);
                                bool prematureBreak = false;
                                while (remain > 0 && !prematureBreak)
                                {
                                    Item itm = getItemFromTag(order[i]);
                                    int stackSize = (int)Math.Min(remain, itm.maxStack);
                                    itm.stack = stackSize;
                                    deposit.Invoke(heart,new object[]{itm});
                                    if (itm.IsAir)
                                    {
                                        remain -= stackSize;
                                    }else
                                    {
                                        remain -= (stackSize - itm.stack);
                                        prematureBreak = true;
                                    }
                                }
                                items.Remove(order[i]);
                                if(remain > 0)
                                    items[order[i]] = remain;
                            }
                        }
                }catch(Exception e)
                {
                    BagsOfHoldingMod.debugChat(e.ToString());
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket pack = mod.GetPacket();
                    pack.Write((byte)2);
                    pack.Write((byte)p.whoAmI);
                    pack.Write((byte)p.selectedItem);
                    TagIO.Write(items, pack);
                    pack.Send();
                }
            }
        }

		public void emptyBagOnChest(Player p, int chest){
            emptyBagOnInventory(p, Main.chest[chest].item, chest);
		}

        public void emptyBagOnBank(Player p, int chest)
        {
            switch (chest)
            {
                case -2: emptyBagOnInventory(p, p.bank.item, chest);
                    break;
                case -3:
                    emptyBagOnInventory(p, p.bank2.item, chest);
                    break;
                case -4:
                    emptyBagOnInventory(p, p.bank3.item, chest);
                    break;
                default:
                    break;
            }
        }

		public override TagCompound Save()
		{
          
                TagCompound s = new TagCompound();
                if (items == null || items.Count <= 0 || order == null)
                    return s;
                for (int i = 0; i < order.Count; i++)
                {
                    if (items.ContainsKey(order[i]))
                    {
                        s[order[i]] = items[order[i]];
                    }
                }
                return s;
          
		}

		public override void Load(TagCompound tag)
		{
			items = tag;
            if (items == null)
                items = new TagCompound();
			/*setupItemList();
			foreach(KeyValuePair<string, object> itm in tag){
				items[itm.Key] = null;
				items[itm.Key] = (long)itm.Value;
			}*/
		}

		public override void NetSend(BinaryWriter writer){
			TagIO.Write(Save(), writer);
		}
		
		public override void NetRecieve(BinaryReader reader){
			Load(TagIO.Read(reader));
		}
		
		
		public override void LoadLegacy(BinaryReader reader)
		{
		}
		
		public override ModItem Clone(){
			GenericHoldingBag cln = (GenericHoldingBag)this.MemberwiseClone();
            cln.items = null;
            cln.SetDefaults();
            cln.order = this.order;
            cln.preventPickup = this.preventPickup;
			cln.items = new TagCompound();
            if (items != null && items.Count > 0)
            {
                for(int i = 0; i< order.Count; i++)
                {
                    if (items.ContainsKey(order[i])){
                        cln.items[order[i]] = items[order[i]];
                    }
                }
            }
			return cln;
		}
		
		
        public bool canPickupItem(string itmTag)
        {
            if (preventPickup.Contains(itmTag))
            {
                return false;
            }
            return canHold(itmTag);
        }

        public bool canHold(string itmTag)
        {
            if (order.Contains(itmTag))
            {
                if (items.ContainsKey(itmTag))
                {
                    return ((long)(items[itmTag])) != Int64.MaxValue;
                }
                return true;
            }
            return false;
        }

		public virtual long addItem(Item itm){
			if(order == null || order.Count == 0)
				setupItemList();
			try{
				if(itm.stack <= 0)
					return 0;
			string type = ItemToTag(itm);
			bool isToInsert = order.Contains(type);
			if(isToInsert){
				long total = itm.stack;
				if(items.ContainsKey(type))
					total += ((long)items[type]);
				
				if (total < 0){
					long remain = Int64.MaxValue - items.GetAsLong(type);
                    items.Remove(type);
					items[type] = Int64.MaxValue;
					itm.stack = (int)(itm.stack - remain);
					recalculateValue();
					return remain;
				}
                long stored = itm.stack;
                items.Remove(type);
                items[type] = (long)total;
				itm.stack = 0;
				itm.type = 0;
				recalculateValue();
				return stored;
			}else{
                    //BagsOfHoldingMod.debugChat("Item not found: " + type);
                    return 0;
			}
			}catch (Exception ex){
				BagsOfHoldingMod.debugChat("Error: " + ex);
                return 0;
			}
			
		}
		
		public void recalculateValue(){
			long val = Item.sellPrice(0,0,10,0);
            foreach(String key in order)
            {
                if(items.ContainsKey(key) && items.GetAsLong(key) != 0)
                {
                    Item i = getItemFromTag(key, true);
                    val += i.value * items.GetAsLong(key);
                    if(val < 0 || val >= 99999999L){
                        item.value = 99999999;
                        return;
                    }
                }
            }
            if (val < 0 || val >= 99999999L)
            {
                item.value = 99999999;
                return;
            }
            item.value = (int)val;
		}
		
		public static int getTypeFromTag(string tag){
            int type =  0;
            if(!Int32.TryParse(tag, out type)) { 
				Mod m = ModLoader.GetMod(tag.Split(':')[0]);
				if(m != null)
					type = m.ItemType(tag.Split(':')[1]);
			}
			return type;
		}
		
		public static Item getItemFromTag(string tag, bool noMatCheck = false){
			Item ret = new Item();
            int type = getTypeFromTag(tag);
            if(type != 0)
                ret.SetDefaults(type, noMatCheck);
			return ret;
		}

        public static string ItemToTag(Item itm){
			String type = "" + itm.type;
			if(itm.modItem != null){
				type = itm.modItem.mod.Name+":"+itm.modItem.Name;
			}
			
			return type;
		}
		
		public static bool IsPlayerInChest(int i)
		{
			for (int j = 0; j < 255; j++)
			{
				if (Main.player[j].chest == i)
				{
					return true;
				}
			}
			return false;
		}

		public static int firstFreeItemSlot(){
			for (int j = 0; j < 400; j++)
			{
				if (!Main.item[j].active && Main.itemLockoutTime[j] == 0)
				{
					return j;
				}
			}
			return 400;
		}

        public virtual void loadConfig()
        {
            if (config == null)
            {
                config = new Preferences(Path.Combine(BagsOfHoldingMod.BagsConfigPath, GetType().Name + ".json"));
                if (!config.Load())
                {
                    config.Put("disableBag", disableBag);
                    config.Put("leftClickOnFloor", leftClickOnFloor);
                    config.Put("leftClickOnChest", leftClickOnChest);
                    config.Put("leftClickOnPiggyBank", leftClickOnPiggyBank);
                    createDefaultItemList();
                    config.Put("order", order.ToArray());
                    config.Put("noPickup", preventPickup.ToArray());
                    config.Save();
                    return;
                }
            }
        }

        public virtual void loadBagInfoFromConfig()
        {
           loadConfig();
           loadLeftClickFromConfig();
           order = BagsOfHoldingMod.getStringListFromConfig(config, "order");
           if(order.Count == 0 && createDefaultItemList())
           {
                config.Put("order", order.ToArray());
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    config.Save();
                }
           }
           preventPickup = BagsOfHoldingMod.getStringListFromConfig(config, "noPickup");
        }
        
        public virtual void loadLeftClickFromConfig()
        {
            disableBag = config.Get<bool>("disableBag", disableBag);
            leftClickOnFloor = config.Get<bool>("leftClickOnFloor", leftClickOnFloor);
            leftClickOnChest = config.Get<bool>("leftClickOnChest", leftClickOnChest);
            leftClickOnPiggyBank = config.Get<bool>("leftClickOnPiggyBank", leftClickOnPiggyBank);
        }

        public virtual bool createDefaultItemList()
        {
            return false;
        }	

        public ulong getOrderHashCode()
        {
            ulong ans = 0;
            for(int i = 0; i< order.Count; i++)
            {
                ans = ans + (uint)(order[i].GetHashCode());
            }
            return ans;
        }
	}
}