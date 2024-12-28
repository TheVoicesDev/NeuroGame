class_name MoveState

extends CharacterState

@export var max_speed : float = 12.0
@export var speed_increment : float = 0.025
@export var speed_decrement : float = 0.0125
@export var rotation_weight : float = 0.1
@export var strength_threshold : Vector2 = Vector2(0, 1)

@export_group("Status")
@export var speed : float = 0.0
@export var movement_angle : Vector3 = Vector3.ZERO

@export_group("Settings")
@export var cap_max_speed : bool = false
@export var use_character_rotation : bool = true
	
func begin(_state : Node, _last_state : Node):
	if _state != self:
		return
	
	super(_state, _last_state)
	
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	var grav : GravityController = chara.GravityController
		
	movement_angle = (chara.global_basis * Vector3.FORWARD).normalized() if use_character_rotation else chara.LookDirection
	grav.Disabled = false

func allow_switch() -> bool:
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	
	var prevent_auto_switch : bool = state_ctrl.PreventAutoSwitch.has(name)
	var meets_threshold : bool = chara.MoveStrength > strength_threshold.x and chara.MoveStrength <= strength_threshold.y
	
	return not prevent_auto_switch and meets_threshold and chara.is_on_floor()
	
func state_process(_delta : float):
	var state_ctrl : StateController = move_set.Controller
	var character : FantomeCharacter = state_ctrl.Character
	if character.MoveStrength == 0.0:
		return
	
	var speed_inc : float = speed_increment
	var predicted_speed : float = speed + speed_inc
	if predicted_speed > max_speed:
		speed_inc = max(0, speed_inc - fmod(predicted_speed, max_speed) - (floorf(predicted_speed / max_speed) * max_speed))
		
	speed += speed_inc
	
	if cap_max_speed and speed > max_speed:
		speed = maxf(speed - speed_decrement, max_speed)
		
	movement_angle = movement_angle.slerp(character.LookDirection, rotation_weight)
	
	var char_angle : Vector2 = Vector2(-movement_angle.z, -movement_angle.x)
	
	if use_character_rotation:
		character.global_rotation.y = char_angle.angle()
	
	character.velocity = Vector3(movement_angle.x * speed, character.velocity.y, movement_angle.z * speed)

func end(_current_state : Node, _next_state : Node):
	if _current_state != self:
		return
	
	if (_next_state is MoveState):
		(_next_state as MoveState).speed = speed
			
	speed = 0.0
	super(_current_state, _next_state)
