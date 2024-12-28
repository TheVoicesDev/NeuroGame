class_name DefaultAnimationProcessor

extends Node

@export var state_controller : StateController

func process_anim(anim_name : StringName, custom_blend : float, custom_speed : float, from_end : bool):
	var chara : Node = state_controller.Character
	if chara is not FantomeCharacter:
		return
	
	var body_animator : AnimationPlayer = chara.BodyAnimator
	
	if anim_name.is_empty():
		return
		
	var current_state : CharacterState = state_controller.CurrentState
	var anim : StringName = get_animation_name(current_state.animation_name, current_state.is_local_animation)
	body_animator.play(anim, custom_blend, custom_speed, from_end)

func get_animation_name(name : StringName, is_local_animation : bool) -> StringName:
	var current_state : CharacterState = state_controller.CurrentState
	return current_state.move_set.name + "/" + name if is_local_animation else name
