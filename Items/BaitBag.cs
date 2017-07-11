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
	public class BaitBag : GenericHoldingBag
	{
		
		public static List<string> contents;
		
		static BaitBag(){
			contents = new List<string>();
			contents.Add(""+ItemID.TruffleWorm);
			contents.Add(""+ItemID.MasterBait);
			contents.Add(""+ItemID.GoldButterfly);
			contents.Add(""+ItemID.GoldGrasshopper);
            contents.Add("" + ItemID.GoldWorm);
            contents.Add(""+ItemID.TreeNymphButterfly);
			contents.Add(""+ItemID.Buggy);
			contents.Add(""+ItemID.EnchantedNightcrawler);
			contents.Add(""+ItemID.LightningBug);
			contents.Add(""+ItemID.PurpleEmperorButterfly);
			contents.Add(""+ItemID.JourneymanBait);
			contents.Add(""+ItemID.RedAdmiralButterfly);
			contents.Add(""+ItemID.JuliaButterfly);
			contents.Add(""+ItemID.Worm);
			contents.Add(""+ItemID.Sluggy);
			contents.Add(""+ItemID.UlyssesButterfly);
			contents.Add(""+ItemID.Firefly);
			contents.Add(""+ItemID.BlueJellyfish);
            contents.Add("" + ItemID.GreenJellyfish);
            contents.Add("" + ItemID.PinkJellyfish);
            contents.Add(""+ItemID.ApprenticeBait);
			contents.Add(""+ItemID.ZebraSwallowtailButterfly);
			contents.Add(""+ItemID.BlackScorpion);
			contents.Add(""+ItemID.GlowingSnail);
			contents.Add(""+ItemID.Grubby);
			contents.Add(""+ItemID.SulphurButterfly);
			contents.Add(""+ItemID.Grasshopper);
            contents.Add("" + ItemID.Scorpion);
            contents.Add(""+ItemID.Snail);
			contents.Add(""+ItemID.MonarchButterfly);
			
			/*thorium support*/
			contents.Add("ThoriumMod:PulseLure");
			
			/*Fishing3 mod support*/
			contents.Add("Fishing3:SoulBait");
			
		}

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Bait Bag");
            Tooltip.SetDefault("Automatically stores picked up bait in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac à Appâts");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les appâts dans le sac si il est équippé dans les accessoires");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Iscas");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda iscas apanhadas no saco automáticamente se equipado como um acessório.");
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