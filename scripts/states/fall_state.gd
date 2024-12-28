class_name FallState

extends MoveState

@export var coyote_timer : float = 0.1
@export var jump_state : Node
@export var move_state : Node

@export_group("Status")
@export var already_jumped : bool = false
@export var coyote_time_passed : float = 0.0
	
func play_animation() -> void:
	pass

func allow_switch() -> bool:
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	return not chara.is_on_floor()

func state_process(_delta : float):
	super(_delta)
	
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	var body_player : AnimationPlayer = chara.BodyAnimator
	
	var attempting_jump : bool = chara.has_meta(&"jumping") and chara.get_meta(&"jumping")
	if (attempting_jump and ((coyote_time_passed <= coyote_timer and not already_jumped) or chara.is_on_floor())):
		state_ctrl.SwitchState(jump_state.name)
		return
		
	coyote_time_passed += _delta
	
	if (chara.velocity.y < 0 and body_player.current_animation != get_animation_name(animation_name)):
		state_ctrl.PlayAnimation(animation_name, -1.0, 1.0, false)

	if (not chara.is_on_floor()):
		return
		
	if (chara.velocity.x != 0.0 and chara.velocity.z != 0.0):
		state_ctrl.SwitchState(move_state.name)
		return
	
	var default_state : Node = move_set.DefaultState
	state_ctrl.SwitchState(default_state.name)

func end(_current_state : Node, _next_state : Node):
	if _current_state != self:
		return
	
	coyote_time_passed = 0.0
	already_jumped = false
	
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	if (chara.MoveStrength == 0.0 and speed != 0.0):
		speed /= 2.0
		
	super(_current_state, _next_state)


func get_animation_name(name : StringName) -> StringName:
	return move_set.name + "/" + name if is_local_animation else animation_name
