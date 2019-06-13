using StardewModdingAPI;

namespace StarTweaks.Options {

    public class Option {
        //private string stringid;
        private string label;
        private bool isActive;

        public Option(string label, bool defaultbool = true) {
            //this.stringid = stringid;
            this.label = label;
            this.isActive = defaultbool;
        }

        public string GetLabel() {
            return this.label;
        }

        public bool GetActive() {
            return this.isActive;
        }

        public void Toggle() {
            this.isActive = !this.isActive;
        }
    }
}
