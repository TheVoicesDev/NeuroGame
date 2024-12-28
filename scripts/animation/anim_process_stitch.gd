class_name StitchAnimationProcessor

extends DefaultAnimationProcessor

@export var left_suffix : StringName = &"-left"
@export var right_suffix : StringName = &"-right"
@export var rotation_weight : float = 2.0

var current_anim : StringName
var is_local_animation : bool

var sprite : VisualInstance3D
var current_pos : float = 0.0
var last_dir : float = 1.0

func _ready():
	var chara : FantomeCharacter = state_controller.Character
	sprite = chara.VisualReference
	sprite.rotation_degrees.y = 90
	
func _physics_process(delta):
	var chara : FantomeCharacter = state_controller.Character
	var direction : float = signf(chara.LookDirection.x)
	last_dir = direction if direction != 0.0 else last_dir
	
	current_pos = lerpf(current_pos, last_dir, rotation_weight) if not is_equal_approx(current_pos, direction) else direction
	
	var target_rot : float = -90.0 if last_dir < 0 else 90.0
	var mult : float = -absf(current_pos) + 1.0
	var current_rot : float = target_rot * mult
	sprite.rotation_degrees.y = current_rot
	
	if current_anim.is_empty():
		return
	
	var body_anim : AnimationPlayer = chara.BodyAnimator
	var anim : StringName = get_animation_name(current_anim, is_local_animation)
	var previous_time : float = body_anim.current_animation_position if body_anim.is_playing() else 0.0
	
	if (not has_directional_anims()):
		body_anim.play(anim)
		return
	
	var left_anim : StringName = anim + left_suffix
	var right_anim : StringName = anim + right_suffix
	var left_going_right : bool = current_pos > 0 and (not body_anim.is_playing() or body_anim.current_animation != right_anim)
	var right_going_left : bool = current_pos < 0 and (not body_anim.is_playing() or body_anim.current_animation != left_anim)
	if (left_going_right):
		body_anim.play(right_anim)
		body_anim.seek(previous_time)
		
	if (right_going_left):
		body_anim.play(left_anim)
		body_anim.seek(previous_time)

func process_anim(anim_name : StringName, custom_blend : float, custom_speed : float, from_end : bool):
	current_anim = anim_name
	
	var current_state : CharacterState = state_controller.CurrentState
	is_local_animation = current_state.is_local_animation

func has_directional_anims() -> bool:
	var retrieved_character : Node = state_controller.Character
	var body_anim : AnimationPlayer = null
	var x_direction : float = 0.0
	if retrieved_character is FantomeCharacter:
		var chara3D : FantomeCharacter = retrieved_character
		body_anim = chara3D.BodyAnimator
		x_direction = chara3D.LookDirection.x
	
	var anim : StringName = get_animation_name(current_anim, is_local_animation)
	var left_anim : StringName = anim + left_suffix
	var right_anim : StringName = anim + right_suffix
	return body_anim != null and body_anim.has_animation(left_anim) and body_anim.has_animation(right_anim)
