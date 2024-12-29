class_name DeathState

extends CharacterState

func begin(_state : Node, _last_state : Node) -> void:
	if _state != self:
		return
	
	super(_state, _last_state)
	
	var ctrl : StateController = move_set.Controller
	ctrl.ManualSwitching = true

func allow_switch() -> bool:
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	
	var prevent_auto_switch : bool = state_ctrl.PreventAutoSwitch.has(name)
	return not prevent_auto_switch and chara.Health <= 0

func end(_current_state : Node, _next_state : Node):
	if _current_state != self:
		return
		
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	var skele : Skeleton3D = chara.SkeletonReference
		
	super(_current_state, _next_state)
