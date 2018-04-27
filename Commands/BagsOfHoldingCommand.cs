using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JPANsBagsOfHoldingMod.Items;
using Terraria.ModLoader;

namespace JPANsBagsOfHoldingMod.Commands
{
    public class BagsOfHoldingCommand : ModCommand
    {
        public override CommandType Type
        {
            get { return CommandType.Chat; }
        }

        public override string Command
        {
            get { return "BagsOfHolding"; }
        }

        public override string Description
        {
            get { return "Use with \"reloadOrder\" with a held bag to reload its order, \"reloadOmnibag\" to reload omnibag's content list and \"remakeBags\" to remake all bag orders into the configs."; }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if(args.Length != 1)
            {
                BagsOfHoldingMod.sendChat("Use with \"reloadOrder\" with a held bag to reload its order, \"reloadOmnibag\" to reload omnibag's content list and \"remakeBags\" to remake all bag orders into the configs.");
                return;
            }
            if((args[0].Equals("reloadOrder") || args[0].Equals("\"reloadOrder\""))&& caller.Player.HeldItem.modItem != null && caller.Player.HeldItem.modItem is GenericHoldingBag)
            {
                GenericHoldingBag bag = caller.Player.HeldItem.modItem as GenericHoldingBag;
                bag.setupItemList();
                BagsOfHoldingMod.sendChat("Reloaded bag order");
                return;
            }
            if (args[0].Equals("reloadOmnibag") || args[0].Equals("\"reloadOmnibag\""))
            {
                TrueOmniBag.contents2 = null;
                BagPlayer.reloadOmnibag();
                BagsOfHoldingMod.sendChat("Recreated Omnibag order.");
                return;
            }

            if (args[0].Equals("remakeBags") || args[0].Equals("\"remakeBags\""))
            {
                GenericHoldingBag bg = new BaitBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new ChunkBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new CoinBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new CrateBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new DirtBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new DyeMaterialBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new FishBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new GemBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new MushroomBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new OreBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new PlantBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();
                bg = new WoodBag();
                resetBagConfig(bg);
                bg.remakeFromConfig();

                FishingBag.resetContents();
                NatureBag.resetContents();
                OmniBag.resetContents();

                BagsOfHoldingMod.sendChat("Reset all bag orders to the original order list.");
            }

            }

        private void resetBagConfig(GenericHoldingBag bg)
        {
            bg.config.Put("order", new String[0]);
            bg.config.Save();
        }
    }
}
