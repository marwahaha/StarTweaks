using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewValley;
using StardewValley.Menus;

namespace StarTweaks.Options {

    /// <summary>
    /// This is the check-box that each boolean <c>Option</c> uses in the options menu.
    /// Inherits from base game <c>OptionsElement</c>.
    /// </summary>
    public class CustomCheckbox : OptionsElement {
        private Option option;
        private bool isChecked;

        /// <summary>
        /// Constructor to initalise the check-box.
        /// Also calls the base constructor for OptionsElement to setup base attributes.
        /// </summary>
        /// <param name="option">This is the mod option relating to this check-box.</param>
        /// <param name="x">The x position of the check-box, usually left at -1 as this is changed by the options menu anyway.</param>
        /// <param name="y">The y position of the check-box, usually left at -1 as this is changed by the options menu anyway.</param>
        /// <param name="whichOption">This is a leftover from the base game code, should be left at -2, as values >= -1 have meaning in the base game code.</param>
        public CustomCheckbox(Option option, int x = -1, int y = -1, int whichOption = -2) : base(option.Label, x, y, 36, 36, whichOption) {
            this.option = option;
            this.isChecked = option.IsActive;
        }

        /// <summary>
        /// Overridden function from <c>OptionsElement</c> used to toggle the option and check-box state.
        /// </summary>
        /// <param name="x">x position of the mouse cursor.</param>
        /// <param name="y">y position of the mouse cursor.</param>
        public override void receiveLeftClick(int x, int y) {
            if (!this.greyedOut) {
                base.receiveLeftClick(x, y);
                this.isChecked = !isChecked;
                option.Toggle();
                Game1.playSound("drumkit6");
            }
        }

        /// <summary>
        /// Draws the check-box in the menu.
        /// </summary>
        /// <param name="b">The XNA sprite batch to add the check-box sprite onto.</param>
        /// <param name="slotX">This is the x position of the slot for the check-box to fit in.</param>
        /// <param name="slotY">This is the y position of the slot for the check-box to fit in.</param>
        public override void draw(SpriteBatch b, int slotX, int slotY) {
            // Creates a null-able Rectangle, where if the check-box is checked then it uses the checked check-box from the base game
            // and if unchecked it uses the unchecked check-box from the base game.
            Rectangle? tempRectangle = new Rectangle?(this.isChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked);

            // Draws the check-box.
            b.Draw(Game1.mouseCursors, new Vector2((float) (slotX + this.bounds.X), (float) (slotY + this.bounds.Y)),
                tempRectangle, Color.White * (this.greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.4f);

            base.draw(b, slotX, slotY);
        }
    }
}
