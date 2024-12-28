class_name JumpState3D

extends MoveState

@export var jump_speed : float = 4.0
@export var seconds_till_fall : float = 0.5
@export var fall_state : Node

var jump_time_passed : float = 0.0

func begin(_state : Node, _last_state : Node):
	if _state != self:
		return
	
	super(_state, _last_state)
	
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	var grav : GravityController = chara.GravityController

	ctrl.PreventAutoSwitch.push_front(fall_state.name)
	
func allow_switch() -> bool:
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	
	return get_jump_state() and chara.is_on_floor()

func state_process(_delta : float):
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	
	if (jump_time_passed >= seconds_till_fall or not get_jump_state()):
		state_ctrl.SwitchState(fall_state.name)
		return
		
	chara.velocity.y = jump_speed
	jump_time_passed += _delta
	super(_delta)
	
func end(_current_state : Node, _next_state : Node):
	if _current_state != self:
		return

	var ctrl : StateController = move_set.Controller
	ctrl.PreventAutoSwitch.erase(fall_state.name)
	
	jump_time_passed = 0.0
	if _next_state is FallState:
		(_next_state as FallState).already_jumped = true
	
	super(_current_state, _next_state)

func get_jump_state() -> bool:
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	return chara.has_meta(&"jumping") and chara.get_meta(&"jumping")
