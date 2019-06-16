using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using StardewValley.Menus;
using StardewValley;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace StarTweaks.UI {
    public class CustomToolbar : Toolbar {
        private ButtonToolbarToggle toggleButton;
        private List<ClickableComponent> buttonList;
        private float transparency;
        private Item hoverItem;
        private string hoverTitle;

        public CustomToolbar(Options.Option toolbarOption) : base() {
            this.toggleButton = new ButtonToolbarToggle(toolbarOption);
            this.buttonList = ModEntry.helper.Reflection.GetField<List<ClickableComponent>>(this, "buttons").GetValue();
            this.transparency = ModEntry.helper.Reflection.GetField<float>(this, "transparency").GetValue();
            this.hoverItem = ModEntry.helper.Reflection.GetField<Item>(this, "hoverItem").GetValue();
            this.hoverTitle = ModEntry.helper.Reflection.GetField<string>(this, "hoverTitle").GetValue();

            ModEntry.helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
        }

        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e) {
            this.toggleButton.bounds = new Rectangle(Game1.viewport.Width / 2 - 384 + 12 * 64 + 16, 0, 32, 32);
            this.gameWindowSizeChanged(new Rectangle() , new Rectangle());
            Game1.onScreenMenus[0] = this;
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds) {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            this.toggleButton.bounds.X = Game1.viewport.Width / 2 - 384 + 12 * 64 + 16;
        }

        public override void performHoverAction(int x, int y) {

            this.hoverItem = (Item)null;
            foreach (ClickableComponent button in this.buttonList) {
                if (button.containsPoint(x, y)) {
                    int int32 = Convert.ToInt32(button.name);
                    if (int32 < Game1.player.items.Count && Game1.player.items[int32] != null) {
                        button.scale = Math.Min(button.scale + 0.05f, 1.1f);
                        this.hoverTitle = Game1.player.items[int32].DisplayName;
                        this.hoverItem = Game1.player.items[int32];
                    }
                } else {
                    button.scale = Math.Max(button.scale - 0.025f, 1f);
                }
            }
        }

        public override void draw(SpriteBatch b) {
            //if (!Game1.options.pinToolbarToggle) { base.draw(b); return; }
            if (Game1.activeClickableMenu != null) { return; }

            Point center = Game1.player.GetBoundingBox().Center;
            Vector2 globalPosition = new Vector2((float)center.X, (float)center.Y);
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, globalPosition);
            bool flag;

            if (Game1.options.pinToolbarToggle) {
                flag = toggleButton.toolbarPositionFlag;
                this.transparency = Math.Min(1f, this.transparency + 0.075f);

                if ((double)local.Y > (double)(Game1.viewport.Height - 192)) {
                    this.transparency = Math.Max(0.33f, this.transparency - 0.15f);
                }
            } else {
                flag = (double)local.Y > (double)(Game1.viewport.Height / 2 + 64);
                this.transparency = 1f;
            }

            int num = Utility.makeSafeMarginY(8);
            int positionOnScreen1 = this.yPositionOnScreen;

            if (!flag) {
                this.yPositionOnScreen = Game1.viewport.Height;
                this.yPositionOnScreen += 8;
                this.yPositionOnScreen -= num;
            } else {
                this.yPositionOnScreen = 112;
                this.yPositionOnScreen -= 8;
                this.yPositionOnScreen += num;
            }
            toggleButton.bounds.Y = this.yPositionOnScreen - 96 + 24;

            int positionOnScreen2 = this.yPositionOnScreen;
            
            if (positionOnScreen1 != positionOnScreen2) {
                for (int i = 0; i < 12; ++i) {
                    this.buttonList[i].bounds.Y = this.yPositionOnScreen - 96 + 8;
                }
                
            }

            IClickableMenu.drawTextureBox(b, Game1.menuTexture, this.toolbarTextSource, Game1.viewport.Width / 2 - 384 - 16,
                this.yPositionOnScreen - 96 - 8, 800, 96, Color.White * this.transparency, 1f, false);
            for (int i = 0; i < 12; ++i) {
                Vector2 position = new Vector2((float)(Game1.viewport.Width / 2 - 384 + i * 64), (float)(this.yPositionOnScreen - 96 + 8));
                b.Draw(Game1.menuTexture, position, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture,
                    Game1.player.CurrentToolIndex == i ? 56 : 10, -1, -1)), Color.White * this.transparency);

                string str;
                switch (i) {
                    case 9:
                        str = "0";
                        break;
                    case 10:
                        str = "-";
                        break;
                    case 11:
                        str = "=";
                        break;
                    default:
                        str = string.Concat((object) (i + 1));
                        break;
                }

                string text = str;
                b.DrawString(Game1.tinyFont, text, position + new Vector2(4f, -8f), Color.DimGray * this.transparency);

                this.buttonList[i].scale = Math.Max(1f, this.buttonList[i].scale - 0.025f);
                Vector2 location = new Vector2((float)(Game1.viewport.Width / 2 - 384 + i * 64), (float)(this.yPositionOnScreen - 96 + 8));
                if (Game1.player.items.Count > i && Game1.player.items.ElementAt<Item>(i) != null) {
                    Game1.player.items[i].drawInMenu(b, location,
                        Game1.player.CurrentToolIndex == i ? 0.9f : this.buttonList.ElementAt<ClickableComponent>(i).scale * 0.8f, this.transparency, 0.88f);
                }
            }

            if (this.hoverItem == null) { return; }

            IClickableMenu.drawToolTip(b, this.hoverItem.getDescription(), this.hoverItem.DisplayName, this.hoverItem,
                false, -1, 0, -1, -1, (CraftingRecipe)null, -1);
            this.hoverItem = (Item) null;
        }
    }
}
