using System.Collections.Generic;

using StardewValley.Menus;

namespace StarTweaks.Options {

    /// <summary>
    /// This class sets up and handles the data for the mod options system.
    /// </summary>
    public class Handler {
        /// <summary>
        /// This dictionary contains all of the options for the mod.
        /// </summary>
        public readonly IDictionary<string, Option> OptionDictionary = new Dictionary<string, Option>();

        private MenuExtension optionsMenu;
        private ModConfig config;

        /// <summary>
        /// The constructor sets up instance variables and sets up the following SMAPI event handlers.
        /// <list>
        /// <item>
        /// 
        ///   <term>Display Menu Changed</term>
        ///   <description>
        ///   Used to save the options to config when the menu is closed.
        ///   For more information see <see cref="Handler.Display_MenuChanged(object sender, MenuChangedEventArgs e)">.
        ///   </description>
        ///   
        /// </item>
        /// </list>
        /// </summary>
        public Handler() {
            this.config = ModEntry.Helper.ReadConfig<ModConfig>();

            OptionDictionary.Add(Keys.ToggleToolbarSwitch, new Option("Toolbar Switch", this.config.ToggleToolbarSwitch));
            OptionDictionary.Add(Keys.ToggleCustomSort, new Option("Custom Sort Button", this.config.ToggleCustomSort));

            optionsMenu = new MenuExtension(OptionDictionary);

            ModEntry.Helper.Events.Display.MenuChanged += Display_MenuChanged;
        }

        /// <summary>
        /// This is used to save the options as edited in the options menu into the config file.
        /// </summary>
        /// <param name="sender">The object from which the event originated.</param>
        /// <param name="e">These are the arguments sent along with the event from SMAPI, contains the previous and current menu's.</param>
        private void Display_MenuChanged(object sender, StardewModdingAPI.Events.MenuChangedEventArgs e) {
            if (e.OldMenu is GameMenu) {
                SaveOptions();
            }
        }

        /// <summary>
        /// Helper function, saves all the options into the config file.
        /// </summary>
        private void SaveOptions() {
            ModEntry.Monitor.Log("Saving Config");

            this.config.ToggleToolbarSwitch = OptionDictionary[Keys.ToggleToolbarSwitch].IsActive;
            this.config.ToggleCustomSort = OptionDictionary[Keys.ToggleCustomSort].IsActive;

            ModEntry.Helper.WriteConfig(this.config);
        }
    }
}
