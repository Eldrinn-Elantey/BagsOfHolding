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
	public class BaitBag : GenericHoldingBag
	{

        private static Preferences bagConfig;

        public static List<string> contents;
        public static List<string> noPickup;

        public override void createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
            order.Add("" + ItemID.TruffleWorm);
            order.Add("" + ItemID.MasterBait);
            order.Add("" + ItemID.GoldButterfly);
            order.Add("" + ItemID.GoldGrasshopper);
            order.Add("" + ItemID.GoldWorm);
            order.Add("" + ItemID.TreeNymphButterfly);
            order.Add("" + ItemID.Buggy);
            order.Add("" + ItemID.EnchantedNightcrawler);
            order.Add("" + ItemID.LightningBug);
            order.Add("" + ItemID.PurpleEmperorButterfly);
            order.Add("" + ItemID.JourneymanBait);
            order.Add("" + ItemID.RedAdmiralButterfly);
            order.Add("" + ItemID.JuliaButterfly);
            order.Add("" + ItemID.Worm);
            order.Add("" + ItemID.Sluggy);
            order.Add("" + ItemID.UlyssesButterfly);
            order.Add("" + ItemID.Firefly);
            order.Add("" + ItemID.BlueJellyfish);
            order.Add("" + ItemID.GreenJellyfish);
            order.Add("" + ItemID.PinkJellyfish);
            order.Add("" + ItemID.ApprenticeBait);
            order.Add("" + ItemID.ZebraSwallowtailButterfly);
            order.Add("" + ItemID.BlackScorpion);
            order.Add("" + ItemID.GlowingSnail);
            order.Add("" + ItemID.Grubby);
            order.Add("" + ItemID.SulphurButterfly);
            order.Add("" + ItemID.Grasshopper);
            order.Add("" + ItemID.Scorpion);
            order.Add("" + ItemID.Snail);
            order.Add("" + ItemID.MonarchButterfly);

            /*thorium support*/
            order.Add("ThoriumMod:PulseLure");
            order.Add("ThoriumMod:AmberButterfly");
            order.Add("ThoriumMod:AmethystButterfly");
            order.Add("ThoriumMod:AncientWingButterfly");
            order.Add("ThoriumMod:AntlionButterfly");
            order.Add("ThoriumMod:AvianButterfly");
            order.Add("ThoriumMod:BlinkrootButterfly");
            order.Add("ThoriumMod:BloodiedButterfly");
            order.Add("ThoriumMod:BlueDungeonButterfly");
            order.Add("ThoriumMod:BombNymphButterfly");
            order.Add("ThoriumMod:BoneButterfly");
            order.Add("ThoriumMod:BorderedButterfly");
            order.Add("ThoriumMod:ChlorophyteButterfly");
            order.Add("ThoriumMod:ClowdwingButterfly");
            order.Add("ThoriumMod:CorruptButterfly");
            order.Add("ThoriumMod:CrimsonButterfly");
            order.Add("ThoriumMod:CursedFlameButterfly");
            order.Add("ThoriumMod:DaybloomButterfly");
            order.Add("ThoriumMod:DeathweedButterfly");
            order.Add("ThoriumMod:DiamondButterfly");
            order.Add("ThoriumMod:EctoplasimicButterfly");
            order.Add("ThoriumMod:EmeraldButterfly");
            order.Add("ThoriumMod:EnergyWingButterfly");
            order.Add("ThoriumMod:FireblossomButterfly");
            order.Add("ThoriumMod:FrozenButterfly");
            order.Add("ThoriumMod:FuschiaButterfly");
            order.Add("ThoriumMod:GreenDungeonButterfly");
            order.Add("ThoriumMod:HallowedButterfly");
            order.Add("ThoriumMod:HellwingButterfly");
            order.Add("ThoriumMod:IchorButterfly");
            order.Add("ThoriumMod:JungleSporeButterfly");
            order.Add("ThoriumMod:LifeCrystalButterfly");
            order.Add("ThoriumMod:LifeFruitButterfly");
            order.Add("ThoriumMod:LuminiteButterfly");
            order.Add("ThoriumMod:MeteorButterfly");
            order.Add("ThoriumMod:MoonglowButterfly");
            order.Add("ThoriumMod:MushroomButterfly");
            order.Add("ThoriumMod:ObsidianButterfly");
            order.Add("ThoriumMod:PinkDungeonButterfly");
            order.Add("ThoriumMod:PinkMonarch");
            order.Add("ThoriumMod:PixiewingButterfly");
            order.Add("ThoriumMod:RubyButterfly");
            order.Add("ThoriumMod:SandButterfly");
            order.Add("ThoriumMod:SapphireButterfly");
            order.Add("ThoriumMod:ShiverthornButterfly");
            order.Add("ThoriumMod:ShroomiteButterfly");
            order.Add("ThoriumMod:SwallowtailButterfly");
            order.Add("ThoriumMod:SweetWingButterfly");
            order.Add("ThoriumMod:TempleButterfly");
            order.Add("ThoriumMod:TotalityButterfly");
            order.Add("ThoriumMod:WaterleafButterfly");
            order.Add("ThoriumMod:ZereneButterfly");

            /*Fishing3 mod support*/
            order.Add("Fishing3:SoulBait");

            /*Battlerods support*/
            order.Add("UnuBattleRods:IceyWorm");
            order.Add("UnuBattleRods:BetsyCurseApprenticeBait");
            order.Add("UnuBattleRods:BetsyCurseBait");
            order.Add("UnuBattleRods:BetsyCurseMasterBait");
            order.Add("UnuBattleRods:ConfusionApprenticeBait");
            order.Add("UnuBattleRods:ConfusionBait");
            order.Add("UnuBattleRods:ConfusionMasterBait");
            order.Add("UnuBattleRods:CursedFlameApprenticeBait");
            order.Add("UnuBattleRods:CursedFlameBait");
            order.Add("UnuBattleRods:CursedFlameMasterBait");
            order.Add("UnuBattleRods:FireApprenticeBait");
            order.Add("UnuBattleRods:FireBait");
            order.Add("UnuBattleRods:FireMasterBait");
            order.Add("UnuBattleRods:FrostburnApprenticeBait");
            order.Add("UnuBattleRods:FrostburnBait");
            order.Add("UnuBattleRods:FrostburnMasterBait");
            order.Add("UnuBattleRods:FrostfireApprenticeBait");
            order.Add("UnuBattleRods:FrostfireBait");
            order.Add("UnuBattleRods:FrostfireMasterBait");
            order.Add("UnuBattleRods:IchorApprenticeBait");
            order.Add("UnuBattleRods:IchorBait");
            order.Add("UnuBattleRods:IchorMasterBait");
            order.Add("UnuBattleRods:MidasApprenticeBait");
            order.Add("UnuBattleRods:MidasBait");
            order.Add("UnuBattleRods:MidasMasterBait");
            order.Add("UnuBattleRods:OilApprenticeBait");
            order.Add("UnuBattleRods:OilBait");
            order.Add("UnuBattleRods:OilMasterBait");
            order.Add("UnuBattleRods:PoisonApprenticeBait");
            order.Add("UnuBattleRods:PoisonBait");
            order.Add("UnuBattleRods:PoisonMasterBait");
            order.Add("UnuBattleRods:ShadowflameApprenticeBait");
            order.Add("UnuBattleRods:ShadowflameBait");
            order.Add("UnuBattleRods:ShadowflameMasterBait");
            order.Add("UnuBattleRods:SolarfireApprenticeBait");
            order.Add("UnuBattleRods:SolarfireBait");
            order.Add("UnuBattleRods:SolarfireMasterBait");
            order.Add("UnuBattleRods:VenomApprenticeBait");
            order.Add("UnuBattleRods:VenomBait");
            order.Add("UnuBattleRods:VenomMasterBait");

            order.Add("GRealm:GlistenbackBeetle");

            order.Add("JoostMod:SucculentCactus");

            order.Add("Wildlife:Firebug");
            order.Add("Wildlife:Dragonfly");
            order.Add("Wildlife:FairyB");
            order.Add("Wildlife:FairyG");
            order.Add("Wildlife:FairyR");

            order.Add("Tacklebox:CrabClaw");
            order.Add("Tacklebox:Glowfly");
            order.Add("Tacklebox:GoldJelly");
            order.Add("Tacklebox:Guppy");
            order.Add("Tacklebox:SpecialBait");
            order.Add("Tacklebox:Wiggler");
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

        public override void setupItemList()
        {
            bagID = 5;
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