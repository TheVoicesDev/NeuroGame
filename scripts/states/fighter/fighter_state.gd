class_name FighterState

extends CharacterState

@export var idle_state : CharacterState
@export var retain_ending_position : bool = true

func begin(_state : Node, _last_state : Node):
	if _state != self:
		return
	
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	
	chara.velocity.x = 0.0
	chara.velocity.z = 0.0
	
	super(_state, _last_state)

func state_process(_delta : float):
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	var body_anim : AnimationPlayer = chara.BodyAnimator
	
	chara.quaternion = chara.quaternion * body_anim.get_root_motion_rotation()
	var cur_velocity : Vector3 = (body_anim.get_root_motion_rotation_accumulator().inverse() * chara.quaternion) * body_anim.get_root_motion_position() / _delta
	chara.velocity = cur_velocity
	
	var cur_time : float = body_anim.current_animation_position
	if body_anim.current_animation_position < body_anim.current_animation_length:
		return
	
	ctrl.SwitchState(idle_state.name)

func end(_current_state : Node, _next_state : Node):
	if _current_state != self:
		return
		
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	var hit_box : HitBox = chara.HitBox
	hit_box.Reset()
		
	super(_current_state, _next_state)

func set_hit_instigating(name : StringName, value : bool):
	var ctrl : StateController = move_set.Controller
	var chara : FantomeCharacter = ctrl.Character
	var hit_box : HitBox = chara.HitBox
	
	var idx : int = hit_box.FindPart(name)
	var part : HitArea = hit_box.GetPart(idx)
	part.Instigating = value
