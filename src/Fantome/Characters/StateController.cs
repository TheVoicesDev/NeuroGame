using System.Linq;
using Godot.Collections;

namespace Fantome.Characters;

/// <summary>
/// A node that controls multiple movesets.
/// </summary>
[GlobalClass]
public partial class StateController : Node
{
    /// <summary>
    /// Keeps track of what not to automatically switch to. Switching is handled via the individual states.
    /// </summary>
    [Export] public Array<StringName> PreventAutoSwitch = [];

    /// <summary>
    /// If true, states have to be manually switched to instead of them doing it themselves automatically.
    /// </summary>
    [Export] public bool ManualSwitching = false;

    /// <summary>
    /// The character linked to this state controller.
    /// </summary>
    [Export] public FantomeCharacter Character;

    /// <summary>
    /// The move set list.
    /// </summary>
    [Export] public Array<MoveSet> MoveSets;

    [ExportGroup("Status"), Export] public Node CurrentState;

    [Signal] public delegate void ProcessAnimationEventHandler(StringName animName, double customBlend, float customSpeed, bool fromEnd);
    
    [Signal] public delegate void HealedEventHandler(int healthGain, FantomeCharacter instigator);
    
    [Signal] public delegate void DamagedEventHandler(int healthLoss, FantomeCharacter instigator);
    
    private Dictionary<StringName, Node> _states = [];
    private Array<Node> _overriddenStates = [];

    public override void _Ready()
    {
        base._Ready();

        Character = GetParent() as FantomeCharacter;
        for (int i = 0; i < MoveSets.Count; i++)
            SetupMoveSet(MoveSets[i]);
        
        for (int i = 0; i < GetChildCount(); i++)
        {
            if (GetChild(i) is not MoveSet moveSet || MoveSets.Contains(moveSet))
                continue;

            AddMoveSet(moveSet);
        }

        Update();
        if (MoveSets.Count > 0)
            SwitchState(MoveSets[^1].DefaultState.Name);
    }
    
    public void PlayAnimation(StringName animName, double customBlend = -1d, float customSpeed = -1f, bool fromEnd = false)
    {
        EmitSignal(SignalName.ProcessAnimation, animName, customBlend, customSpeed, fromEnd);
    }

    public void CallDeath()
    {
        SwitchState(MoveSets.Last(x => x.DeathState != null).DeathState.Name);
    }

    /// <summary>
    /// Gets the <see cref="Node"/> that matches the name of the string provided.
    /// Movesets with a higher index will be prioritized.
    /// </summary>
    /// <param name="name">The name provided</param>
    /// <returns>The moveset</returns>
    public Node GetState(string name) => _states[name];

    /// <summary>
    /// Get all states available to be switched to.
    /// </summary>
    /// <returns>An array of states</returns>
    public Node[] GetStates() => _states.Values.ToArray();

    /// <summary>
    /// Get of the states' names that are available to be switched to.
    /// </summary>
    /// <returns>An array of state names</returns>
    public StringName[] GetStateNames() => _states.Keys.ToArray();

    public Node[] GetOverriddenStates() => _overriddenStates.ToArray();

    /// <summary>
    /// Switches to the current state matching the name provided.
    /// Movesets with a higher index will be prioritized.
    /// </summary>
    /// <param name="name">The state to switch to</param>
    public void SwitchState(string name)
    {
        Node state = GetState(name);
        if (state == null)
        {
            GD.PrintErr($"State {name} not found for character \"{Character.Name}\".");
            return;
        }

        for (int i = 0; i < MoveSets.Count; i++)
            MoveSets[i].EmitSignal(MoveSet.SignalName.EndState, CurrentState, state);

        Node lastState = CurrentState;
        CurrentState = state;

        for (int i = 0; i < MoveSets.Count; i++)
            MoveSets[i].EmitSignal(MoveSet.SignalName.BeginState, CurrentState, lastState);
    }

    /// <summary>
    /// Adds a moveset to the state controller.
    /// </summary>
    /// <param name="moveSet">The moveset</param>
    public void AddMoveSet(MoveSet moveSet)
    {
        if (moveSet.GetParent() != this)
            AddChild(moveSet);

        SetupMoveSet(moveSet);
        MoveSets.Add(moveSet);

        Update();
    }

    /// <summary>
    /// Inserts a moveset into the state controller with the provided index.
    /// </summary>
    /// <param name="moveSet">The moveset</param>
    /// <param name="idx">The index</param>
    public void InsertMoveSet(MoveSet moveSet, int idx)
    {
        if (moveSet.GetParent() != this)
        {
            AddChild(moveSet);
            MoveChild(moveSet, idx);
        }

        moveSet.Controller = this;
        Character.BodyAnimator.AddAnimationLibrary(moveSet.Name, moveSet.Animations);
        MoveSets.Insert(idx, moveSet);

        Update();
    }

    /// <summary>
    /// Removes a moveset from the state controller.
    /// </summary>
    /// <param name="moveSet">The moveset to remove.</param>
    /// /// <param name="free">If true, frees the moveset provided.</param>
    public void RemoveMoveSet(MoveSet moveSet, bool free = false)
    {
        RemoveChild(moveSet);
        moveSet.Controller = null;
        Character.BodyAnimator.RemoveAnimationLibrary(moveSet.Name);
        MoveSets.Remove(moveSet);

        if (free)
            moveSet.QueueFree();

        Update();
    }

    /// <summary>
    /// Clears all the movesets and replaces the first moveset with the one provided.
    /// </summary>
    /// <param name="moveSet">The moveset to reset to. If none is provided, it will use the first one in the list.</param>
    /// <param name="free">If true, frees the movesets that are removed.</param>
    public void ResetMoveSet(MoveSet moveSet, bool free = false)
    {
        for (int i = 0; i < MoveSets.Count; i++)
        {
            if (MoveSets[i] == moveSet)
                continue;

            RemoveChild(MoveSets[i]);
            Character.BodyAnimator.RemoveAnimationLibrary(moveSet.Name);

            if (free)
                MoveSets[i].QueueFree();
        }

        MoveSets.Clear();
        AddMoveSet(moveSet);
    }

    /// <summary>
    /// Gets a moveset with the name provided.
    /// </summary>
    /// <param name="name">The name</param>
    /// <returns>A moveset that matches the name provided, or null if there is none.</returns>
    public MoveSet GetMoveSet(string name) => MoveSets.FirstOrDefault(x => x.Name == name);

    /// <summary>
    /// Gets a moveset at the index provided.
    /// </summary>
    /// <param name="idx">The index</param>
    /// <returns>The moveset</returns>
    public MoveSet GetMoveSetAt(int idx) => MoveSets[idx];

    /// <summary>
    /// Gets the index of the moveset provided.
    /// </summary>
    /// <param name="moveSet">The moveset to check for</param>
    /// <returns>The index of the moveset.</returns>
    public int GetMoveSetIndex(MoveSet moveSet) => MoveSets.IndexOf(moveSet);

    /// <summary>
    /// Removes the moveset at the index provided.
    /// </summary>
    /// <param name="idx">The index of the moveset.</param>
    /// <param name="free">If true, frees the moveset found.</param>
    public void RemoveMoveSetAt(int idx, bool free = false) => RemoveMoveSet(MoveSets[idx], free);

    private void Update()
    {
        _states.Clear();
        _overriddenStates.Clear();
        
        for (int i = MoveSets.Count - 1; i >= 0; i--)
        {
            MoveSet moveSet = MoveSets[i];
            for (int j = 0; j < moveSet.GetChildCount(); j++)
            {
                Node state = moveSet.GetChild(j);
                if (_states.Keys.Contains(state.Name))
                {
                    _overriddenStates.Add(state);
                    continue;
                }

                _states.Add(state.Name, state);
            }
        }
    }

    private void SetupMoveSet(MoveSet moveSet)
    {
        moveSet.Controller = this;
        Character.BodyAnimator.AddAnimationLibrary(moveSet.Name, moveSet.Animations);
    }
}