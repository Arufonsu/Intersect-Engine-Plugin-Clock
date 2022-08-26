using Intersect.Client.Framework.Gwen.Control;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using InGameClock.Logging;

namespace InGameClock.Client.Interface
{
    public static class BaseExtensions
    {
        public static void LoadJsonUi(this Base control, string filePath, bool saveOutput = true)
        {
            try
            {
                var jobject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(filePath));
                if (jobject != null)
                {
                    control.LoadJson(jobject);
                    control.ProcessAlignments();
                }
                if (jobject == null)
                    saveOutput = false;
            }
            catch (Exception ex)
            {
                if (Logger.Context != null)
                    Logger.Write(LogLevel.Error, ex.Message);
            }
            if (!saveOutput)
                return;
            control.SaveControlLayout(filePath);
        }

        private static void SaveControlLayout(this Base control, string filePath) => File.WriteAllText(filePath, control.GetJsonUI());
    }
}