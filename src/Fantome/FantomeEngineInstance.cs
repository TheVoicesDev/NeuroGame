global using Godot;
global using System;
using Fantome.Utilities;
using Godot.Collections;

namespace Fantome;

/// <summary>
/// The main class for Fantome Engine.
/// </summary>
[GlobalClass, StaticAutoloadSingleton("Fantome", "FantomeEngine")]
public partial class FantomeEngineInstance : Node
{
    /// <summary>
    /// The current iteration of Fantome Engine at the moment.
    /// </summary>
    public static VersionInfo Version = new(0, 1, 0, 0, "-doki");

    /// <inheritdoc cref="GetGame"/>
    public FantomeGame Game => GetGame();

    /// <summary>
    /// The default input map the game was launched with.
    /// </summary>
    public Dictionary<string, Array<InputEvent>> DefaultInputMap = new();

    public override void _Ready()
    {
        GetWindow().ContentScaleSize = ProjectSettings.GetSetting("fantome/viewport/content_minimum_size").AsVector2I();

        Array<StringName> actionNames = InputMap.GetActions();
        for (int i = 0; i < actionNames.Count; i++)
        {
            string actionName = actionNames[i];
            DefaultInputMap[actionName] = InputMap.ActionGetEvents(actionName);
        }

        GD.Print("Fantome Engine singleton initialized! Version: " + Version);
    }

    /// <summary>
    /// If the current scene is an instance of <see cref="FantomeGame"/>, return that instance as that type.
    /// </summary>
    /// <returns><see cref="FantomeGame"/> if it is the current scene, else null.</returns>
    public FantomeGame GetGame()
    {
        Node currentScene = GetTree().CurrentScene;
        if (currentScene is FantomeGame game)
            return game;

        return null;
    }
}