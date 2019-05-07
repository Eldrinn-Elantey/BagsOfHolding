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
using Terraria.IO;

namespace JPANsBagsOfHoldingMod.Items
{
	public class DirtBag : GenericHoldingBag
	{

        public static List<string> contents;
        public static List<string> noPickup;

        public override bool createDefaultItemList()
        {
           
            preventPickup = new List<string>();
            order = new List<string>();
			order.Add(""+ItemID.DirtBlock);
			order.Add(""+ItemID.ClayBlock);
			order.Add(""+ItemID.SiltBlock);
			order.Add(""+ItemID.MudBlock);
			order.Add(""+ItemID.StoneBlock);
			order.Add(""+ItemID.EbonstoneBlock);
			order.Add(""+ItemID.CrimstoneBlock);
			order.Add(""+ItemID.PearlstoneBlock);
			order.Add(""+ItemID.SandBlock);
			order.Add(""+ItemID.EbonsandBlock);
			order.Add(""+ItemID.CrimsandBlock);
			order.Add(""+ItemID.PearlsandBlock);
			order.Add(""+ItemID.HardenedSand);
			order.Add(""+ItemID.CorruptHardenedSand);
			order.Add(""+ItemID.CrimsonHardenedSand);
			order.Add(""+ItemID.HallowHardenedSand);
			order.Add(""+ItemID.Sandstone);
			order.Add(""+ItemID.CorruptSandstone);
			order.Add(""+ItemID.CrimsonSandstone);
			order.Add(""+ItemID.HallowSandstone);
			order.Add(""+ItemID.DesertFossil);
			order.Add(""+ItemID.SnowBlock);
			order.Add(""+ItemID.SlushBlock);
			order.Add(""+ItemID.IceBlock);
			order.Add(""+ItemID.PurpleIceBlock);
			order.Add(""+ItemID.RedIceBlock);
			order.Add(""+ItemID.PinkIceBlock);
			order.Add(""+ItemID.Granite);
			order.Add(""+ItemID.GraniteBlock);
			order.Add(""+ItemID.Marble);
			order.Add(""+ItemID.MarbleBlock);
			order.Add(""+ItemID.AshBlock);
			
			/*thorium support*/
			order.Add("ThoriumMod:MarineRock");
			order.Add("ThoriumMod:MarineRockMoss");
			order.Add("ThoriumMod:BrackMud");

			/*crystilium mod support*/
			order.Add("CrystiliumMod:CrystalBlock");

            /*spirit mod support*/
            order.Add("SpiritMod:ReachGrass");
            order.Add("SpiritMod:HalloweenGrass");
            order.Add("SpiritMod:SpiritDirtItem");
			order.Add("SpiritMod:SpiritStoneItem");
			order.Add("SpiritMod:SpiritSandItem");
			order.Add("SpiritMod:SpiritIceItem");

            /*GRealm support*/
            order.Add("GRealm:EverfrostBlock");
            order.Add("GRealm:EversnowBlock");

            order.Add("Tremor:IceBlockB");

            order.Add("CosmeticVariety:Starstone");
            order.Add("CosmeticVariety:Starsoil");
            order.Add("CosmeticVariety:Starsand");

            order.Add("EA:Item_3101");
            order.Add("EA:Item_3131");
            order.Add("EA:Item_3163");
            order.Add("EA:Item_3367");
            order.Add("EA:Item_3390");
            order.Add("EA:Item_3606");
            order.Add("EA:Item_3607");
            order.Add("EA:Item_3608");
            order.Add("EA:Item_3609");

            order.Add("Erilipah:ShadaineStone");

            order.Add("Exodus:LimestoneItem");

            order.Add("EXOsphere:ultrastone");

            order.Add("SacredTools:FlameRack");
            order.Add("SacredTools:FlamingRack");
            order.Add("SacredTools:FrostRack");
            order.Add("SacredTools:ScorchedSand");

            order.Add("TerrarianExlorationPlus:RadiatedEarth");
            order.Add("TerrarianExlorationPlus:Andesite");
            order.Add("TerrarianExlorationPlus:Basalt");
            order.Add("TerrarianExlorationPlus:Dacite");
            order.Add("TerrarianExlorationPlus:Diorite");
            order.Add("TerrarianExlorationPlus:MossyStone");

            order.Add("titedogOre:TutorialBiomeBlock");

            return true;
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

        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 1;
            if (bagConfig == null)
            {
                remakeFromConfig();
            }
            else
            {
                if (items == null)
                    items = new TagCompound();
                config = bagConfig;
                order = contents;
                preventPickup = noPickup;
                loadLeftClickFromConfig();
            }
        }

        public override void remakeFromConfig()
        {
            base.setupItemList();
            if (contents == null)
            {
                contents = new List<string>();
            }
            else
            {
                contents.Clear();
            }
            contents.AddRange(order);
            if (noPickup == null)
            {
                noPickup = new List<string>();
            }
            else
            {
                noPickup.Clear();
            }
            noPickup.AddRange(preventPickup);
            if (bagConfig == null)
            {
                bagConfig = config;
            }
            else
            {
                foreach (string k in config.GetAllKeys())
                {
                    bagConfig.Put(k, config.Get<object>(k, null));
                }
                bagConfig.Save();
            }
        }

        public override void AddRecipes(){
            if (!disableBag)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.Silk, 15);
                recipe.AddIngredient(ItemID.FallenStar, 1);
                recipe.SetResult(this, 1);
                recipe.AddTile(TileID.WorkBenches);
                recipe.AddRecipe();
            }
		}
	}
}