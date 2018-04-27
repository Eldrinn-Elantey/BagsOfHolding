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
	public class MushroomBag : GenericHoldingBag
	{
        public static List<string> contents;
        public static List<string> noPickup;

        public override void createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
            order.Add("" + ItemID.Mushroom);
            order.Add("" + ItemID.GlowingMushroom);
            order.Add("" + ItemID.VileMushroom);
            order.Add("" + ItemID.ViciousMushroom);
            order.Add(""+ItemID.GrassSeeds);
            order.Add("" + ItemID.JungleGrassSeeds);
            order.Add(""+ItemID.MushroomGrassSeeds);
            order.Add("" + ItemID.CorruptSeeds);
            order.Add("" + ItemID.CrimsonSeeds);
            order.Add("" + ItemID.HallowedSeeds);
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Mushroom Bag");
            Tooltip.SetDefault("Automatically stores mushrooms and grass seeds in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac à Champignons");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les champignons et les graines si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Cogumelos");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda cogumelos e sementes de grama no saco automáticamente se equipado como um acessório.");
        }

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}

        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 11;
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