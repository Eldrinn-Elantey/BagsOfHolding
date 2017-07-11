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
	public class FishBag : GenericHoldingBag
	{
		
		public static List<string> contents;
		
		static FishBag(){
			contents = new List<string>();
            contents.Add("" + ItemID.FlarefinKoi);
            contents.Add("" + ItemID.Obsidifish);
            contents.Add("" + ItemID.Honeyfin);
            contents.Add(""+ItemID.ArmoredCavefish);
            contents.Add("" + ItemID.SpecularFish);
            contents.Add("" + ItemID.Damselfish);
            contents.Add("" + ItemID.Stinkfish);
            contents.Add("" + ItemID.Ebonkoi);
            contents.Add("" + ItemID.CrimsonTigerfish);
            contents.Add("" + ItemID.Hemopiranha);
            contents.Add("" + ItemID.ChaosFish);
            contents.Add("" + ItemID.PrincessFish);
            contents.Add("" + ItemID.Prismite);
            contents.Add("" + ItemID.DoubleCod);
            contents.Add("" + ItemID.VariegatedLardfish);
            contents.Add(""+ItemID.FrostMinnow);
            contents.Add("" + ItemID.GoldenCarp);
            contents.Add(""+ItemID.Bass);
            contents.Add("" + ItemID.AtlanticCod);
            contents.Add("" + ItemID.Salmon);
            contents.Add("" + ItemID.Trout);
            contents.Add("" + ItemID.Tuna);
            contents.Add("" + ItemID.RedSnapper);
            contents.Add("" + ItemID.Shrimp);
            contents.Add("" + ItemID.NeonTetra);

            contents.Add("" + ItemID.OldShoe);
            contents.Add("" + ItemID.TinCan);
            contents.Add("" + ItemID.FishingSeaweed);
            contents.Add("" + ItemID.Seaweed);
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Fish Bag");
            Tooltip.SetDefault("Automatically stores picked up fish (and junk) in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Poissons");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les poissons (et les déchets) si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Peixes");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda os peixes (e lixo) no saco automáticamente se equipado como um acessório.");
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