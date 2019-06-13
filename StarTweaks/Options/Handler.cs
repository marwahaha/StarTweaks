using System.Collections.Generic;
using StardewValley.Menus;

namespace StarTweaks.Options {
    public class Handler {
        private readonly IDictionary<string, Option> optionDictionary = new Dictionary<string, Option>();
        private MenuExtension optionsMenu;
        private ModConfig config;
        public Handler() {
            this.config = ModEntry.helper.ReadConfig<ModConfig>();

            optionDictionary.Add(Keys.ToggleToolbarSwitch, new Option("Toolbar Switch", this.config.ToggleToolbarSwitch));
            optionDictionary.Add(Keys.ToggleToolbarSwitch1, new Option("Toolbar Switch1", this.config.ToggleToolbarSwitch1));
            optionDictionary.Add(Keys.ToggleToolbarSwitch2, new Option("Toolbar Switch2", this.config.ToggleToolbarSwitch2));
            optionDictionary.Add(Keys.ToggleToolbarSwitch3, new Option("Toolbar Switch3", this.config.ToggleToolbarSwitch3));

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
            this.config.ToggleToolbarSwitch1 = optionDictionary[Keys.ToggleToolbarSwitch1].GetActive();
            this.config.ToggleToolbarSwitch2 = optionDictionary[Keys.ToggleToolbarSwitch2].GetActive();
            this.config.ToggleToolbarSwitch3 = optionDictionary[Keys.ToggleToolbarSwitch3].GetActive();

            ModEntry.helper.WriteConfig(this.config);
        }
    }
}
