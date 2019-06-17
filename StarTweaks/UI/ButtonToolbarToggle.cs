using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace StarTweaks.UI {
    /// <summary>
    /// This is a button this is used in conjunction with <c>CustomToolbar</c> to allow for toggling between the top
    /// and bottom position for
    /// </summary>
    public class ButtonToolbarToggle {
        private readonly Texture2D texture = ModEntry.Helper.Content.Load<Texture2D>("assets/ToolbarToggleUp.png");

        /// <summary>
        /// This flag tracks whether the toolbar should be at the top or bottom.
        /// True for top.
        /// False for bottom.
        /// </summary>
        public bool ToolbarPositionFlag;

        /// <summary>
        /// This is the <c>Option</c> that controls whether this button should appear or not.
        /// </summary>
        public Options.Option ToolbarOption;

        /// <summary>
        /// This is the boundary rectangle for the button.
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// This constructor creates the object without defining the bounds.
        /// It also sets up the following SMAPI event handlers
        /// <list>
        /// <item>
        /// 
        ///   <term>Display Rendering HUD</term>
        ///   <description>
        ///   Used to draw the button before the HUD is rendered.
        ///   For more information see <see cref="ButtonToolbarToggle.Display_RenderingHud(object sender, StardewModdingAPI.Events.RenderingHudEventArgs e)">.
        ///   </description>
        ///   
        /// </item>
        /// 
        /// <item>
        /// 
        ///   <term>Input Button Pressed</term>
        ///   <description>
        ///   Used to toggle toolbar position when the button is clicked.
        ///   For more information see <see cref="ButtonToolbarToggle.Input_ButtonPressed(object sender, StardewModdingAPI.Events.ButtonPressedEventArgs e)">.
        ///   </description>
        ///   
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="toolbarOption">This is the <c>Option</c> used to represent the toolbar toggle being either on or off.</param>
        public ButtonToolbarToggle(Options.Option toolbarOption) {
            this.ToolbarOption = toolbarOption;

            ModEntry.Helper.Events.Display.RenderingHud += Display_RenderingHud;
            ModEntry.Helper.Events.Input.ButtonPressed += Input_ButtonPressed;
        }

        /// <summary>
        /// This constructor is used when the full arguments are given.
        /// Calls the constructor that only takes <paramref name="toolbarOption"/>.
        /// Note that the width and height of the button is 32x32.
        /// </summary>
        /// <param name="toolbarOption">This is the <c>Option</c> used to represent the toolbar toggle being either on or off.</param>
        /// <param name="x">This is the x position the button is being initalised at.</param>
        /// <param name="y">This is the y position the button is being initalised at.</param>
        public ButtonToolbarToggle(Options.Option toolbarOption, int x, int y) : this(toolbarOption) {
            this.Bounds = new Rectangle(x, y, 32, 32);
        }

        /// <summary>
        /// Event handler for rendering the button sprite to the screen.
        /// Only renders if 1: There is no menu active.
        ///                 2: The toolbar is set to be pinned in the game options.
        ///                 3: The mod option for toolbar toggle is enabled.
        /// </summary>
        /// 
        /// <remarks>
        /// Done before the HUD rendering instead of after to allow the button to be hidden behind tool-tips.
        /// </remarks>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI, contains the Sprite-batch</param>
        private void Display_RenderingHud(object sender, StardewModdingAPI.Events.RenderingHudEventArgs e) {
            if (Game1.activeClickableMenu == null && Game1.options.pinToolbarToggle && this.ToolbarOption.GetActive()) { 
                e.SpriteBatch.Draw(this.texture, new Vector2(this.Bounds.X, this.Bounds.Y), Color.White);
            }
        }

        /// <summary>
        /// Event handler for clicking the button.
        /// The button is clicked if it is the left mouse button that is pressed, the cursor is within the button bounds
        /// and the mod option for toolbar toggle is enabled.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI, contains which button was pressed and cursor position.</param>
        private void Input_ButtonPressed(object sender, StardewModdingAPI.Events.ButtonPressedEventArgs e) {
            if (e.Button == SButton.MouseLeft && this.ToolbarOption.GetActive() &&
                this.Bounds.Contains((int) e.Cursor.ScreenPixels.X, (int) e.Cursor.ScreenPixels.Y)) {

                this.ToolbarPositionFlag = !ToolbarPositionFlag;
            }
        }
    }
}
