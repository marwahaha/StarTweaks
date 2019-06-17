using System.Collections.Generic;
using StardewModdingAPI.Events;
using StardewValley.Menus;

namespace StarTweaks.Options {
    public class MenuExtension {
        IDictionary<string, Option> options;
        public MenuExtension(IDictionary<string, Option> options) {
            this.options = options;
            ModEntry.Helper.Events.Display.MenuChanged += Display_MenuChanged;
        }

        private void Display_MenuChanged(object sender, MenuChangedEventArgs e) {
            if (e.NewMenu is GameMenu) {
                GameMenu menu = e.NewMenu as GameMenu;
                addOptions(menu);
            }
        }

        private void addOptions(GameMenu menu) {
            OptionsPage optionsPage = (OptionsPage) ModEntry.Helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue()[6];
            List<OptionsElement> options = ModEntry.Helper.Reflection.GetField<List<OptionsElement>>(optionsPage, "options").GetValue();

            options.Add(new OptionsElement("StarTweaks:"));
            foreach(string optionID in this.options.Keys) {
                options.Add((OptionsElement)new CustomCheckbox(this.options[optionID]));
            }
        }
    }
}
