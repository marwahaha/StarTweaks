using System.Collections.Generic;

using Microsoft.Xna.Framework;

using StardewModdingAPI;

using StardewValley;
using StardewValley.Menus;

namespace StarTweaks.UI {

    /// <summary>
    /// Handles the drawing and logic for the custom sort button in the players inventory.
    /// </summary>
    public class CustomSort {
        private ClickableTextureComponent organiseButton;
        private InventoryPage reflectedInventoryPage;
        private string hoverText = "";
        private Options.Option sortOption;

        private const int TOOLBAR_SIZE = 12;
        private const int SEPARATION_SPACE = 8;

        /// <summary>
        /// Constructor, sets up the following SMAPI events.
        /// <item>
        /// 
        ///   <term>Menu Changed</term>
        ///   <description>
        ///   Used to enable and disable SMAPI events, as well as get menu information from the base game.
        ///   For more information see <see cref="CustomSort.Display_MenuChanged(object sender, StardewModdingAPI.Events.MenuChangedEventArgs e)">.
        ///   </description>
        ///   
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sortOption">This is the <c>Option</c> used to represent the custom sort toggle being either on or off.</param>
        public CustomSort(Options.Option sortOption) {
            this.sortOption = sortOption;

            this.organiseButton = new ClickableTextureComponent("", new Rectangle(0, 0, 64, 64), "",
                "StarTweaks Organise", Game1.mouseCursors, new Rectangle(162, 440, 16, 16), 4f, false);

            ModEntry.Helper.Events.Display.MenuChanged += Display_MenuChanged;
        }

        /// <summary>
        /// If the sortOption is active then this will enable the events required for custom sort functionality
        /// otherwise it will remove these events from the event handler.
        /// 
        /// Also gets menu data so the button can be placed in the correct location.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI.</param>
        private void Display_MenuChanged(object sender, StardewModdingAPI.Events.MenuChangedEventArgs e) {
            if (!this.sortOption.IsActive) {
                ModEntry.Helper.Events.Input.ButtonPressed -= Input_ButtonPressed;
                ModEntry.Helper.Events.Input.CursorMoved -= Input_CursorMoved;

                ModEntry.Helper.Events.Display.RenderingActiveMenu -= Display_RenderingActiveMenu;
                ModEntry.Helper.Events.Display.RenderedActiveMenu -= Display_RenderedActiveMenu;
                return;
            } else {
                ModEntry.Helper.Events.Input.ButtonPressed += Input_ButtonPressed;
                ModEntry.Helper.Events.Input.CursorMoved += Input_CursorMoved;

                ModEntry.Helper.Events.Display.RenderingActiveMenu += Display_RenderingActiveMenu;
                ModEntry.Helper.Events.Display.RenderedActiveMenu += Display_RenderedActiveMenu;
            }

            GameMenu menu = Game1.activeClickableMenu as GameMenu;
            if (menu != null) {
                this.reflectedInventoryPage = ModEntry.Helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue()[0] as InventoryPage;
                this.organiseButton.bounds.X = this.reflectedInventoryPage.organizeButton.bounds.X + this.organiseButton.bounds.Width + SEPARATION_SPACE;
                this.organiseButton.bounds.Y = this.reflectedInventoryPage.organizeButton.bounds.Y;
            }
            
        }

        /// <summary>
        /// Event handler to render the hover tooltip to the screen after the rest of the SpriteBatch is drawn.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI, used to access the SpriteBatch.</param>
        private void Display_RenderedActiveMenu(object sender, StardewModdingAPI.Events.RenderedActiveMenuEventArgs e) {
            GameMenu menu = Game1.activeClickableMenu as GameMenu;
            if (menu != null && menu.currentTab == 0) {
                if (hoverText != "") {
                    IClickableMenu.drawToolTip(e.SpriteBatch, this.hoverText, "", null);
                }
            }
        }

        /// <summary>
        /// Event handler to render the button to the screen before the rest of the SpriteBatch is drawn.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI, used to access the SpriteBatch.</param>
        private void Display_RenderingActiveMenu(object sender, StardewModdingAPI.Events.RenderingActiveMenuEventArgs e) {
            GameMenu menu = Game1.activeClickableMenu as GameMenu;
            if (menu != null && menu.currentTab == 0) {
                organiseButton.draw(e.SpriteBatch, Color.White, (float)(0.860000014305115 + (double)organiseButton.bounds.Y / 20000.0));
            }
        }

        /// <summary>
        /// Event handler for checking if the button is being hovered over by the mouse cursor.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI, used here to get cursor position.</param>
        private void Input_CursorMoved(object sender, StardewModdingAPI.Events.CursorMovedEventArgs e) {
            if (organiseButton.containsPoint((int) e.NewPosition.ScreenPixels.X, (int) e.NewPosition.ScreenPixels.Y)) {
                hoverText = organiseButton.hoverText;
            } else {
                hoverText = "";
            }
            organiseButton.tryHover((int) e.NewPosition.ScreenPixels.X, (int) e.NewPosition.ScreenPixels.Y);
        }

        /// <summary>
        /// Event handler for sorting the inventory when the organise button is clicked.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI, used here to get cursor position.</param>
        private void Input_ButtonPressed(object sender, StardewModdingAPI.Events.ButtonPressedEventArgs e) {
            if (e.Button == SButton.MouseLeft && this.organiseButton.containsPoint((int) e.Cursor.ScreenPixels.X, (int) e.Cursor.ScreenPixels.Y)) {
                CustomOrganise((IList<Item>) Game1.player.items);
                Game1.playSound("Ship");
            }
        }

        private void CustomOrganise(IList<Item> items) {
            List<Item> sortedList = new List<Item>(items).GetRange(TOOLBAR_SIZE, items.Count - TOOLBAR_SIZE);
            sortedList.Sort();

            // Only changed the inventory slots that come after the first 12, which correspond to the toolbar slots.
            for (int i = TOOLBAR_SIZE; i < items.Count; ++i) {
                items[i] = (Item) null;
                items[i] = sortedList[sortedList.Count - 1 - i + TOOLBAR_SIZE];
            }

        }
    }
}
