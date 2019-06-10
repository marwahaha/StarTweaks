using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace StarTweaks {
    public class OptionsMenuExtension {

        private List<OptionsElement> optionPageElements;
        private bool addedOptions;
        private IModHelper helper;
        private IMonitor monitor;
        public OptionsMenuExtension(IModHelper Helper, IMonitor Monitor) {
            this.helper = Helper;
            this.monitor = Monitor;
            this.addedOptions = false;
            this.helper.Events.Display.MenuChanged += Display_MenuChanged;
        }

        private void Display_MenuChanged(object sender, MenuChangedEventArgs e) {
            if (e.NewMenu is GameMenu) {
                GameMenu menu = e.NewMenu as GameMenu;
                addOptions(menu);
                this.monitor.Log($"Menu type is {menu.currentTab}");
            }
        }

        private void addOptions(GameMenu menu) {
            if (!addedOptions) {
                OptionsPage optionsPage = (OptionsPage) this.helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue()[6];
                IReflectedField<List<OptionsElement>> options = this.helper.Reflection.GetField<List<OptionsElement>>(optionsPage, "options");
                options.GetValue().Add(new OptionsElement("Mod Options"));
            }
        }
    }
}
