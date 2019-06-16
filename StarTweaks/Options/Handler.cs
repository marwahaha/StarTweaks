using System.Collections.Generic;
using StardewValley.Menus;

namespace StarTweaks.Options {
    public class Handler {
        public readonly IDictionary<string, Option> optionDictionary = new Dictionary<string, Option>();
        private MenuExtension optionsMenu;
        private ModConfig config;
        public Handler() {
            this.config = ModEntry.helper.ReadConfig<ModConfig>();

            optionDictionary.Add(Keys.ToggleToolbarSwitch, new Option("Toolbar Switch", this.config.ToggleToolbarSwitch));

            optionsMenu = new MenuExtension(optionDictionary);

            ModEntry.helper.Events.Display.MenuChanged += Display_MenuChanged;
        }

        private void Display_MenuChanged(object sender, StardewModdingAPI.Events.MenuChangedEventArgs e) {
            if (e.OldMenu is GameMenu) {
                saveOptions();
            }
        }

        private void saveOptions() {
            ModEntry.monitor.Log("Saving Config");

            this.config.ToggleToolbarSwitch = optionDictionary[Keys.ToggleToolbarSwitch].GetActive();

            ModEntry.helper.WriteConfig(this.config);
        }
    }
}
