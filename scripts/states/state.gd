class_name CharacterState

extends Node

@export var animation_name : StringName = &"idle"
@export var is_local_animation : bool = true
@export var time_until_switch : float = 0.05

@export_group("Status")
@export var active : bool = false

var move_set : MoveSet
var time_passed : float = 0.0

func _ready() -> void:
	move_set = get_parent()
	move_set.BeginState.connect(begin)
	move_set.EndState.connect(end)

func begin(_state : Node, _last_state : Node) -> void:
	if _state != self:
		return
	
	active = true
	time_passed = 0.0
	
	var state_ctrl : StateController = move_set.Controller
	state_ctrl.PlayAnimation(animation_name, -1.0, 1.0, false)
	
func allow_switch() -> bool:
	return false
	
func _physics_process(_delta : float) -> void:
	if (active):
		state_process(_delta)
		return
		
	move_set = get_parent()
	var state_ctrl : StateController = move_set.Controller
	var overridden : Array = state_ctrl.GetOverriddenStates()
	if (overridden.has(self) 
		or move_set == null 
		or state_ctrl.ManualSwitching
		or state_ctrl.PreventAutoSwitch.has(name) 
		or not allow_switch()
	):	
		time_passed = 0.0
		return
		
	time_passed += _delta
	if (time_passed > time_until_switch):
		state_ctrl.SwitchState(name)

func state_process(_delta : float):
	pass

func end(_current_state : Node, _next_state : Node):
	if _current_state != self:
		return
	
	active = false
	time_passed = 0.0
