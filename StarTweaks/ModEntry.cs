using StardewModdingAPI;

namespace StarTweaks {
    public class ModEntry : Mod {

        private Options.Handler optionsHandler;
        public static IModHelper helper;
        public static IMonitor monitor;

        public override void Entry(IModHelper Helper) {
            ModEntry.monitor = Monitor;
            ModEntry.helper = Helper;
            this.optionsHandler = new Options.Handler();
        }
    }
}
