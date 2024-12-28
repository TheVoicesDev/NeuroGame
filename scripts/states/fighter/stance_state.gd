class_name StanceState

extends CharacterState

@export var time_till_idle : float = 3.0
@export var idle_state_name : StringName = &"Idle"
var idle_time : float = 0.0

func begin(_state : Node, _last_state : Node):
	if _state != self:
		return
	
	active = true
	time_passed = 0.0
	idle_time = 0.0
	
	var state_ctrl : StateController = move_set.Controller
	var return_from_fighter : bool = _last_state is FighterState
	if return_from_fighter:
		var body_anim : AnimationPlayer = state_ctrl.Character.BodyAnimator
		body_anim.stop()
		body_anim.seek(0.0, true)
	
	state_ctrl.PlayAnimation(animation_name, -1.0 if not return_from_fighter else 0.0, 1.0, false)

func state_process(_delta : float):
	if (idle_time >= time_till_idle):
		var ctrl : StateController = move_set.Controller
		ctrl.SwitchState(idle_state_name)
		return
	
	idle_time += _delta
	
func end(_current_state : Node, _next_state : Node):
	if _current_state != self:
		return
		
	idle_time = 0.0
	super(_current_state, _next_state)
