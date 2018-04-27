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
	public class FishingBag : GenericHoldingBag
	{
	
		public static List<string> contents = new List<string>();
		public static List<string> originalContents = new List<string>();
		static FishingBag(){
			resetContents();
		}
		public static void resetContents(){
            contents.Clear();
            GenericHoldingBag bg = new CrateBag();
            bg.setupItemList();
            contents.AddRange(bg.order);
            bg = new FishBag();
            bg.setupItemList();
            contents.AddRange(bg.order);
            bg = new BaitBag();
            bg.setupItemList();
            contents.AddRange(bg.order);
		}

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Fishing Bag");
            Tooltip.SetDefault("Automatically stores picked up fished-up items and crates in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Pêche");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les objets pêchés si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Pesca");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda muitos dos objectos pescados e iscas no saco automáticamente se equipado como um acessório.");

        }

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}
		
		public override void setupItemList(){
            bagID = 8;
            order = contents;
			base.setupItemList();
            order = contents;
        }
		
		public override void AddRecipes(){
            if (!disableBag)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "FishBag", 1);
                recipe.AddIngredient(null, "BaitBag", 1);
                recipe.AddIngredient(null, "CrateBag", 1);
                recipe.AddIngredient(ItemID.FallenStar, 1);
                recipe.SetResult(this, 1);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.AddRecipe();
            }
		}
	}
}