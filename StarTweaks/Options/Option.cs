using StardewModdingAPI;

namespace StarTweaks.Options {

    /// <summary>
    /// Class to represent a mod option.
    /// </summary>
    public class Option {

        private string label;
        private bool isActive;

        public string Label { get { return label; } }
        public bool IsActive { get { return isActive; } }

        /// <summary>
        /// Initalises the option.
        /// </summary>
        /// <param name="label">This is the string that will appear in the options menu.</param>
        /// <param name="defaultbool">Whether on first creation the option is set to true or false.</param>
        public Option(string label, bool defaultbool = true) {
            this.label = label;
            this.isActive = defaultbool;
        }

        /// <summary>
        /// This is used to toggle the option.
        /// </summary>
        public void Toggle() {
            this.isActive = !this.isActive;
        }
    }
}
