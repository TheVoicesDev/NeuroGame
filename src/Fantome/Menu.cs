namespace Fantome;

[GlobalClass]
public partial class Menu : Node
{
    [ExportGroup("Status"), Export] public int Selection = 0;
		
    [ExportGroup("Settings"), Export] public bool AllowScrollWheel = true;
    [Export] public bool AllowEcho = true;
		
    [ExportGroup("References"), Export] public AudioStream MoveCursor;
    [Export] public AudioStream ConfirmAudio;
    [Export] public AudioStream BackAudio;

    [Signal] public delegate void DownEventHandler(bool pressed);
    [Signal] public delegate void UpEventHandler(bool pressed);
    [Signal] public delegate void LeftEventHandler(bool pressed);
    [Signal] public delegate void RightEventHandler(bool pressed);
    [Signal] public delegate void ConfirmEventHandler(bool pressed);
    [Signal] public delegate void BackEventHandler(bool pressed);
    [Signal] public delegate void ScrollEventHandler(float direction);
    
    public override void _Input(InputEvent @event)
    {
	    base._Input(@event);

	    if (@event.IsAction("ui_down", AllowEcho))
		    EmitSignalDown(@event.IsPressed());
	    else if (@event.IsAction("ui_up", AllowEcho)) 
		    EmitSignalUp(@event.IsPressed());
	    else if (@event.IsAction("ui_left", AllowEcho))
		    EmitSignalLeft(@event.IsPressed());
	    else if (@event.IsAction("ui_right", AllowEcho))
		    EmitSignalRight(@event.IsPressed());
	    else if (@event.IsAction("ui_accept"))
		    EmitSignalConfirm(@event.IsPressed());
	    else if (@event.IsAction("ui_cancel", AllowEcho))
		    EmitSignalBack(@event.IsPressed());

	    if (!AllowScrollWheel || @event is not InputEventMouseButton mouseEvent || (mouseEvent.ButtonIndex != MouseButton.WheelDown && mouseEvent.ButtonIndex != MouseButton.WheelUp))
		    return;
	    
	    EmitSignalScroll(mouseEvent.Factor);
    }
}