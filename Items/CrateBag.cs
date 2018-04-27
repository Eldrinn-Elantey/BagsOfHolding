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
	public class CrateBag : GenericHoldingBag
	{

        public static List<string> contents;
        public static List<string> noPickup;

        public override void createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
            order.Add(""+ItemID.WoodenCrate);
			order.Add(""+ItemID.IronCrate);
			order.Add(""+ItemID.GoldenCrate);
			order.Add(""+ItemID.CorruptFishingCrate);
            order.Add("" + ItemID.CrimsonFishingCrate);
            order.Add(""+ItemID.HallowedFishingCrate);
            order.Add("" + ItemID.JungleFishingCrate);
            order.Add("" + ItemID.FloatingIslandFishingCrate);
            order.Add(""+ItemID.DungeonFishingCrate);
			
			/*thorium support*/
			order.Add("ThoriumMod:StrangeCrate");
			
			/*Fishing3 mod support*/
			order.Add("Fishing3:ForestCrate");
            order.Add("Fishing3:CaveCrate");
            order.Add("Fishing3:DesertCrate");
            order.Add("Fishing3:IceCrate");
            order.Add("Fishing3:OceanCrate");
            order.Add("Fishing3:ObsidianCrate");
            order.Add("Fishing3:PCorruptCrate");
            order.Add("Fishing3:PCrimsonCrate");
            order.Add("Fishing3:PHallowCrate");
            order.Add("Fishing3:PJungleCrate");
            order.Add("Fishing3:PossessedCrate");

            /*Unu's Battle Rods Support*/
            order.Add("UnuBattleRods:GraniteCrate");
            order.Add("UnuBattleRods:MarbleCrate");
            order.Add("UnuBattleRods:MimicCrate");
            order.Add("UnuBattleRods:CorruptCrate");
            order.Add("UnuBattleRods:CrimsonCrate");
            order.Add("UnuBattleRods:ObsidianCrate");
            order.Add("UnuBattleRods:MeteorCrate");
            order.Add("UnuBattleRods:HallowedCrate");
            order.Add("UnuBattleRods:SoulCrate");
            order.Add("UnuBattleRods:BeeCrate");
            order.Add("UnuBattleRods:ChlorophyteCrate");
            order.Add("UnuBattleRods:SpookyCrate");
            order.Add("UnuBattleRods:ShroomiteCrate");
            order.Add("UnuBattleRods:TerraCrate");
            order.Add("UnuBattleRods:LuminiteCrate");

            order.Add("ForgottenMemories:ForgottenCrate");

            order.Add("FishingEX:CustomGoldenCrate");
            order.Add("FishingEX:CustomIronCrate");
            order.Add("FishingEX:CustomWoodenCrate");
            order.Add("FishingEX:GenuineGoldenCrate");
            order.Add("FishingEX:GenuineIronCrate");
            order.Add("FishingEX:GenuineWoodenCrate");

            order.Add("FishingPlus:SandstoneCrate");
            order.Add("FishingPlus:SnowyCrate");
            order.Add("FishingPlus:SteampunkCrate");

            order.Add("MoreBiomeFishingCrates:AnglerCrate");
            order.Add("MoreBiomeFishingCrates:DesertCrate");
            order.Add("MoreBiomeFishingCrates:GlowingMushroomCrate");
            order.Add("MoreBiomeFishingCrates:HellCrate");
            order.Add("MoreBiomeFishingCrates:IceCrate");
            order.Add("MoreBiomeFishingCrates:OceanCrate");

            order.Add("QwertysRandomContent:RhuthiniumCrate");

            order.Add("SacredTools:FlariumCrate");

            order.Add("SpiritMod:PirateCrate");
            order.Add("SpiritMod:SpiritCrate");

            order.Add("SpelunkSurge:BoneCrate");
            order.Add("SpelunkSurge:GraniteCrate");
            order.Add("SpelunkSurge:MarbleCrate");

            order.Add("Tacklebox:GemCrate");
            order.Add("Tacklebox:HellCrate");
            order.Add("Tacklebox:SandCrate");
            order.Add("Tacklebox:SeaCrate");
            order.Add("Tacklebox:SnowCrate");

            order.Add("Terraria2:JunkCrate");
            order.Add("Terraria2:QuestCrate");
            order.Add("Terraria2:ExpertCasket");

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

        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 6;
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