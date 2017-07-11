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
	public class OmniBag : GenericHoldingBag
	{
	
		public static List<string> contents = new List<string>();
		public static List<string> originalContents = new List<string>();
		static OmniBag(){
			resetContents();
		}
		public static void resetContents(){
			contents.Clear();
			contents.AddRange(originalContents);
			contents.AddRange(OreBag.contents);
			contents.AddRange(GemBag.contents);
			contents.AddRange(DirtBag.contents);
		}

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Omni Bag");
            Tooltip.SetDefault("Automatically stores picked up gems, ores, rocks and soils in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac Omni");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les gemmes, les minerais, la pierre et la terre si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Omni-saco");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda gemas, solos e minérios no saco automáticamente se equipado como um acessório.");
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
			recipe.AddIngredient(null, "OreBag",1);
			recipe.AddIngredient(null, "GemBag",1);
			recipe.AddIngredient(null, "DirtBag",1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.SetResult(this, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddRecipe();
		}
	}
}