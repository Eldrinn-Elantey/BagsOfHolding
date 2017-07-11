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
	public class DyeMaterialBag : GenericHoldingBag
	{
		public static List<string> contents;
		
		static DyeMaterialBag(){
			contents = new List<string>();
            contents.Add("" + ItemID.StrangePlant1);
            contents.Add("" + ItemID.StrangePlant2);
            contents.Add("" + ItemID.StrangePlant3);
            contents.Add("" + ItemID.StrangePlant4);
            contents.Add(""+ItemID.BlackInk);
            contents.Add("" + ItemID.OrangeBloodroot);
            contents.Add(""+ItemID.YellowMarigold);
            contents.Add("" + ItemID.LimeKelp);
            contents.Add("" + ItemID.GreenMushroom);
            contents.Add("" + ItemID.TealMushroom);
            contents.Add("" + ItemID.CyanHusk);
            contents.Add("" + ItemID.SkyBlueFlower);
            contents.Add("" + ItemID.BlueBerries);
            contents.Add("" + ItemID.VioletHusk);
            contents.Add("" + ItemID.PinkPricklyPear);
            
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Dye Material Bag");
            Tooltip.SetDefault("Automatically stores natural dye materials and strange plants in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Teintures");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les matériaux de teintures si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Materiais de Tintura");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda materiais para obter tinturas no saco automáticamente se equipado como um acessório.");

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