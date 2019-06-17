using System.Collections.Generic;
using StardewModdingAPI.Events;
using StardewValley.Menus;

namespace StarTweaks.Options {
    
    /// <summary>
    /// This is used to handle adding new options into the existing base game option menu.
    /// Note that this does not create a separate mod options page but instead adds the options 
    /// to the end of the existing options page.
    /// </summary>
    /// 
    /// <remarks>
    /// Does not directly draw any options to the screen, instead adding them to a list in the base
    /// game code and allowing the base game to draw them.
    /// </remarks>

    public class MenuExtension {

        private IDictionary<string, Option> options;

        /// <summary>
        /// Initalises options, and sets up the following SMAPI event handlers.
        /// <list>
        /// <item>
        /// 
        ///   <term>Display Menu Changed</term>
        ///   <description>
        ///   Used to add in the options onto the end of the options page when the menu is opened.
        ///   For more information see <see cref="MenuExtension.Display_MenuChanged(object sender, MenuChangedEventArgs e)">.
        ///   </description>
        ///   
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="options">This is a dictionary with each key being a key from <c>Options.Keys</c> and the value being an <c>Option</c>.</param>
        public MenuExtension(IDictionary<string, Option> options) {
            this.options = options;

            ModEntry.Helper.Events.Display.MenuChanged += Display_MenuChanged;
        }

        /// <summary>
        /// Event handler, used to add in the options into the base game options menu.
        /// This occurs every time an instance of the base game <c>GameMenu</c> is the activeMenu.
        /// </summary>
        /// 
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI, contains the previous and current menu's.</param>
        private void Display_MenuChanged(object sender, MenuChangedEventArgs e) {
            if (e.NewMenu is GameMenu) {
                GameMenu menu = e.NewMenu as GameMenu;
                AddOptionsToMenu(menu);
            }
        }

        /// <summary>
        /// Helper function to add in the options to the menu.
        /// Iterates over each Option in the options dictionary and adds them as well as a header.
        /// </summary>
        /// 
        /// <remarks>
        /// Makes use of SMAPI reflection to get the existing options list to add to.
        /// </remarks>
        /// 
        /// <param name="menu">This is the menu that the options will be added to.</param>
        private void AddOptionsToMenu(GameMenu menu) {
            OptionsPage optionsPage = (OptionsPage) ModEntry.Helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue()[6];
            List<OptionsElement> options = ModEntry.Helper.Reflection.GetField<List<OptionsElement>>(optionsPage, "options").GetValue();

            options.Add(new OptionsElement("StarTweaks:"));
            foreach(string optionID in this.options.Keys) {
                options.Add((OptionsElement)new CustomCheckbox(this.options[optionID]));
            }
        }
    }
}
