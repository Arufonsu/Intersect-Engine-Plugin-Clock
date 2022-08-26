using Intersect.Plugins;
using System.Runtime.Serialization;
using Intersect.Client.Framework.GenericClasses;

namespace InGameClock.Configuration
{
    public class PluginSettings : PluginConfiguration
    {
        public static PluginSettings Settings { get; set; }

        public string ClockWindowsTitle { get; set; } = "Clock";

        public string ClockFormat { get; set; } = "H:mm";

        public Keys ClockToggleKeyHold { get; set; } = Keys.Control;

        public Keys ClockToggleKeyDown { get; set; } = Keys.O;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
        }
    }
}