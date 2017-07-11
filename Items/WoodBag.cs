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
	public class WoodBag : GenericHoldingBag
	{
		public static List<string> contents;
		
		static WoodBag(){
			contents = new List<string>();
			contents.Add(""+ItemID.Wood);
			contents.Add(""+ItemID.BorealWood);
			contents.Add(""+ItemID.RichMahogany);
			contents.Add(""+ItemID.Ebonwood);
			contents.Add(""+ItemID.Shadewood);
			contents.Add(""+ItemID.Pearlwood);
            contents.Add("" + ItemID.PalmWood);
            contents.Add("" + ItemID.SpookyWood);
            contents.Add("" + ItemID.Acorn);
            contents.Add("" + ItemID.Cactus);

			/*crystilium mod support*/
			contents.Add("CrystiliumMod:CrystalWood");

            /*spirit mod Suppore*/
            contents.Add("SpiritMod:SpiritWoodItem");

        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Wood Bag");
            Tooltip.SetDefault("Automatically stores picked up wood and acorns in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Bois");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement le bois et les glands si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Madeira");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda madeiras no saco automáticamente se equipado como um acessório.");
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