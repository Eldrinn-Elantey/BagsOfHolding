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
	public class DyeMaterialBag : GenericHoldingBag
	{
        public static List<string> contents;
        public static List<string> noPickup;

        public override bool createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
            order.Add("" + ItemID.StrangePlant1);
            order.Add("" + ItemID.StrangePlant2);
            order.Add("" + ItemID.StrangePlant3);
            order.Add("" + ItemID.StrangePlant4);
            order.Add(""+ItemID.BlackInk);
            order.Add("" + ItemID.OrangeBloodroot);
            order.Add(""+ItemID.YellowMarigold);
            order.Add("" + ItemID.LimeKelp);
            order.Add("" + ItemID.GreenMushroom);
            order.Add("" + ItemID.TealMushroom);
            order.Add("" + ItemID.CyanHusk);
            order.Add("" + ItemID.SkyBlueFlower);
            order.Add("" + ItemID.BlueBerries);
            order.Add("" + ItemID.VioletHusk);
            order.Add("" + ItemID.PinkPricklyPear);

            return true;
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Dye Material Bag");
            Tooltip.SetDefault("Automatically stores natural dye materials and strange plants in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Teintures");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les matériaux de teintures si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Materiais de Tintura");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda materiais para obter tinturas no saco automáticamente se equipado como um acessório.");

        }

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}

        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 12;
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