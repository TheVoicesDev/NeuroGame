class_name SlowState3D

extends MoveState

@export var match_speed_with_anim : bool = false

func begin(_state : Node, _last_state : Node):
	if _state != self:
		return
		
	super(_state, _last_state)

func allow_switch() -> bool:
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	
	var no_move_strength : bool = chara.MoveStrength == 0.0
	var is_moving : bool = chara.velocity.x != 0.0 or chara.velocity.z != 0.0
	return no_move_strength and is_moving and chara.is_on_floor()

func state_process(_delta : float):
	speed = maxf(speed - speed_decrement, 0.0)
	
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	var body_anim : AnimationPlayer = chara.BodyAnimator
	
	if match_speed_with_anim:
		body_anim.speed_scale = speed / max_speed
	
	chara.velocity = Vector3(movement_angle.x * speed, chara.velocity.y, movement_angle.z * speed)
	
	var cur_anim : String = body_anim.current_animation
	var is_looped_anim : bool = body_anim.has_animation(cur_anim) and body_anim.get_animation(body_anim.current_animation).loop_mode != 0
	var finished_anim : bool = body_anim.current_animation_position > body_anim.current_animation_length
	if speed <= 0.0 and (finished_anim or is_looped_anim):
		var default_state : Node = move_set.DefaultState
		state_ctrl.SwitchState(default_state.name)

func end(_current_state : Node, _next_state : Node):
	if _current_state != self:
		return
		
	var state_ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = state_ctrl.Character
	var body_anim : AnimationPlayer = chara.BodyAnimator
	body_anim.speed_scale = 1.0
	
	super(_current_state, _next_state)
