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
	public class OreBag : GenericHoldingBag
	{
	
		public static List<string> contents;
		
		static OreBag(){
			contents = new List<string>();
			contents.Add(""+ItemID.CopperOre);
			contents.Add(""+ItemID.TinOre);
			contents.Add(""+ItemID.IronOre);
			contents.Add(""+ItemID.LeadOre);
			contents.Add(""+ItemID.SilverOre);
			contents.Add(""+ItemID.TungstenOre);
			contents.Add(""+ItemID.GoldOre);
			contents.Add(""+ItemID.PlatinumOre);
			contents.Add(""+ItemID.DemoniteOre);
			contents.Add(""+ItemID.ShadowScale);
			contents.Add(""+ItemID.CrimtaneOre);
			contents.Add(""+ItemID.TissueSample);
			contents.Add(""+ItemID.Meteorite);
			contents.Add(""+ItemID.Hellstone);
			contents.Add(""+ItemID.Obsidian);
			contents.Add(""+ItemID.CobaltOre);
			contents.Add(""+ItemID.PalladiumOre);
			contents.Add(""+ItemID.MythrilOre);
			contents.Add(""+ItemID.OrichalcumOre);
			contents.Add(""+ItemID.AdamantiteOre);
			contents.Add(""+ItemID.TitaniumOre);
			contents.Add(""+ItemID.ChlorophyteOre);
			contents.Add(""+ItemID.LunarOre);
			
			/*thorium support*/
			contents.Add("ThoriumMod:ThoriumOre");
			contents.Add("ThoriumMod:Medicite");
			contents.Add("ThoriumMod:SmoothCoal");
			contents.Add("ThoriumMod:MagmaOre");
			contents.Add("ThoriumMod:Aquaite");
			contents.Add("ThoriumMod:LodeStoneChunk");
			contents.Add("ThoriumMod:ValadiumChunk");
			contents.Add("ThoriumMod:IllumiteChunk");
			
			/*calamity support*/
			contents.Add("CalamityMod:AerialiteOre");
			contents.Add("CalamityMod:CryonicOre");
			contents.Add("CalamityMod:PerennialOre");
			contents.Add("CalamityMod:ChaoticOre");
			contents.Add("CalamityMod:UelibloomOre");
			
			/*crystilium mod support*/
			contents.Add("CrystiliumMod:RadiantOre");
			
			/*cosmetic variety support*/
			contents.Add("CosmeticVariety:Mantilum");
			contents.Add("CosmeticVariety:Pallasite");
			contents.Add("CosmeticVariety:StarlightOre");
			contents.Add("CosmeticVariety:Veridanite");
			
			/*grox mod support*/
			contents.Add("GRealm:GroviteOre");
			
			/*spirit mod support*/
			contents.Add("SpiritMod:FloranOre");
			contents.Add("SpiritMod:MagiciteOreItem");
			contents.Add("SpiritMod:SpiritOre");
			
			/*zm mod support*/
			contents.Add("ZM:ManastoneOre");
			contents.Add("ZM:ZynidiumOreItem");

            /*Tremor support*/
            contents.Add("Tremor:ArgiteOre");
            contents.Add("Tremor:FrostoneOre");
            contents.Add("Tremor:NightmareOre");
            contents.Add("Tremor:CometiteOre");
            contents.Add("Tremor:AngeliteOre");

        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Ore Bag");
            Tooltip.SetDefault("Automatically stores picked up ores in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac à Minerais");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les minerais si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Minérios");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda minérios no saco automáticamente se equipado como um acessório.");

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