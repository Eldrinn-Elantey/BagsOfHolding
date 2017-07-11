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
using Terraria.Localization;

namespace JPANsBagsOfHoldingMod.Items
{
	public class TrueOmniBag : GenericHoldingBag
	{

        public static List<string> contents = new List<string>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("True Omni Bag");
            Tooltip.SetDefault("Ultimate storage Bag. Automatically stores any stackeable item in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Le Vrai Sac Omni");
            Tooltip.AddTranslation(GameCulture.French, "Sac de Stockage ultime, récupère tout les objets empilable si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "O Verdadeiro Omni-saco");
            Tooltip.AddTranslation(GameCulture.Portuguese, "O derradeiro saco. Guarda automáticamente qualquer objecto que possa ser empilhado se euqipado como um acessório.");
        }

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}
		
		public override void setupItemList(){
            order = contents;
			base.setupItemList();
		}

        public override long addItem(Item itm)
        {
            long ans = base.addItem(itm);
            rebalanceCoins();
            return ans;
        }

        public override void AddRecipes(){
			//this.SetDefaults();
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "OmniBag",1);
			recipe.AddIngredient(null, "NatureBag",1);
			recipe.AddIngredient(null, "FishingBag",1);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 2);
			recipe.SetResult(this, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddRecipe();
		}

        private void rebalanceCoins()
        {
            long totalCopper = items.ContainsKey("" + ItemID.CopperCoin) ? items.GetAsLong("" + ItemID.CopperCoin) : 0;
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
                }
                else
                {
                    if (totalSilver < Int64.MaxValue)
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
                    }
                    else
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
                        }
                        else
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
            if (items.ContainsKey("" + ItemID.CopperCoin))
            {
                items.Remove("" + ItemID.CopperCoin);
            }
            if (totalCopper > 0)
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
