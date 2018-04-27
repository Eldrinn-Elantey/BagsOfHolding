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
	public class FishBag : GenericHoldingBag
	{

        public static List<string> contents;
        public static List<string> noPickup;

        public override void createDefaultItemList()
        {
            preventPickup = new List<string>();
            order = new List<string>();
            order.Add("" + ItemID.FlarefinKoi);
            order.Add("" + ItemID.Obsidifish);
            order.Add("" + ItemID.Honeyfin);
            order.Add(""+ItemID.ArmoredCavefish);
            order.Add("" + ItemID.SpecularFish);
            order.Add("" + ItemID.Damselfish);
            order.Add("" + ItemID.Stinkfish);
            order.Add("" + ItemID.Ebonkoi);
            order.Add("" + ItemID.CrimsonTigerfish);
            order.Add("" + ItemID.Hemopiranha);
            order.Add("" + ItemID.ChaosFish);
            order.Add("" + ItemID.PrincessFish);
            order.Add("" + ItemID.Prismite);
            order.Add("" + ItemID.DoubleCod);
            order.Add("" + ItemID.VariegatedLardfish);
            order.Add(""+ItemID.FrostMinnow);
            order.Add("" + ItemID.GoldenCarp);
            order.Add(""+ItemID.Bass);
            order.Add("" + ItemID.AtlanticCod);
            order.Add("" + ItemID.Salmon);
            order.Add("" + ItemID.Trout);
            order.Add("" + ItemID.Tuna);
            order.Add("" + ItemID.RedSnapper);
            order.Add("" + ItemID.Shrimp);
            order.Add("" + ItemID.NeonTetra);

            order.Add("" + ItemID.OldShoe);
            order.Add("" + ItemID.TinCan);
            order.Add("" + ItemID.FishingSeaweed);
            order.Add("" + ItemID.Seaweed);

            order.Add("EchoesoftheAncients:Life_Fish");

            order.Add("SacredTools:Blazefury");
            order.Add("SacredTools:FieryFin");

            order.Add("Tacklebox:Anglerfish");
            order.Add("Tacklebox:Boxfish");
            order.Add("Tacklebox:Clusterfish");
            order.Add("Tacklebox:Coelacanth");
            order.Add("Tacklebox:DesertEel");
            order.Add("Tacklebox:Flounder");
            order.Add("Tacklebox:Flyfish");
            order.Add("Tacklebox:Frostedfin");
            order.Add("Tacklebox:Gamifish");
            order.Add("Tacklebox:Glittergill");
            order.Add("Tacklebox:HivePuffer");
            order.Add("Tacklebox:LavaEel");
            order.Add("Tacklebox:Lobster");
            order.Add("Tacklebox:Mushfin");
            order.Add("Tacklebox:Plankton");
            order.Add("Tacklebox:PrettyGuppy");
            order.Add("Tacklebox:Pupfish");
            order.Add("Tacklebox:RedHerring");
            order.Add("Tacklebox:SandShark");
            order.Add("Tacklebox:Sardine");
            order.Add("Tacklebox:Spacefish");
            order.Add("Tacklebox:Sturgeon");
            order.Add("Tacklebox:Superglub");
            order.Add("Tacklebox:Sweetglub");
            order.Add("Tacklebox:Venomfish");
            order.Add("Tacklebox:Whiskeyfish");

            order.Add("Tacklebox:Driftwood");
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Fish Bag");
            Tooltip.SetDefault("Automatically stores picked up fish (and junk) in the bag for you if equipped as an accessory.");
            DisplayName.AddTranslation(GameCulture.French, "Sac de Poissons");
            Tooltip.AddTranslation(GameCulture.French, "Récupère automatiquement les poissons (et les déchets) si le sac est équippé dans les accessoires.");
            DisplayName.AddTranslation(GameCulture.Portuguese, "Saco de Peixes");
            Tooltip.AddTranslation(GameCulture.Portuguese, "Guarda os peixes (e lixo) no saco automáticamente se equipado como um acessório.");
        }

        public override void SetDefaults()
		{
			base.SetDefaults();
			item.value = Item.sellPrice(0,0,15,0);
		}

        private Preferences bagConfig;

        public override void setupItemList()
        {
            bagID = 7;
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