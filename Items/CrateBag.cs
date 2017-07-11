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
	public class CrateBag : GenericHoldingBag
	{
		
		public static List<string> contents;
		
		static CrateBag(){
			contents = new List<string>();
			contents.Add(""+ItemID.WoodenCrate);
			contents.Add(""+ItemID.IronCrate);
			contents.Add(""+ItemID.GoldenCrate);
			contents.Add(""+ItemID.CorruptFishingCrate);
            contents.Add("" + ItemID.CrimsonFishingCrate);
            contents.Add(""+ItemID.HallowedFishingCrate);
            contents.Add("" + ItemID.JungleFishingCrate);
            contents.Add("" + ItemID.FloatingIslandFishingCrate);
            contents.Add(""+ItemID.DungeonFishingCrate);
			
			/*thorium support*/
			contents.Add("ThoriumMod:StrangeCrate");
			
			/*Fishing3 mod support*/
			contents.Add("Fishing3:ForestCrate");
            contents.Add("Fishing3:CaveCrate");
            contents.Add("Fishing3:DesertCrate");
            contents.Add("Fishing3:IceCrate");
            contents.Add("Fishing3:OceanCrate");
            contents.Add("Fishing3:ObsidianCrate");
            contents.Add("Fishing3:PCorruptCrate");
            contents.Add("Fishing3:PCrimsonCrate");
            contents.Add("Fishing3:PHallowCrate");
            contents.Add("Fishing3:PossessedCrate");

            /*Unu's Battle Rods Support*/
            contents.Add("UnuBattleRods:GraniteCrate");
            contents.Add("UnuBattleRods:MarbleCrate");
            contents.Add("UnuBattleRods:MimicCrate");
            contents.Add("UnuBattleRods:CorruptCrate");
            contents.Add("UnuBattleRods:CrimsonCrate");
            contents.Add("UnuBattleRods:ObsidianCrate");
            contents.Add("UnuBattleRods:MeteorCrate");
            contents.Add("UnuBattleRods:HallowedCrate");
            contents.Add("UnuBattleRods:SoulCrate");
            contents.Add("UnuBattleRods:BeeCrate");
            contents.Add("UnuBattleRods:ChlorophyteCrate");
            contents.Add("UnuBattleRods:SpookyCrate");
            contents.Add("UnuBattleRods:ShroomiteCrate");
            contents.Add("UnuBattleRods:TerraCrate");
            contents.Add("UnuBattleRods:LuminiteCrate");
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Crate Bag");
            Tooltip.SetDefault("Automatically stores picked up crates in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Caisse");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les caisses si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Caixotes");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda caixotes dentro do saco automáticamente se equipado como acessório.");
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