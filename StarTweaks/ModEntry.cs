using StardewModdingAPI;

namespace StarTweaks {
    /// <summary>
    /// The mod entry point.
    /// Contains methods for setting up the mod.
    /// </summary>
    public class ModEntry : Mod {

        private Options.Handler optionsHandler;
        private UI.CustomToolbar customToolbar;
        private UI.CustomSort customSort;

        /// <summary> 
        /// This provides an easy access reference to the SMAPI Helper object.
        /// This is setup to be a reference to the inherited instance Helper from the <c>SMAPI Mod</c> class.
        /// </summary>
        public new static IModHelper Helper;

        /// <summary> 
        /// This provides an easy access reference to the SMAPI Monitor object.
        /// This is setup to be a reference to the inherited instance Monitor from the <c>SMAPI Mod</c> class.
        /// </summary>
        public new static IMonitor Monitor;

        /// <summary>
        /// Sets up all objects required by the mod.
        /// </summary>
        /// 
        /// <param name="Helper">This is the Helper provided by the SMAPI Initalisation.</param>
        public override void Entry(IModHelper Helper) {
            ModEntry.Monitor = base.Monitor;
            ModEntry.Helper = Helper;

            this.optionsHandler = new Options.Handler();
            this.customToolbar = new UI.CustomToolbar(optionsHandler.OptionDictionary[Options.Keys.ToggleToolbarSwitch]);
            this.customSort = new UI.CustomSort(optionsHandler.OptionDictionary[Options.Keys.ToggleCustomSort]);
        }
    }
}
