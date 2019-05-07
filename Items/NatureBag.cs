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
	public class NatureBag : GenericHoldingBag
	{
	
		public static List<string> contents = new List<string>();
		public static List<string> originalContents = new List<string>();
		static NatureBag(){
			resetContents();
		}
		public static void resetContents(){
            contents.Clear();
            GenericHoldingBag bg = new WoodBag();
            bg.setupItemList();
            contents.AddRange(bg.order);
            bg = new PlantBag();
            bg.setupItemList();
            contents.AddRange(bg.order);
            bg = new MushroomBag();
            bg.setupItemList();
            contents.AddRange(bg.order);
            bg = new DyeMaterialBag();
            bg.setupItemList();
            contents.AddRange(bg.order);
		}

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Nature Bag");
            Tooltip.SetDefault("Automatically stores wood,plant, mushroom and dye materials in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de la Nature");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement le bois, les plantes, les champignons et les matériaux de teintures si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco da Natureza");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda a madeira, plantas, sementes, cogumelos e materiais de tintura automáticamente quando apanhados enquanto equipado como um acessório.");
        }

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}
		
		public override void setupItemList(){
            bagID = 13;
            if (order == null || order.Count == 0)
            {
                base.setupItemList();
                order.AddRange(contents);
            }
        }
		
		public override void AddRecipes(){
            if (!disableBag)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "WoodBag", 1);
                recipe.AddIngredient(null, "PlantBag", 1);
                recipe.AddIngredient(null, "MushroomBag", 1);
                recipe.AddIngredient(null, "DyeMaterialBag", 1);
                recipe.AddIngredient(ItemID.FallenStar, 1);
                recipe.SetResult(this, 1);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.AddRecipe();
            }
		}
	}
}