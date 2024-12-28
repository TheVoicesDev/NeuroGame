class_name DiveState

extends CharacterState

@export var dive_speed = 0.0
@export var jump_speed = 0.0

@export_group("References")
@export var fall_state : Node
@export var move_state : Node

func begin(_state : Node, _last_state : Node):
	if _state != self:
		return
	
	super(_state, _last_state)
	
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	var grav : GravityController = chara.GravityController
	
	grav.Disabled = false
	state_ctrl.PreventAutoSwitch.push_front(fall_state.name)
	var direction : Vector3 = chara.LookDirection
	var char_angle : Vector2 = Vector2(-direction.z, -direction.x)
	chara.global_rotation.y = char_angle.angle()
	chara.velocity = Vector3(direction.x * dive_speed, jump_speed, direction.z * dive_speed)

func allow_switch() -> bool:
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	var attempting_dive : bool = chara.has_meta(&"diving") and chara.get_meta(&"diving")

	return attempting_dive and not chara.is_on_floor()

func state_process(_delta : float):
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	
	if (chara.is_on_floor()):
		state_ctrl.SwitchState(move_state.name)

func end(_current_state : Node, _next_state : Node):
	if _current_state != self:
		return
	
	super(_current_state, _next_state)
	
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	state_ctrl.PreventAutoSwitch.erase(fall_state.name)
	
	if (_next_state is not MoveState):
		return
		
	var move : MoveState = _next_state
	var current_speed : float = Vector2(chara.velocity.x, chara.velocity.z).length()
	if (chara.MoveStrength != 0.0):
		move.speed = current_speed
	else:
		move.speed = current_speed / 2.0
