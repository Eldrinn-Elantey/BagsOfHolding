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
	public class GemBag : GenericHoldingBag
	{
        public static List<string> contents;
        public static List<string> noPickup;

        public override bool createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
            order.Add(""+ItemID.Amethyst);
			order.Add(""+ItemID.Topaz);
			order.Add(""+ItemID.Emerald);
			order.Add(""+ItemID.Sapphire);
			order.Add(""+ItemID.Ruby);
			order.Add(""+ItemID.Diamond);
            order.Add("" + ItemID.Amber);
			
			/*thorium support*/
			order.Add("ThoriumMod:LifeQuartz");
			order.Add("ThoriumMod:Pearl");
            order.Add("ThoriumMod:Opal");
			
			/*crystilium mod support*/
			order.Add("CrystiliumMod:ShinyGemstone");

            order.Add("Tremor:Aquamarine");
            order.Add("Tremor:LapisLazuli");

            order.Add("CivitasMod:Agate");
            order.Add("CivitasMod:Tourmaline");

            order.Add("CosmeticVariety:Aquamarine");
            order.Add("CosmeticVariety:Peridot");

            order.Add("EA:Item_3245");
            order.Add("EA:Item_3291");
            order.Add("EA:Item_3292");
            order.Add("EA:Item_2909");
            order.Add("EA:Item_2948");
            order.Add("EA:Item_3412");

            order.Add("ElementsAwoken:MagmaCrystal");

            order.Add("EnchantedJewels:BlackOpal");
            order.Add("EnchantedJewels:DawnStone");
            order.Add("EnchantedJewels:DuskStone");

            order.Add("Exodus:Peridot");
            order.Add("Exodus:Zircon");
            order.Add("Exodus:AirElement");
            order.Add("Exodus:DesertElement");
            order.Add("Exodus:FireElement");
            order.Add("Exodus:FrostElement");
            order.Add("Exodus:JungleElement");
            order.Add("Exodus:WaterElement");
            order.Add("Exodus:EnergyGem");
            order.Add("Exodus:GoldenRelic");
            
            order.Add("ForgottenMemories:Citrine");
            order.Add("ForgottenMemories:Spinel");
            order.Add("ForgottenMemories:Tourmaline");

            order.Add("SacredTools:DesertCrystal");

            order.Add("OldSchoolRuneScape:Dragonstone");
            order.Add("OldSchoolRuneScape:Onyx");
            order.Add("OldSchoolRuneScape:Zenyte");

            order.Add("SpelunkSurge:Moonstone");

            return true;
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Gem Bag");
            Tooltip.SetDefault("Automatically stores picked up gems in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Gemmes");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les gemmes si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Gemas");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda gemas no saco automáticamente se equipado como um acessório.");
        }

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}

        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 3;
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