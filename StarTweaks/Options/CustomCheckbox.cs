using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewValley;
using StardewValley.Menus;

namespace StarTweaks.Options {
    public class CustomCheckbox : OptionsElement {
        private Option option;
        private bool isChecked;

        public CustomCheckbox(Option option, int x = -1, int y = -1, int whichOption = -2) : base(option.GetLabel(), x, y, 36, 36, whichOption) {
            this.option = option;
            this.isChecked = option.GetActive();
        }

        public override void receiveLeftClick(int x, int y) {
            if (!this.greyedOut) {
                base.receiveLeftClick(x, y);
                this.isChecked = !isChecked;
                option.Toggle();
                Game1.playSound("drumkit6");
            }
        }

        public override void draw(SpriteBatch b, int slotX, int slotY) {
            b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X), (float)(slotY + this.bounds.Y)),
                new Rectangle?(this.isChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked),
                Color.White * (this.greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.4f);
            base.draw(b, slotX, slotY);
        }
    }
}
