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
	public class PlantBag : GenericHoldingBag
	{
		public static List<string> contents;
		
		static PlantBag(){
			contents = new List<string>();
			contents.Add(""+ItemID.Blinkroot);
			contents.Add(""+ItemID.Daybloom);
            contents.Add("" + ItemID.Moonglow);
            contents.Add("" + ItemID.Waterleaf);
            contents.Add("" + ItemID.Fireblossom);
            contents.Add("" + ItemID.Deathweed);
            contents.Add("" + ItemID.Shiverthorn);
            contents.Add("" + ItemID.BlinkrootSeeds);
            contents.Add("" + ItemID.DaybloomSeeds);
            contents.Add("" + ItemID.MoonglowSeeds);
            contents.Add("" + ItemID.WaterleafSeeds);
            contents.Add("" + ItemID.FireblossomSeeds);
            contents.Add("" + ItemID.DeathweedSeeds);
            contents.Add("" + ItemID.ShiverthornSeeds);
            contents.Add("" + ItemID.Pumpkin);
            contents.Add("" + ItemID.PumpkinSeed);
            contents.Add("" + ItemID.Sunflower);
            contents.Add("" + ItemID.VineRope);
            contents.Add("" + ItemID.VineRopeCoil);

            /*Thorium Support*/
            contents.Add("ThoriumMod:MarineKelp");
            contents.Add("ThoriumMod:ManaBerry");
            contents.Add("ThoriumMod:MarineKelpSeeds");
            contents.Add("ThoriumMod:ManaBerrySeeds");
            
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
		
		public override void setupItemList(){
			order = contents;
			base.setupItemList();
		}
		
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 15);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.SetResult(this, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}
	}
}