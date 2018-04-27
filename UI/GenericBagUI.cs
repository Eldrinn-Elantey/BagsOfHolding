using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.GameInput;
using System.Collections.Generic;
using JPANsBagsOfHoldingMod.Items;
using Terraria.Localization;

namespace JPANsBagsOfHoldingMod.UI
{
	public class GenericBagUI : UIState
	{
	
		private static Mod bagMod;
		public static bool visible = false;
		public static GenericHoldingBag openBag = null;
		public static int useSlot = -1;
		public static Player player;
		
		
		private static List<string> presentItem = new List<string>();
		private static int presentIndex = 0;
        private static List<Item> presentItemItem = new List<Item>();
		
		private UIImageButton nextButton;
		private Texture2D[] nextTextures;
		private UIImageButton prevButton;
		private Texture2D[] prevTextures;
		
		
		public GenericBagUI(){
		}
		
		public GenericBagUI(Mod m){
			bagMod = m;
		}
		
		public override void OnInitialize()
		{
			if(bagMod == null)
				bagMod = ModLoader.GetMod("BagsOfHoldingMod");
			
			nextTextures = new Texture2D[2];
			nextTextures[0] = bagMod.GetTexture("UI/NextArrow");
			nextTextures[1] = bagMod.GetTexture("UI/NextArrow");
			nextButton = new UIImageButton(nextTextures[0]);
			
			nextButton.Left.Set(78f + 40f + 560f * 0.755f, 0.0f);
			nextButton.Top.Set((float)Main.instance.invBottom + 12f+ (float)(4 * 56) * 0.755f, 0.0f);
            nextButton.Width.Set(32.0f, 0.0f);
            nextButton.Height.Set(32.0f, 0.0f);
            nextButton.OnClick += new MouseEvent(NextButtonClicked);
			
			
			prevTextures = new Texture2D[2];
			prevTextures[0] = bagMod.GetTexture("UI/PrevArrow");
			prevTextures[1] = bagMod.GetTexture("UI/PrevArrow");
			prevButton = new UIImageButton(prevTextures[0]);
			
			prevButton.Left.Set(78f + 560f * 0.755f, 0.0f);
			prevButton.Top.Set((float)Main.instance.invBottom + 12f + (float)(4 * 56) * 0.755f, 0.0f);
			prevButton.Width.Set(32.0f, 0.0f);
            prevButton.Height.Set(32.0f, 0.0f);
            prevButton.OnClick += new MouseEvent(PrevButtonClicked);
			
			base.Append(nextButton);
            base.Append(prevButton);
		}
		
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if(!Main.playerInventory || player.chest != -1 || !isSameBag()){
				close(true);
				return;
			}
			
			try{
			
				DrawButtons(spriteBatch);
				DrawSlots(spriteBatch);
			}catch(Exception ex){
				BagsOfHoldingMod.debugChat("Error: " + ex);
			}
		}
		
		
		private void DrawSlots(SpriteBatch spriteBatch){
			bool rebuild = false;
			Main.inventoryScale = 0.755f;
			if (Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0f, 0f, 73f, (float)Main.instance.invBottom + 56, 560f * Main.inventoryScale, 224f * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
			{
				player.mouseInterface = true;
			}
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int num = (int)(73f + (float)(i * 56) * Main.inventoryScale);
					int num2 = (int)((float)Main.instance.invBottom + 12f + (float)((j+1) * 56) * Main.inventoryScale);
					int slot = i + j * 10;
					new Color(100, 100, 100, 100);
					Item itm = presentIndex + slot >= presentItem.Count ? new Item(): presentItemItem[presentIndex + slot];
					if (Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0f, 0f, (float)num, (float)num2, (float)Main.inventoryBackTexture.Width * Main.inventoryScale, (float)Main.inventoryBackTexture.Height * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
					{
						player.mouseInterface = true;
						ItemSlotHandle(itm, ref rebuild);
					}
					ItemSlot.Draw(spriteBatch, ref itm, 3, new Vector2((float)num, (float)num2), default(Color));
				}
			}
			
			if(rebuild){
				buildItem();
				if(presentItem.Count <= presentIndex && presentIndex > 0){
					presentIndex -= 40;
				}
			}
		}
		
		
		private void DrawButtons(SpriteBatch spriteBatch){
            Main.inventoryScale = 0.755f;
            if (presentItem.Count >= presentIndex + 41)
            {
                if (nextButton.IsMouseHovering)
                {
                    player.mouseInterface = true;
                    nextButton.SetImage(nextTextures[1]);
                    Main.hoverItemName = "Next Page";
                }
                else
                {
                    nextButton.SetImage(nextTextures[0]);
                }
                nextButton.SetVisibility(1.0f, 1.0f);
            }else
            {
                nextButton.SetVisibility(0.0f, 0.0f);
            }

            if (presentIndex >= 40)
            {
                if (prevButton.IsMouseHovering)
                {
                    player.mouseInterface = true;
                    prevButton.SetImage(prevTextures[1]);
                    Main.hoverItemName = "Previous Page";
                }
                else
                {
                    prevButton.SetImage(prevTextures[0]);
                }
				prevButton.SetVisibility(1.0f, 1.0f);
			}else{
				prevButton.SetVisibility(0.0f, 0.0f);
			}

		}
		
		private void ItemSlotHandle(Item itm, ref bool rebuild){

            string itmTag = GenericHoldingBag.ItemToTag(itm);
            if (Main.mouseLeftRelease && Main.mouseLeft){
				if(Main.mouseItem != null && Main.mouseItem.type != 0){
					Item t = Main.mouseItem;
					openBag.addItem(t);
					if(t.stack == 0){
						Main.mouseItem = new Item();
					}
					rebuild = true;
				}else{
					if(itm != null && itm.type != 0 && openBag.items.ContainsKey(itmTag))
                    {
					
						if(ItemSlot.ShiftInUse){
							if(withdrawItem(itm, ref rebuild)){
								Recipe.FindRecipes();
							}
							
						}else{
                            

                            int total = (int)Math.Min((long)(openBag.items[itmTag]), itm.maxStack);
							Main.mouseItem = itm.Clone();
							Main.mouseItem.stack = total;
					
							long l = (long)(openBag.items[itmTag]) - total;
					
							openBag.items.Remove(itmTag);
							if(l > 0){
								openBag.items[itmTag] = l;
							}else{
                                openBag.items[itmTag] = 0L;
                                itm.SetDefaults(0);
                                rebuild = true;
							}
						}
					}
				}
			}else if (Main.stackSplit <= 1 && Main.mouseRight && (itm != null && itm.type != 0 && openBag.items.ContainsKey(itmTag)))
			{
				if (itm.maxStack >= 1 && (Main.mouseItem.IsTheSameAs(itm) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
				{
					if (Main.mouseItem.type == 0)
					{
						Main.mouseItem = itm.Clone(); 
						Main.mouseItem.stack = 0;
					}
					Main.mouseItem.stack++;
					
                    
					long l = (long)(openBag.items[itmTag]) - 1;
					
					openBag.items.Remove(itmTag);
					if(l > 0){
						openBag.items[itmTag] = l;
					}else{
                        openBag.items[itmTag] = 0L;
                        itm.SetDefaults(0);
                        rebuild = true;
					}
					Recipe.FindRecipes();
					Main.soundInstanceMenuTick.Stop();
					Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
					Main.PlaySound(12, -1, -1, 1, 1f, 0f);
					if (Main.stackSplit == 0)
					{
						Main.stackSplit = 15;
					}
					else
					{
						Main.stackSplit = Main.stackDelay;
					}
				}
			}
		
		
		
			if(itm != null && itm.type != 0 && openBag.items.ContainsKey(itmTag))
            {
				Main.hoverItemName = itm.HoverName;
				object hoverItemName = Main.hoverItemName;
				Main.hoverItemName = string.Concat(new object[]
				{
					hoverItemName,
					" (",
					"" + (openBag.items[itmTag])+ " left in the bag",
					")"
				});
			}else{
			}
			
				
		}
		
		public void open(Player p, GenericHoldingBag bag, bool silent)
        {
            visible = true;
			player = p;
            Main.playerInventory = true;
            Main.recBigList = false;

            if (player.chest != -1)
                player.chest = -1;

            openBag = bag;
			useSlot = player.selectedItem;
			
			buildItem();
            if (!silent)
                Main.PlaySound(Terraria.ID.SoundID.MenuOpen);
        }

        public void close(bool silent)
        {
            visible = false;
			openBag = null;
			useSlot = -1;
            if (!silent)
                Main.PlaySound(Terraria.ID.SoundID.MenuClose);

        }
		
		public static void buildItem(){
			presentItem.Clear();
            presentItemItem.Clear();

            if (openBag.order == null || openBag.order.Count <= 0)
                return;
			for(int i = 0; i < openBag.order.Count; i++){
                string key = openBag.order[i];
				if(openBag.items.ContainsKey(key) && openBag.items.GetAsLong(key) > 0L) {
                    Item itm = GenericHoldingBag.getItemFromTag(key);
                    if (itm.type != 0)
                    {
                        presentItem.Add(key);
                        presentItemItem.Add(itm);
                    }
                    else
                    {
                        openBag.items.Remove(key);
                    }
				}
			}
		}
		
		
		private void NextButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
			if(presentItem.Count > presentIndex+40){
					presentIndex += 40;
				}
		}
		
		private void PrevButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
			if(presentIndex > 39){
				presentIndex -= 40;
			}
		}
		
		
		public bool isSameBag(){
			return openBag != null && useSlot != -1 && player.inventory[useSlot].modItem == openBag;
		}
		
		
		private bool withdrawItem(Item itm, ref bool rebuild){
            /*if(itm.type > 70 && itm.type < 75){
				long money = mergeAllCoins();
				for(int i = 0; i < 50; i++){
								
				}	
			}*/
            string itmTag = GenericHoldingBag.ItemToTag(itm);

            int total = (int)Math.Min(openBag.items.GetAsLong(itmTag), itm.maxStack);
			Item t = itm.Clone(); 
			t.stack = total;
			
			t = player.GetItem(player.whoAmI, t, false, true);
			total -= t.stack;
			
			long l = openBag.items.GetAsLong(itmTag) - total;
					
			openBag.items.Remove(itmTag);
			if(l > 0){
				openBag.items[itmTag] = l;
			}else{
                openBag.items[itmTag] = 0L;
                itm.SetDefaults(0);
                rebuild = true;
			}
			
			return true;
			
			
			
		}
		
		private long mergeAllCoins(){
			long ans = 0L;
			for(int i = 0; i < 54; i++){
				if(player.inventory[i].type == 71){
					ans += player.inventory[i].stack;
				}else if(player.inventory[i].type == 72){
					ans += player.inventory[i].stack*100;
				}else if(player.inventory[i].type == 73){
					ans += player.inventory[i].stack*10000;
				}else if(player.inventory[i].type == 74){
					ans += player.inventory[i].stack*1000000;
				}else{
				
				}
			}
			return ans;
		}
	}
}