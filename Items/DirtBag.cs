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
	public class DirtBag : GenericHoldingBag
	{
		
		public static List<string> contents;
		
		static DirtBag(){
			contents = new List<string>();
			contents.Add(""+ItemID.DirtBlock);
			contents.Add(""+ItemID.ClayBlock);
			contents.Add(""+ItemID.SiltBlock);
			contents.Add(""+ItemID.MudBlock);
			contents.Add(""+ItemID.StoneBlock);
			contents.Add(""+ItemID.EbonstoneBlock);
			contents.Add(""+ItemID.CrimstoneBlock);
			contents.Add(""+ItemID.PearlstoneBlock);
			contents.Add(""+ItemID.SandBlock);
			contents.Add(""+ItemID.EbonsandBlock);
			contents.Add(""+ItemID.CrimsandBlock);
			contents.Add(""+ItemID.PearlsandBlock);
			contents.Add(""+ItemID.HardenedSand);
			contents.Add(""+ItemID.CorruptHardenedSand);
			contents.Add(""+ItemID.CrimsonHardenedSand);
			contents.Add(""+ItemID.HallowHardenedSand);
			contents.Add(""+ItemID.Sandstone);
			contents.Add(""+ItemID.CorruptSandstone);
			contents.Add(""+ItemID.CrimsonSandstone);
			contents.Add(""+ItemID.HallowSandstone);
			contents.Add(""+ItemID.DesertFossil);
			contents.Add(""+ItemID.SnowBlock);
			contents.Add(""+ItemID.SlushBlock);
			contents.Add(""+ItemID.IceBlock);
			contents.Add(""+ItemID.PurpleIceBlock);
			contents.Add(""+ItemID.RedIceBlock);
			contents.Add(""+ItemID.PinkIceBlock);
			contents.Add(""+ItemID.Granite);
			contents.Add(""+ItemID.GraniteBlock);
			contents.Add(""+ItemID.Marble);
			contents.Add(""+ItemID.MarbleBlock);
			contents.Add(""+ItemID.AshBlock);
			
			/*thorium support*/
			contents.Add("ThoriumMod:MarineRock");
			contents.Add("ThoriumMod:MarineRockMoss");
			contents.Add("ThoriumMod:BrackMud");
			
			/*crystilium mod support*/
			contents.Add("CrystiliumMod:CrystalBlock");
			
			/*spirit mod support*/
			contents.Add("SpiritMod:SpiritDirtItem");
			contents.Add("SpiritMod:SpiritStoneItem");
			contents.Add("SpiritMod:SpiritSandItem");
			contents.Add("SpiritMod:SpiritIceItem");
			
		}

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Soil Bag");
            Tooltip.SetDefault("Automatically stores picked up rocks and soils in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Terre");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement la terre et la pierre si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Solos");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda pedras, terras e areias dentro do saco automáticamente se equipado como um acessório.");
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