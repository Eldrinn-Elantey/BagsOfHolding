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
	public class MushroomBag : GenericHoldingBag
	{
		public static List<string> contents;
		
		static MushroomBag(){
			contents = new List<string>();
            contents.Add("" + ItemID.Mushroom);
            contents.Add("" + ItemID.GlowingMushroom);
            contents.Add("" + ItemID.VileMushroom);
            contents.Add("" + ItemID.ViciousMushroom);
            contents.Add(""+ItemID.GrassSeeds);
            contents.Add("" + ItemID.JungleGrassSeeds);
            contents.Add(""+ItemID.MushroomGrassSeeds);
            contents.Add("" + ItemID.CorruptSeeds);
            contents.Add("" + ItemID.CrimsonSeeds);
            contents.Add("" + ItemID.HallowedSeeds);
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Mushroom Bag");
            Tooltip.SetDefault("Automatically stores mushrooms and grass seeds in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac à Champignons");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les champignons et les graines si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Cogumelos");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda cogumelos e sementes de grama no saco automáticamente se equipado como um acessório.");
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