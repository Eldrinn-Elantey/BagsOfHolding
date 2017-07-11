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
			contents.AddRange(originalContents);
			contents.AddRange(WoodBag.contents);
			contents.AddRange(PlantBag.contents);
            contents.AddRange(MushroomBag.contents);
            contents.AddRange(DyeMaterialBag.contents);
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
			/*items = new TagCompound();
			GemBag gb = new GemBag();
			gb.setupItemList();
			foreach(KeyValuePair<string, object> itm in gb.items){
				if(!items.HasTag(itm.Key))
					items.Add(itm);
			}
			OreBag ob = new OreBag();
			ob.setupItemList();
			foreach(KeyValuePair<string, object> itm in ob.items){
				if(!items.HasTag(itm.Key))
					items.Add(itm);
			}
			DirtBag db = new DirtBag();
			db.setupItemList();
			foreach(KeyValuePair<string, object> itm in db.items){
				if(!items.HasTag(itm.Key))
					items.Add(itm);
			}*/
			
			order = contents;
			base.setupItemList();
		}
		
		public override void AddRecipes(){
			//this.SetDefaults();
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