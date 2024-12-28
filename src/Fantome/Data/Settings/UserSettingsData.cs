using Godot.Collections;
using Array = Godot.Collections.Array;

namespace Fantome.Data.Settings;

public partial class UserSettingsData
{
    public VideoSection Video = new();

    public GameplaySection Gameplay = new();

    public AudioSection Audio = new();

    public InputMapSection Bindings = new();

    /// <summary>
    /// Loads all valid settings from a <see cref="ConfigFile"/>.
    /// </summary>
    /// <param name="config">The config file to input</param>
    public partial void Load(ConfigFile config);

    /// <summary>
    /// Creates a new instance of <see cref="ConfigFile"/>, populated with the current settings.
    /// </summary>
    /// <returns></returns>
    public partial ConfigFile CreateConfigFileInstance();

    /// <summary>
    /// Gets a setting by key. More useful in GDScript than it is in C#.
    /// </summary>
    /// <param name="key">The key (case-sensitive)</param>
    /// <returns>The variant value if found, null if not.</returns>
    public partial Variant GetSetting(string key);

    /// <summary>
    /// Sets a setting by key. More useful in GDScript than it is in C#.
    /// </summary>
    /// <param name="key">The key (case-sensitive)</param>
    /// <param name="val">The variant value to set this setting to</param>
    public partial void SetSetting(string key, Variant val);
}

public class VideoSection
{
    public Window.ModeEnum Fullscreen = (Window.ModeEnum)ProjectSettings.GetSetting("display/window/size/mode").AsInt64();

    public Vector2I Resolution = new(ProjectSettings.GetSetting("display/window/size/window_width_override").AsInt32(), ProjectSettings.GetSetting("display/window/size/window_height_override").AsInt32());

    public DisplayServer.VSyncMode VSync = (DisplayServer.VSyncMode)ProjectSettings.GetSetting("display/window/vsync/vsync_mode").AsInt64();

    public int MaxFps = ProjectSettings.GetSetting("application/run/max_fps").AsInt32();

    public Settings3DSection Settings3D = new();

    public class Settings3DSection
    {
        public Viewport.Scaling3DModeEnum Scaling3DMode = (Viewport.Scaling3DModeEnum)ProjectSettings.GetSetting("rendering/scaling_3d/scale").AsInt64();

        public float RenderScale = ProjectSettings.GetSetting("rendering/scaling_3d/scale").AsSingle();

        public float FsrSharpness = ProjectSettings.GetSetting("rendering/scaling_3d/fsr_sharpness").AsSingle();
    }
}

public class GameplaySection
{
    public Vector2 MouseSensitivity = new Vector2(1f, 0.5f);
    public Vector2 GamepadSensitivity = new Vector2(1f, 1f);
}

public class AudioSection
{
    public float MasterVolume = 1.0f;
    public float MusicVolume = 1.0f;
    public float VocalsVolume = 1.0f;
    public float SfxVolume = 1.0f;
}

public class InputMapSection
{
    public Dictionary<string, Array<InputEvent>> Map = FantomeEngine.DefaultInputMap.Duplicate();
}