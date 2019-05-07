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
	public class WoodBag : GenericHoldingBag
	{
        public static List<string> contents;
        public static List<string> noPickup;

        public override bool createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
            order.Add(""+ItemID.Wood);
			order.Add(""+ItemID.BorealWood);
			order.Add(""+ItemID.RichMahogany);
			order.Add(""+ItemID.Ebonwood);
			order.Add(""+ItemID.Shadewood);
			order.Add(""+ItemID.Pearlwood);
            order.Add("" + ItemID.PalmWood);
            order.Add("" + ItemID.SpookyWood);
            order.Add("" + ItemID.Acorn);
            order.Add("" + ItemID.Cactus);

            order.Add("ThoriumMod:YewWood");

			/*crystilium mod support*/
			order.Add("CrystiliumMod:CrystalWood");

            /*spirit mod Suppore*/
            order.Add("SpiritMod:SpiritWoodItem");

            order.Add("Tremor:GlacierWood");

            order.Add("CosmeticVariety:Floralwood");
            order.Add("CosmeticVariety:Starwood");
            order.Add("CosmeticVariety:Starseed");

            order.Add("EA:Item_3382");
            order.Add("EA:Item_3604");
            order.Add("EA:Item_3050");

            order.Add("Erilipah:GrayscaleWood");

            order.Add("Exodus:DarkWood");

            order.Add("SacredTools:FrostWood");
            order.Add("SacredTools:FlameWood");

            return true;
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Wood Bag");
            Tooltip.SetDefault("Automatically stores picked up wood and acorns in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Bois");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement le bois et les glands si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Madeira");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda madeiras no saco automáticamente se equipado como um acessório.");
        }

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}

        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 10;
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