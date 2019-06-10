using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace StarTweaks {
    public class ModEntry : Mod {

        private OptionsMenuExtension extension;

        public override void Entry(IModHelper Helper) {
            extension = new OptionsMenuExtension(Helper, this.Monitor);
        }
    }
}
