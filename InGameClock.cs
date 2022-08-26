using System;
using InGameClock.Client.Interface;
using InGameClock.Configuration;
using Intersect.Client.Framework.Content;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Graphics;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Plugins;
using Intersect.Client.General;
using System.Collections.Generic;
using System.IO;
using Intersect.Client.Core;

namespace InGameClock
{
    public class InGameClock
    {
        private readonly IClientPluginContext _mContext;
        private readonly string _mPluginDir;
        private GameRenderTexture _mRenderTexture;
        private ImagePanel _mInGameClock;
        private WindowControl _mInGameClockControl;
        private Label _inGameClockLabel;
        private readonly Dictionary<Keys, bool> _keyStatesChanged = new Dictionary<Keys, bool>();

        public InGameClock(IClientPluginContext context, string pluginDir)
        {
            _mContext = context;
            _mPluginDir = pluginDir;
        }

        public void Initialize()
        {
            LoadAssets(Path.Combine(Path.Combine(_mPluginDir, "InGameClock_GUI")));
            GenerateControls();
            _mInGameClockControl.LoadJsonUi(Path.Combine(_mPluginDir, "InGameClock_GUI", "InGameClock.json"));
            _mRenderTexture = GenerateBaseRenderTexture();
            _mInGameClock.Texture = _mRenderTexture;
            _mInGameClock.SetTextureRect(0, 0, _mRenderTexture.Width, _mRenderTexture.Height);
            Input.KeyDown += HandleKeyDown;
        }

        private void GenerateControls()
        {
            _mInGameClockControl =
                _mContext.Lifecycle.Interface.Create<WindowControl>(string.Empty, false, _mContext.Assembly.FullName);
            _mInGameClockControl.DisableResizing();
            _mInGameClockControl.Title = PluginSettings.Settings.ClockWindowsTitle;
            _mInGameClock = new ImagePanel(_mInGameClockControl, "InGameClockContainer");
            _inGameClockLabel = new Label(_mInGameClockControl, "InGameClockLabel");
        }

        private GameRenderTexture GenerateBaseRenderTexture() => _mContext.Graphics.CreateRenderTexture(128, 47);

        private void HandleKeyDown(Keys modifier, Keys key)
        {
            if (_keyStatesChanged.ContainsKey(key))
                return;
            _keyStatesChanged.Add(key, true);
        }

        public void Update()
        {
            _inGameClockLabel.SetText(
                new DateTime(DateTime.Parse(Time.GetTime()).Ticks).ToString(PluginSettings.Settings.ClockFormat));
            if (Globals.InputManager.KeyDown(PluginSettings.Settings.ClockToggleKeyHold))
            {
                if (_keyStatesChanged.TryGetValue(PluginSettings.Settings.ClockToggleKeyDown, out var clock))
                {
                    if (!clock)
                        return;
                    _mInGameClockControl.ToggleHidden();
                }
            }

            _keyStatesChanged.Clear();
        }

        private void LoadAssets(string resourceDir)
        {
            foreach (var enumerateFile in Directory.EnumerateFiles(Path.Combine(resourceDir)))
                _mContext.ContentManager.Load<GameTexture>((ContentTypes)6, enumerateFile,
                    Path.GetFileName(enumerateFile).ToLower());
        }
    }
}