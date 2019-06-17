using System;
using System.Collections.Generic;
using System.Linq;

using StardewValley.Menus;
using StardewValley;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StarTweaks.UI {
    /// <summary>
    /// The main custom toolbar class allowing for the toolbar position to be changed as a toggle.
    /// Used to replace the base games <c>Toolbar</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Theoretically maintains compatibility with other mods as it is instantiated based on the existing toolbar at runtime.
    /// Contains mostly overridden base game <c>Toolbar class</c> functionality
    /// </remarks>
    public class CustomToolbar : Toolbar {

        private ButtonToolbarToggle toggleButton;
        private List<ClickableComponent> buttonList;
        private float transparency;
        private Item hoverItem;
        private string hoverTitle;

        /// <summary>
        /// Primary constructor, also sets up SMAPI events.
        /// Sets up the following SMAPI events;
        /// <list>
        /// <item>
        /// 
        ///   <term>Game loop Save loaded</term>
        ///   <description>
        ///   Used to replace base toolbar with custom toolbar.
        ///   For more information see <see cref="CustomToolbar.GameLoop_SaveLoaded(object, StardewModdingAPI.Events.SaveLoadedEventArgs)">.
        ///   </description>
        ///   
        /// </item>
        /// </list>
        /// </summary>
        /// 
        /// <remarks>
        /// Contains heavy use of SMAPI Reflection, in order to obtain private fields from the base class.
        /// </remarks>
        /// 
        /// <param name="toolbarOption">This is the <c>Option</c> used to represent the toolbar toggle being either on or off.</param>
        public CustomToolbar(Options.Option toolbarOption) : base() {

            this.toggleButton = new ButtonToolbarToggle(toolbarOption);
            this.buttonList = ModEntry.Helper.Reflection.GetField<List<ClickableComponent>>(this, "buttons").GetValue();
            this.transparency = ModEntry.Helper.Reflection.GetField<float>(this, "transparency").GetValue();
            this.hoverItem = ModEntry.Helper.Reflection.GetField<Item>(this, "hoverItem").GetValue();
            this.hoverTitle = ModEntry.Helper.Reflection.GetField<string>(this, "hoverTitle").GetValue();

            ModEntry.Helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
        }

        /// <summary>
        /// Event handler for swapping the real game toolbar for this custom one on save loading.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI.</param>
        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e) {

            this.toggleButton.Bounds = new Rectangle(Game1.viewport.Width / 2 - 384 + this.buttonList.Count * 64 + 16, 0, 32, 32);
            this.gameWindowSizeChanged(new Rectangle() , new Rectangle());
            Game1.onScreenMenus[0] = this;
        }

        /// <summary>
        /// An overridden method from <c>Toolbar</c> for when the resolution is changed.
        /// Is executed by base game code.
        /// </summary>
        /// 
        /// <remarks>
        /// The two bounds rectangles don't appear to be used for the base toolbar, so they can be empty <c>Rectangle</c> objects.
        /// </remarks>
        /// <param name="oldBounds">Bounds of the old toolbar.</param>
        /// <param name="newBounds">Bounds of the new toolbar.</param>
        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds) {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            this.toggleButton.Bounds.X = Game1.viewport.Width / 2 - 384 + this.buttonList.Count * 64 + 16;
        }

        /// <summary>
        /// This is executed when the cursor is hovering over the toolbar object in game.
        /// Used for showing the tool-tip on an item stored in the position being hovered at.
        /// </summary>
        /// <param name="x">The horizontal cursor position.</param>
        /// <param name="y">The vertical cursor position.</param>
        public override void performHoverAction(int x, int y) {

            this.hoverItem = (Item) null;
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

        /// <summary>
        /// Draws the toolbar sprite to the screen.
        /// Doesn't draw anything if there is a menu open.
        /// </summary>
        /// <remarks>
        /// Mostly the same as base game code, with modification to change how the y position is determined.
        /// Also sets the y position for the toggle button accordingly.
        /// </remarks>
        /// <param name="b">This is the XNA Sprite-batch which the sprite is added to.</param>
        public override void draw(SpriteBatch b) {
            if (Game1.activeClickableMenu != null) { return; }

            Point center = Game1.player.GetBoundingBox().Center;
            Vector2 globalPosition = new Vector2((float)center.X, (float)center.Y);
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, globalPosition);
            bool flag;

            // If the user has the toolbar pin option as true then decide position manually.
            // Otherwise decide it based on the player position.
            if (Game1.options.pinToolbarToggle) {

                flag = toggleButton.ToolbarPositionFlag;
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

            // When flag is false then the toolbar is drawn at the bottom of the screen.
            // When flag is true then the toolbar is drawn at the top of the screen.
            if (!flag) {
                this.yPositionOnScreen = Game1.viewport.Height;
                this.yPositionOnScreen += 8;
                this.yPositionOnScreen -= num;
            } else {
                this.yPositionOnScreen = 112;
                this.yPositionOnScreen -= 8;
                this.yPositionOnScreen += num;
            }

            // Offset by 24, as that centers the 32x32 sprite against the 64x64 sprite.
            toggleButton.Bounds.Y = this.yPositionOnScreen - 96 + 24;

            int positionOnScreen2 = this.yPositionOnScreen;
            
            // If we have changed y position since last draw frame then re-position the buttons.
            if (positionOnScreen1 != positionOnScreen2) {
                for (int i = 0; i < this.buttonList.Count; ++i) {
                    this.buttonList[i].bounds.Y = this.yPositionOnScreen - 96 + 8;
                }
                
            }

            // This is the main draw call for the toolbar sprite.
            IClickableMenu.drawTextureBox(b, Game1.menuTexture, this.toolbarTextSource, Game1.viewport.Width / 2 - 384 - 16,
                this.yPositionOnScreen - 96 - 8, 800, 96, Color.White * this.transparency, 1f, false);

            // This draws each button in buttonList and adds the key helper character in the top left.
            for (int i = 0; i < this.buttonList.Count; ++i) {
                // This is for drawing the button.
                Vector2 position = new Vector2((float)(Game1.viewport.Width / 2 - 384 + i * 64), (float)(this.yPositionOnScreen - 96 + 8));
                b.Draw(Game1.menuTexture, position, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture,
                    Game1.player.CurrentToolIndex == i ? 56 : 10, -1, -1)), Color.White * this.transparency);

                // This decides which character to display.
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

                // This draws the character.
                string text = str;
                b.DrawString(Game1.tinyFont, text, position + new Vector2(4f, -8f), Color.DimGray * this.transparency);

                this.buttonList[i].scale = Math.Max(1f, this.buttonList[i].scale - 0.025f);
                Vector2 location = new Vector2((float)(Game1.viewport.Width / 2 - 384 + i * 64), (float)(this.yPositionOnScreen - 96 + 8));

                // This draws the items inside the buttons, if there is an item there to draw.
                if (Game1.player.items.Count > i && Game1.player.items.ElementAt<Item>(i) != null) {
                    Game1.player.items[i].drawInMenu(b, location,
                        Game1.player.CurrentToolIndex == i ? 0.9f : this.buttonList.ElementAt<ClickableComponent>(i).scale * 0.8f, this.transparency, 0.88f);
                }
            }

            // This section draws the tool-tips if there is need, see <see cref="CustomToolbar.performHoverAction(int x, int y)"/> to see how this is decided.
            if (this.hoverItem == null) { return; }

            IClickableMenu.drawToolTip(b, this.hoverItem.getDescription(), this.hoverItem.DisplayName, this.hoverItem,
                false, -1, 0, -1, -1, (CraftingRecipe)null, -1);
            this.hoverItem = (Item) null;
        }
    }
}
