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
using Terraria.Localization;
using Terraria.IO;

namespace JPANsBagsOfHoldingMod.Items
{
	public class CoinBag : GenericHoldingBag
	{
        public static List<string> contents;
        public static List<string> noPickup;

        public override bool createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
            order.Add(""+ItemID.CopperCoin);
			order.Add(""+ItemID.SilverCoin);
			order.Add(""+ItemID.GoldCoin);
			order.Add(""+ItemID.PlatinumCoin);

            order.Add("" + ItemID.DefenderMedal);

            //thorium support
            order.Add("ThoriumMod:VanquisherMedal");

            //expeditions support
            order.Add("Expeditions:BountyVoucher");

            //imksushi mod support
            order.Add("imkSushisMod:LootStartToken");
            order.Add("imkSushisMod:LootGoblinsToken");
            order.Add("imkSushisMod:LootSkeletronToken");
            order.Add("imkSushisMod:LootHardmodeToken");
            order.Add("imkSushisMod:LootPiratesToken");
            order.Add("imkSushisMod:LootMechToken");
            order.Add("imkSushisMod:LootPlanteraToken");
            order.Add("imkSushisMod:LootMartiansToken");

            order.Add("imkSushisMod:SurfacePurityStartToken");
            order.Add("imkSushisMod:SurfacePurityEocToken");
            order.Add("imkSushisMod:SpacePurityStartToken");
            order.Add("imkSushisMod:SpacePurityHardmodeToken");
            order.Add("imkSushisMod:SurfaceDesertStartToken");
            order.Add("imkSushisMod:UndergroundPurityStartToken");
            order.Add("imkSushisMod:UndergroundJungleStartToken");
            order.Add("imkSushisMod:UndergroundSnowStartToken");
            order.Add("imkSushisMod:UndergroundCorruptionEocToken");
            order.Add("imkSushisMod:UndergroundCrimsonEocToken");
            order.Add("imkSushisMod:UndergroundDungeonSkeletronToken");
            order.Add("imkSushisMod:UnderworldHellSkeletronToken");
            order.Add("imkSushisMod:UnderwaterOceanStartToken");
            order.Add("imkSushisMod:FishingStartToken");
            order.Add("imkSushisMod:FishingHardmodeToken");
            order.Add("imkSushisMod:TempleJunglePlanteraToken");
            order.Add("imkSushisMod:SwapToken");

            order.Add("ExperienceAndClasses:Boss_Orb");
            order.Add("ExperienceAndClasses:Monster_Orb");

            return true;
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Coin Bag");
            Tooltip.SetDefault("Automatically stores picked up coins in the bag for you if equipped as an accessory. Tries to store the highest possible coins.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de pièces");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les pièces si le sac est équippé dans les accessoires, essayez d'en récupérer un maximum.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Moedas");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda modedas dentro do saco automáticamente se apanhadas enquanto equipado como acessório. Tenta guardar como o mínimo de moedas possíves.");
        }

        public override void SetDefaults()
		{
            base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}


        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 14;
            if (bagConfig == null)
            {
                remakeFromConfig();
            }
            else
            {
                if (items == null)
                    items = new TagCompound();
                config = bagConfig;
                order = contents;
                preventPickup = noPickup;
                loadLeftClickFromConfig();
            }
        }

        public override void remakeFromConfig()
        {
            base.setupItemList();
            if (contents == null)
            {
                contents = new List<string>();
            }
            else
            {
                contents.Clear();
            }
            contents.AddRange(order);
            if (noPickup == null)
            {
                noPickup = new List<string>();
            }
            else
            {
                noPickup.Clear();
            }
            noPickup.AddRange(preventPickup);
            if (bagConfig == null)
            {
                bagConfig = config;
            }
            else
            {
                foreach (string k in config.GetAllKeys())
                {
                    bagConfig.Put(k, config.Get<object>(k, null));
                }
                bagConfig.Save();
            }
        }

        public override void AddRecipes(){
            if (!disableBag)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.Silk, 15);
                recipe.AddIngredient(ItemID.FallenStar, 1);
                recipe.SetResult(this, 1);
                recipe.AddTile(TileID.WorkBenches);
                recipe.AddRecipe();
            }
		}

        public Item[] condenseCoins(Item[] inv)
        {
            Item[] ans = new Item[4];
            for (int i = 0; i < 4; i++)
            {
                ans[i] = new Item();
                ans[i].SetDefaults(ItemID.CopperCoin + i);
                ans[i].stack = 0;
            }
            List<int> toRemove = new List<int>();
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (inv[i].type == ans[j].type)
                    {
                        ans[j].stack += inv[i].stack;
                        toRemove.Add(i);
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                ans[i + 1].stack = ans[i + 1].stack + (ans[i].stack / 100);
                ans[i].stack = ans[i].stack % 100;
            }

            foreach(int i in toRemove)
            {
                inv[i] = new Item();
                inv[i].SetDefaults(0);
            }
            return ans;
        }

        public override void emptyBagOnInventory(Player p, Item[] inv, int chest)
        {
            //first, maximize space by taking all coins in the inventory and condense them in 4 stacks with unlimited value
            Item[] coins = condenseCoins(inv);
            //next, add the coins to the total (pouch): try adding them to lower coin value (100 copper if 1 silver) if it does not fit
            for(int i = coins.Length-1; i>= 0; i--)
            {
                if (coins[i].stack > 0)
                {
                    addItem(coins[i]);
                    if (coins[i].stack > 0 && i > 0)
                    {
                        coins[i - 1].stack = coins[i].stack * 100;
                        i = i-2;
                    }
                }
            }
            //make regular coin stacks (except platinum, which is still unlimited)
            for (int i = 0; i < 3; i++)
            {
                coins[i + 1].stack = coins[i + 1].stack + (coins[i].stack / 100);
                coins[i].stack = coins[i].stack % 100;
            }
            //dump the non-fitting coins on the inventory (or if something goes wrong, the player)
            for(int i = 0; i < 4; i++)
            {
                tryAddtoInventory(inv, coins[i]);
                if(coins[i].stack > 0)
                {
                    p.QuickSpawnItem(coins[i], coins[i].stack);
                }
            }

            //now, try to fill inventory with the biggest coin type to the smallest

            emptyPlatinum(inv);
            emptyGold(inv);
            emptySilver(inv);
            emptyCopper(inv);
            
            foreach(string key in order)
            {
                if(key != "" + ItemID.PlatinumCoin && key != "" + ItemID.GoldCoin &&
                   key != "" + ItemID.SilverCoin && key != "" + ItemID.CopperCoin)
                {
                    placeItemInInventory(key, inv, chest);
                }
            }
        }

        private void emptyPlatinum(Item[] inv)
        {
            if (!emptyCoins(inv, "" + ItemID.PlatinumCoin, "" + ItemID.PlatinumCoin, 1))
                return;
            if (!emptyCoins(inv, "" + ItemID.PlatinumCoin, "" + ItemID.GoldCoin, 100))
                return;
            if (!emptyCoins(inv, "" + ItemID.PlatinumCoin, "" + ItemID.SilverCoin, 10000))
                return;
            if (!emptyCoins(inv, "" + ItemID.PlatinumCoin, "" + ItemID.CopperCoin, 1000000))
                return;
        }

        private void emptyGold(Item[] inv)
        {
            if (!emptyCoins(inv, "" + ItemID.GoldCoin, "" + ItemID.GoldCoin, 1))
                return;
            if (!emptyCoins(inv, "" + ItemID.GoldCoin, "" + ItemID.SilverCoin, 100))
                return;
            if (!emptyCoins(inv, "" + ItemID.GoldCoin, "" + ItemID.CopperCoin, 10000))
                return;
        }

        private void emptySilver(Item[] inv)
        {
            if (!emptyCoins(inv, "" + ItemID.SilverCoin, "" + ItemID.SilverCoin, 1))
                return;
            if (!emptyCoins(inv, "" + ItemID.SilverCoin, "" + ItemID.CopperCoin, 100))
                return;
        }

        private void emptyCopper(Item[] inv)
        {
            emptyCoins(inv, "" + ItemID.CopperCoin, "" + ItemID.CopperCoin, 1);
        }

        public bool emptyCoins(Item[] inv, string itmTag, string tag, int mult)
        {
            long totalCoin = items.ContainsKey(tag) ? (items.GetAsLong(tag) / mult) : 0;
            Item itm = getItemFromTag(itmTag);
            while (totalCoin> 0)
            {
                itm.stack = (int)Math.Min(totalCoin, itm.maxStack);
                totalCoin -= itm.stack;
                tryAddtoInventory(inv, itm);
                if (itm.stack > 0)
                {
                    totalCoin += itm.stack;
                    totalCoin *= mult;
                    totalCoin += items.ContainsKey(tag) ? (items.GetAsLong(tag) % mult) : 0;
                    if (items.ContainsKey(tag))
                        items.Remove(tag);
                    items[tag] = totalCoin;
                    return false;
                }
            }
            totalCoin = items.ContainsKey(tag) ? (items.GetAsLong(tag) % mult) : 0;
            if (items.ContainsKey(tag))
                items.Remove(tag);
            items[tag] = totalCoin;
            return true;
        }

        private void tryAddtoInventory(Item[] inv, Item itm)
        {
            for (int j = 0; j < inv.Length; j++)
            {
                if(inv[j] != null && inv[j].type == itm.type && inv[j].stack < inv[j].maxStack)
                {
                    int rem = inv[j].maxStack - inv[j].stack;
                    if(rem > itm.stack)
                    {
                        inv[j].stack += itm.stack;
                        itm.stack = 0;
                        return;
                    }else
                    {
                        itm.stack -= rem;
                        inv[j].stack = inv[j].maxStack;
                    }
                }
            }

            for (int j = 0; j < inv.Length; j++)
            {
                if (inv[j] == null || inv[j].type == 0 || inv[j].stack <= 0)
                {
                    inv[j] = new Item();
                    inv[j].SetDefaults(itm.type);
                    inv[j].stack = Math.Min(itm.stack, itm.maxStack);
                    itm.stack -= inv[j].stack;
                    if(itm.stack <= 0)
                        return;
                }
            }
        }

        private long addItemToDown(Item itm)
        {
            if (itm.stack <= 0)
                return 0;
            string type = ItemToTag(itm);
            bool isToInsert = order.Contains(type);
            if (isToInsert)
            {
                long total = itm.stack;
                if (items.ContainsKey(type))
                    total += ((long)items[type]);

                if (total < 0)
                {
                    if(itm.type != ItemID.CopperCoin)
                    {
                        Item nItm = new Item();
                        nItm.SetDefaults(itm.type - 1);
                        nItm.stack = itm.stack * 100;
                        long ret = addItemToDown(nItm);
                        itm.stack = nItm.stack / 100;
                        if (itm.stack <= 0)
                            return ret/100;
                    }
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
            }
            return 0;
        }

        public override long addItem(Item itm)
        {
            if (order == null)
                setupItemList();
            try
            {
                string itmTag = ItemToTag(itm);
                if (itmTag == "" + ItemID.PlatinumCoin || itmTag == "" + ItemID.GoldCoin ||
                  itmTag == "" + ItemID.SilverCoin || itmTag == "" + ItemID.CopperCoin)
                {
                    long ans = addItemToDown(itm);
                    if (ans > 0)
                    {
                        rebalanceCoins();
                    }
                    return ans;
                }else
                {
                    return base.addItem(itm);
                }
            }
            catch (Exception ex)
            {
                BagsOfHoldingMod.debugChat("Error: " + ex);
                return 0;
            }
        }

        private void rebalanceCoins()
        {
            long totalCopper = items.ContainsKey(""+ItemID.CopperCoin) ? items.GetAsLong("" + ItemID.CopperCoin)  : 0;
            long totalSilver = items.ContainsKey("" + ItemID.SilverCoin) ? items.GetAsLong("" + ItemID.SilverCoin) : 0;
            long totalGold = items.ContainsKey("" + ItemID.GoldCoin) ? items.GetAsLong("" + ItemID.GoldCoin) : 0;
            long totalPlatinum = items.ContainsKey("" + ItemID.PlatinumCoin) ? items.GetAsLong("" + ItemID.PlatinumCoin) : 0;

            //copper compression
            if (totalCopper >= 100)
            {
                if ((totalCopper / 100) + totalSilver > 0)
                {
                    totalSilver += (totalCopper / 100);
                    totalCopper = totalCopper % 100;
                }else
                {
                   if(totalSilver < Int64.MaxValue)
                    {
                        long rem = Int64.MaxValue - totalSilver;
                        totalCopper -= rem * 100;
                        totalSilver = Int64.MaxValue;
                    }
                }
                if (totalCopper >= 10000)
                {
                    if ((totalCopper / 10000) + totalGold > 0)
                    {
                        totalGold += (totalCopper / 10000);
                        totalCopper = totalCopper % 10000;
                    }else
                    {
                        if (totalGold < Int64.MaxValue)
                        {
                            long rem = Int64.MaxValue - totalGold;
                            totalCopper -= rem * 10000;
                            totalGold = Int64.MaxValue;
                        }
                    }
                    if (totalCopper >= 1000000)
                    {
                        if ((totalCopper / 1000000) + totalPlatinum > 0)
                        {
                            totalPlatinum += (totalCopper / 1000000);
                            totalCopper = totalCopper % 1000000;
                        }else
                        {
                            if (totalPlatinum < Int64.MaxValue)
                            {
                                long rem = Int64.MaxValue - totalPlatinum;
                                totalCopper -= rem * 1000000;
                                totalPlatinum = Int64.MaxValue;
                            }
                        }
                    }
                }
            }
            //silver compression
            if (totalSilver >= 100)
            {
                if ((totalSilver / 100) + totalGold > 0)
                {
                    totalGold += (totalSilver / 100);
                    totalSilver = totalSilver % 100;
                }
                else
                {
                    if (totalGold < Int64.MaxValue)
                    {
                        long rem = Int64.MaxValue - totalGold;
                        totalSilver -= rem * 100;
                        totalGold = Int64.MaxValue;
                    }
                }
                if (totalSilver >= 10000)
                {
                    if ((totalSilver / 10000) + totalGold > 0)
                    {
                        totalPlatinum += (totalSilver / 10000);
                        totalSilver = totalSilver % 10000;
                    }
                    else
                    {
                        if (totalPlatinum < Int64.MaxValue)
                        {
                            long rem = Int64.MaxValue - totalPlatinum;
                            totalSilver -= rem * 10000;
                            totalPlatinum = Int64.MaxValue;
                        }
                    }
                }
            }

            //gold compression
            if (totalGold >= 100)
            {
                if ((totalGold / 100) + totalPlatinum > 0)
                {
                    totalPlatinum += (totalGold / 100);
                    totalGold = totalGold % 100;
                }
                else
                {
                    if (totalPlatinum < Int64.MaxValue)
                    {
                        long rem = Int64.MaxValue - totalPlatinum;
                        totalPlatinum -= rem * 100;
                        totalPlatinum = Int64.MaxValue;
                    }
                }
            }
            if(items.ContainsKey("" + ItemID.CopperCoin))
            {
                items.Remove("" + ItemID.CopperCoin);
            }
            if(totalCopper > 0)
            {
                items["" + ItemID.CopperCoin] = totalCopper;
            }
            if (items.ContainsKey("" + ItemID.SilverCoin))
            {
                items.Remove("" + ItemID.SilverCoin);
            }
            if (totalSilver > 0)
            {
                items["" + ItemID.SilverCoin] = totalSilver;
            }

            if (items.ContainsKey("" + ItemID.GoldCoin))
            {
                items.Remove("" + ItemID.GoldCoin);
            }
            if (totalGold > 0)
            {
                items["" + ItemID.GoldCoin] = totalGold;
            }

            if (items.ContainsKey("" + ItemID.PlatinumCoin))
            {
                items.Remove("" + ItemID.PlatinumCoin);
            }
            if (totalPlatinum > 0)
            {
                items["" + ItemID.PlatinumCoin] = totalPlatinum;
            }
        }
    }
}