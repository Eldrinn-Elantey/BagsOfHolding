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
using Terraria.IO;

namespace JPANsBagsOfHoldingMod.Items
{
	public class TrueOmniBag : CoinBag //GenericHoldingBag
	{

        public static List<string> contents2 = new List<string>();
        public static List<string> blacklist = null;

        public static bool pickupCoins = true;

        public static bool isInit = false;

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

        public override void setupItemList()
        {
            bagID = 15;
            basestSeupItemList();
            order = contents2;
        }

        public override void loadConfig()
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
                    config.Put("coinPickup", pickupCoins);
                    config.Put("blacklist", new List<string>());
                    config.Put("noPickup", new List<string>());
                    config.Save();
                    return;
                }
            }
        }

        public override void loadBagInfoFromConfig()
        {
            loadConfig();
            loadLeftClickFromConfig();
            pickupCoins = config.Get<bool>("coinPickup", pickupCoins);
            if(blacklist == null)
            {
                blacklist = BagsOfHoldingMod.getStringListFromConfig(config, "blacklist");
            }
            preventPickup = BagsOfHoldingMod.getStringListFromConfig(config, "noPickup");
            if (!pickupCoins)
            {
                preventPickup.Add("" + ItemID.CopperCoin);
                preventPickup.Add("" + ItemID.SilverCoin);
                preventPickup.Add("" + ItemID.GoldCoin);
                preventPickup.Add("" + ItemID.PlatinumCoin);
            }
        }

        public override bool createDefaultItemList()
        {
            blacklist = new List<string>();
            preventPickup = new List<string>();
            return false;
        }

        public override void AddRecipes(){
            if (!disableBag)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "OmniBag", 1);
                recipe.AddIngredient(null, "NatureBag", 1);
                recipe.AddIngredient(null, "FishingBag", 1);
                recipe.AddIngredient(ItemID.BrokenHeroSword, 2);
                recipe.SetResult(this, 1);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.AddRecipe();
            }
		}
    }

}
