using Intersect.Plugins;

namespace InGameClock.Logging
{
    public static class Logger
    {
        public static IPluginBaseContext Context { get; set; }

        public static void Write(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Info:
                    Context.Logging.Plugin.Info(message);
                    break;
                case LogLevel.Warning:
                    Context.Logging.Plugin.Warn(message);
                    break;
                case LogLevel.Error:
                    Context.Logging.Plugin.Error(message);
                    break;
            }
        }
    }
    
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
    }
}