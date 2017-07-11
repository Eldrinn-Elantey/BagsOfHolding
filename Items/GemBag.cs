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
	public class GemBag : GenericHoldingBag
	{
		public static List<string> contents;
		
		static GemBag(){
			contents = new List<string>();
			contents.Add(""+ItemID.Amethyst);
			contents.Add(""+ItemID.Topaz);
			contents.Add(""+ItemID.Emerald);
			contents.Add(""+ItemID.Sapphire);
			contents.Add(""+ItemID.Ruby);
			contents.Add(""+ItemID.Diamond);
            contents.Add("" + ItemID.Amber);
			
			/*thorium support*/
			contents.Add("ThoriumMod:LifeQuartz");
			contents.Add("ThoriumMod:Pearl");
			
			/*crystilium mod support*/
			contents.Add("CrystiliumMod:ShinyGemstone");
			
		}


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Gem Bag");
            Tooltip.SetDefault("Automatically stores picked up gems in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Gemmes");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les gemmes si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Gemas");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda gemas no saco automáticamente se equipado como um acessório.");
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