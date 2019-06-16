using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace StarTweaks.UI {
    public class ButtonToolbarToggle {
        private readonly Texture2D texture = ModEntry.helper.Content.Load<Texture2D>("assets/ToolbarToggleUp.png");
        public Rectangle bounds;

        // flag is false for toolbar at bottom and true for toolbar at the top 
        public bool toolbarPositionFlag;
        public Options.Option toolbarOption;

        public ButtonToolbarToggle(Options.Option toolbarOption, int x, int y) : this(toolbarOption) {
            this.bounds = new Rectangle(x, y, 32, 32);
        }

        public ButtonToolbarToggle(Options.Option toolbarOption) {
            this.toolbarOption = toolbarOption;

            ModEntry.helper.Events.Display.RenderingHud += Display_RenderingHud;
            ModEntry.helper.Events.Input.ButtonPressed += Input_ButtonPressed;
        }

        private void Display_RenderingHud(object sender, StardewModdingAPI.Events.RenderingHudEventArgs e) {
            if (Game1.activeClickableMenu == null && Game1.options.pinToolbarToggle && this.toolbarOption.GetActive()) { 
                e.SpriteBatch.Draw(this.texture, new Vector2(this.bounds.X, this.bounds.Y), Color.White);
            }
        }

        private void Input_ButtonPressed(object sender, StardewModdingAPI.Events.ButtonPressedEventArgs e) {
            if (e.Button == SButton.MouseLeft && this.toolbarOption.GetActive() &&
                this.bounds.Contains((int)e.Cursor.ScreenPixels.X, (int)e.Cursor.ScreenPixels.Y)) {

                this.toolbarPositionFlag = !toolbarPositionFlag;
            }
        }
    }
}
