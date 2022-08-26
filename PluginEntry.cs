using InGameClock.Configuration;
using InGameClock.Logging;
using Intersect.Client.Plugins;
using Intersect.Client.Plugins.Interfaces;
using Intersect.Plugins;
using System.IO;
using Intersect.Client.General;
using Microsoft;

namespace InGameClock
{
  public class PluginEntry : ClientPluginEntry
  {
    private InGameClock _mInGameClock;
    private bool _mInitialized;

    public override void OnBootstrap([ValidatedNotNull] IPluginBootstrapContext context)
    {
      PluginSettings.Settings = context.GetTypedConfiguration<PluginSettings>();
      Logger.Context = context;
      Logger.Write(LogLevel.Info, $"[PLUGIN NAME] {context.Manifest.Name}");
      Logger.Write(LogLevel.Info, $"[VERSION] {context.Manifest.Version}");
      Logger.Write(LogLevel.Info, $"[BUILD DATE] 25-08-2022");
      Logger.Write(LogLevel.Info, "[AUTHOR] Arufonsu");
      Logger.Write(LogLevel.Info, $"[GITHUB] {context.Manifest.Homepage}");
    }

    public override void OnStart([ValidatedNotNull] IClientPluginContext context)
    {
      _mInGameClock = new InGameClock(context, Path.GetDirectoryName(context.Assembly.Location));
      Logger.Write(LogLevel.Info, "InGameClock Plugin has been successfully loaded by the game client!");
      context.Lifecycle.LifecycleChangeState += HandleLifecycleChangeState;
      context.Lifecycle.GameUpdate += HandleGameUpdate;
    }

    private void HandleLifecycleChangeState(
      [ValidatedNotNull] IClientPluginContext context,
      [ValidatedNotNull] LifecycleChangeStateArgs lifecycleChangeStateArgs)
    {
      if (context.Lifecycle.Interface == null || lifecycleChangeStateArgs.State != (GameStates)3 || !_mInitialized)
        return;
      _mInitialized = false;
    }

    private void HandleGameUpdate([ValidatedNotNull] IClientPluginContext context, [ValidatedNotNull] GameUpdateArgs gameUpdateArgs)
    {
      if (gameUpdateArgs.State == (GameStates)3)
      {
        if (!_mInitialized)
        {
          _mInGameClock.Initialize();
          _mInitialized = true;
        }
        _mInGameClock.Update();
      }
      else
      {
        if (!_mInitialized)
          return;
        _mInitialized = false;
      }
    }

    public override void OnStop([ValidatedNotNull] IClientPluginContext context)
    {
    }
  }
}
