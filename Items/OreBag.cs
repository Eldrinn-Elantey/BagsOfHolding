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
	public class OreBag : GenericHoldingBag
	{

        public static List<string> contents;
        public static List<string> noPickup;

        public override void createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
			order.Add(""+ItemID.CopperOre);
			order.Add(""+ItemID.TinOre);
			order.Add(""+ItemID.IronOre);
			order.Add(""+ItemID.LeadOre);
			order.Add(""+ItemID.SilverOre);
			order.Add(""+ItemID.TungstenOre);
			order.Add(""+ItemID.GoldOre);
			order.Add(""+ItemID.PlatinumOre);
			order.Add(""+ItemID.DemoniteOre);
			order.Add(""+ItemID.ShadowScale);
			order.Add(""+ItemID.CrimtaneOre);
			order.Add(""+ItemID.TissueSample);
			order.Add(""+ItemID.Meteorite);
			order.Add(""+ItemID.Hellstone);
			order.Add(""+ItemID.Obsidian);
			order.Add(""+ItemID.CobaltOre);
			order.Add(""+ItemID.PalladiumOre);
			order.Add(""+ItemID.MythrilOre);
			order.Add(""+ItemID.OrichalcumOre);
			order.Add(""+ItemID.AdamantiteOre);
			order.Add(""+ItemID.TitaniumOre);
			order.Add(""+ItemID.ChlorophyteOre);
			order.Add(""+ItemID.LunarOre);
			
			/*thorium support*/
			order.Add("ThoriumMod:ThoriumOre");
			order.Add("ThoriumMod:Medicite");
			order.Add("ThoriumMod:SmoothCoal");
			order.Add("ThoriumMod:MagmaOre");
			order.Add("ThoriumMod:Aquaite");
			order.Add("ThoriumMod:LodeStoneChunk");
			order.Add("ThoriumMod:ValadiumChunk");
			order.Add("ThoriumMod:IllumiteChunk");

            /*calamity support*/
            order.Add("CalamityMod:WulfrumShard");
            order.Add("CalamityMod:AerialiteOre");
			order.Add("CalamityMod:CryonicOre");
            order.Add("CalamityMod:CharredOre");
            order.Add("CalamityMod:PerennialOre");
            order.Add("CalamityMod:AstralOre");
            order.Add("CalamityMod:ChaoticOre");
			order.Add("CalamityMod:UelibloomOre");
            order.Add("CalamityMod:MeldBlob");

            /*crystilium mod support*/
            order.Add("CrystiliumMod:RadiantOre");
			
			/*cosmetic variety support*/
			order.Add("CosmeticVariety:Mantilum");
			order.Add("CosmeticVariety:Pallasite");
			order.Add("CosmeticVariety:StarlightOre");
			order.Add("CosmeticVariety:Veridanite");
            order.Add("CosmeticVariety:ZilliumOre");
			
			/*grox mod support*/
			order.Add("GRealm:GroviteOre");
			
			/*spirit mod support*/
			order.Add("SpiritMod:FloranOre");
			order.Add("SpiritMod:MagiciteOreItem");
			order.Add("SpiritMod:SpiritOre");
            order.Add("SpiritMod:ThermiteOre");

            /*zm mod support*/
            order.Add("ZM:ManastoneOre");
			order.Add("ZM:ZynidiumOreItem");

            /*Tremor support*/
            order.Add("Tremor:Rupicide");
            order.Add("Tremor:ArgiteOre");
            order.Add("Tremor:FrostoneOre");
            order.Add("Tremor:NightmareOre");
            order.Add("Tremor:CometiteOre");
            order.Add("Tremor:AngeliteOre");
            order.Add("Tremor:BrassNugget");
            order.Add("Tremor:HardCometiteOre");

            order.Add("AluminumMod:BauxiteOre");

            order.Add("Azurium:Azurite");

            order.Add("Bismuth:BismuthumOre");
            order.Add("Bismuth:OrcishFragment");

            order.Add("Bluemagic:PuriumOre");

            order.Add("Carnation:DemonbloodOreItem");
            order.Add("Carnation:GalaxiaOreItem");

            order.Add("CheezeMod:StarlightOre");

            order.Add("CivitasMod:AgateOre");
            order.Add("CivitasMod:BerilliumOre");
            order.Add("CivitasMod:TourmalineOre");

            /*Disarray support*/
            order.Add("Disarray:Vitalium");
            order.Add("Disarray:AquauryOre");
            order.Add("Disarray:AtlastriumOre");
            order.Add("Disarray:KhaotiumOre");
            order.Add("Disarray:LegendOre");
            order.Add("Disarray:InauguriteOre");
            order.Add("Disarray:TechnodriumOre");
            order.Add("Disarray:Uranium");
            order.Add("Disarray:DeepIronOre");

            order.Add("EA:Item_2854");
            order.Add("EA:Item_2877");
            order.Add("EA:Item_2884");
            order.Add("EA:Item_2907");
            order.Add("EA:Item_2923");
            order.Add("EA:Item_2956");
            order.Add("EA:Item_3089");
            order.Add("EA:Item_3139");
            order.Add("EA:Item_3151");
            order.Add("EA:Item_3224");
            order.Add("EA:Item_3225");
            order.Add("EA:Item_3298");
            order.Add("EA:Item_3302");
            order.Add("EA:Item_3316");
            order.Add("EA:Item_3330");
            order.Add("EA:Item_3348");
            order.Add("EA:Item_3372");
            order.Add("EA:Item_3451");

            order.Add("EchoesoftheAncient:ArcaniumOre");
            order.Add("EchoesoftheAncient:Ashen_Ore");
            order.Add("EchoesoftheAncients:Skystone_Ore");

            order.Add("Erilipah:CompactHydrogen");
            order.Add("Erilipah:SeriumOre");
            order.Add("Erilipah:SeriumOre");
            order.Add("Erilipah:ShadaineOre");

            /*Exodus support*/
            order.Add("Exodus:ArtaniumOreItem");
            order.Add("Exodus:PhlaziteOreItem");
            order.Add("Exodus:CoalOre");
            order.Add("Exodus:DeepironOre");
            order.Add("Exodus:Jade");
            order.Add("Exodus:NickelOre");
            order.Add("Exodus:OsmiumOre");
            order.Add("Exodus:TriliteOre");

            order.Add("EXOsphere:ultraore");

            order.Add("ForgottenMemories:CosmodiumOre");
            order.Add("ForgottenMemories:SpaceRockFragment");
            order.Add("ForgottenMemories:BlightOreItem");
            order.Add("ForgottenMemories:CryotineOreItem");
            order.Add("ForgottenMemories:GelatineOreItem");

            order.Add("Fernium:FerniumOre");

            order.Add("Godfall:GodfallOre");

            order.Add("Havoc:PrismalOre");

            order.Add("Laugicality:ObsidiumOre");

            order.Add("SacredTools:RawLapis");
            order.Add("SacredTools:FlariumItem");
            order.Add("SacredTools:CerniumItem");
            order.Add("SacredTools:OblivionOre");

            order.Add("Nightmares:NitmarOreItem");

            order.Add("OldGods:BlutoniumOre");

            order.Add("OldSchoolRuneScape:Runiteore");

            order.Add("pinkymod:IceOre");
            order.Add("pinkymod:PandaenOreItem");
            order.Add("pinkymod:VoidTitanite");

            order.Add("QwertysRandomContent:RhuthiniumOre");

            order.Add("Split:TulaniteOre");

            order.Add("ToastersMod:Protagium");

            order.Add("TerrarianExlorationPlus:AerialiphyteOre");
            order.Add("TerrarianExlorationPlus:AluminumOre");
            order.Add("TerrarianExlorationPlus:AncientOre");
            order.Add("TerrarianExlorationPlus:BrikiteOre");
            order.Add("TerrarianExlorationPlus:BrimstoneOre");
            order.Add("TerrarianExlorationPlus:CoalOre");
            order.Add("TerrarianExlorationPlus:CyaniteOre");
            order.Add("TerrarianExlorationPlus:ElectrumOre");
            order.Add("TerrarianExlorationPlus:EnergiteOre");
            order.Add("TerrarianExlorationPlus:Error404Ore");
            order.Add("TerrarianExlorationPlus:ExoticinihteOre");
            order.Add("TerrarianExlorationPlus:ExplosiphyteOre");
            order.Add("TerrarianExlorationPlus:FolsgoldOre");
            order.Add("TerrarianExlorationPlus:GhostironOre");
            order.Add("TerrarianExlorationPlus:Haranite");
            order.Add("TerrarianExlorationPlus:HartiphyteOre");
            order.Add("TerrarianExlorationPlus:HolystoneOre");
            order.Add("TerrarianExlorationPlus:LavasteelOre");
            order.Add("TerrarianExlorationPlus:ManasteelOre");
            order.Add("TerrarianExlorationPlus:MechaniteOre");
            order.Add("TerrarianExlorationPlus:MesmaditeOre");
            order.Add("TerrarianExlorationPlus:MetineOre");
            order.Add("TerrarianExlorationPlus:MorrownihteOre");
            order.Add("TerrarianExlorationPlus:MossiphyteOre");
            order.Add("TerrarianExlorationPlus:OceaniteOre");
            order.Add("TerrarianExlorationPlus:PinkiniteOre");
            order.Add("TerrarianExlorationPlus:PoniteOre");
            order.Add("TerrarianExlorationPlus:RadixiteOre");
            order.Add("TerrarianExlorationPlus:RinaeriteOre");
            order.Add("TerrarianExlorationPlus:RiversteelOre");
            order.Add("TerrarianExlorationPlus:SoulshardOre");
            order.Add("TerrarianExlorationPlus:SteelOre");
            order.Add("TerrarianExlorationPlus:TechnophyteOre");
            order.Add("TerrarianExlorationPlus:TwilightOre");
            order.Add("TerrarianExlorationPlus:TypeNullOre");
            order.Add("TerrarianExlorationPlus:UraniumOre");
            order.Add("TerrarianExlorationPlus:VioniteOre");
            order.Add("TerrarianExlorationPlus:ZirconiumOre");

            order.Add("titedogOre:TutorialOre");

            order.Add("Ultraconyx:NebulaOre");
            order.Add("Ultraconyx:CosmicRock");

            order.Add("Uraniummod:UraniumOre");

            order.Add("WIKModRedux:CeraminteOre");

            order.Add("Xrandia:Xenerite");
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

        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 2;
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