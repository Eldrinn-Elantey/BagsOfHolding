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
	public class PlantBag : GenericHoldingBag
	{
        public static List<string> contents;
        public static List<string> noPickup;

        public override bool createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
            order.Add(""+ItemID.Blinkroot);
			order.Add(""+ItemID.Daybloom);
            order.Add("" + ItemID.Moonglow);
            order.Add("" + ItemID.Waterleaf);
            order.Add("" + ItemID.Fireblossom);
            order.Add("" + ItemID.Deathweed);
            order.Add("" + ItemID.Shiverthorn);
            order.Add("" + ItemID.BlinkrootSeeds);
            order.Add("" + ItemID.DaybloomSeeds);
            order.Add("" + ItemID.MoonglowSeeds);
            order.Add("" + ItemID.WaterleafSeeds);
            order.Add("" + ItemID.FireblossomSeeds);
            order.Add("" + ItemID.DeathweedSeeds);
            order.Add("" + ItemID.ShiverthornSeeds);
            order.Add("" + ItemID.Pumpkin);
            order.Add("" + ItemID.PumpkinSeed);
            order.Add("" + ItemID.Sunflower);
            order.Add("" + ItemID.VineRope);
            order.Add("" + ItemID.VineRopeCoil);

            /*Thorium Support*/
            order.Add("ThoriumMod:MarineKelp");
            order.Add("ThoriumMod:ManaBerry");
            order.Add("ThoriumMod:MarineKelpSeeds");
            order.Add("ThoriumMod:ManaBerrySeeds");

            order.Add("Antiaris:Apple");

            order.Add("CosmeticVariety:Apple");
            order.Add("CosmeticVariety:AppleSeed");
            order.Add("CosmeticVariety:Banana");
            order.Add("CosmeticVariety:BananaSeed");
            order.Add("CosmeticVariety:Cherry");
            order.Add("CosmeticVariety:CherrySeed");
            order.Add("CosmeticVariety:Lemon");
            order.Add("CosmeticVariety:LemonSeed");
            order.Add("CosmeticVariety:Orange");
            order.Add("CosmeticVariety:OrangeSeed");
            order.Add("CosmeticVariety:Strawberry");
            order.Add("CosmeticVariety:StrawberrySeed");
            order.Add("CosmeticVariety:Watermelon");
            order.Add("CosmeticVariety:WatermelonSeed");
            order.Add("CosmeticVariety:HotPepper");
            order.Add("CosmeticVariety:PepperSeeds");
            order.Add("CosmeticVariety:AshPotato");
            order.Add("CosmeticVariety:PotatoSeeds");
            order.Add("CosmeticVariety:Wheat");
            order.Add("CosmeticVariety:WheatSeed");

            order.Add("CosmeticVariety:Rose");
            order.Add("CosmeticVariety:TigerLily");
            order.Add("CosmeticVariety:Tulip");
            order.Add("CosmeticVariety:Starseed");

            for (int i = 1; i < 66; i++)
            {
                order.Add("OreSeeds:OreSeeds" + i);
            }

            order.Add("Wildlife:Bamboo");
            order.Add("Wildlife:BambooStalk");
            order.Add("Wildlife:CatTail");
            order.Add("Wildlife:FluxCoral");
            order.Add("Wildlife:ForestBerry");
            order.Add("Wildlife:FrostShine");
            order.Add("Wildlife:GelonSlice");
            order.Add("Wildlife:Jaderose");
            order.Add("Wildlife:MagmaCorn");
            order.Add("Wildlife:MedusaBerry");
            order.Add("Wildlife:Strawberry");

            return true;
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Plant Bag");
            Tooltip.SetDefault("Automatically stores plants and their seeds in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Plantes");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les plantes si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Plantas");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda plantas (e suas sementes) no saco automáticamente se equipado como um acessório.");
        }

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}

        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 9;
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
	}
}