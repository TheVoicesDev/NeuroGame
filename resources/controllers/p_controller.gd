class_name PlayerScheme

extends Node

@export var rotation_limit : Vector2 = Vector2(0, -90)
var camera : CameraController

func _ready():
	var game3D : FantomeGame = FantomeEngine.GetGame()
	camera = game3D.Camera
	
	# for now
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _physics_process(delta : float):
	var game3D : FantomeGame = FantomeEngine.GetGame()
	var player : FantomeCharacter = game3D.Player
	
	var state_ctrl : StateController = player.StateController
	var move_dir : Vector2 = Input.get_vector("adv_left", "adv_right", "adv_up", "adv_down")
	
	player.MoveStrength = move_dir.length()
	
	if player.MoveStrength != 0.0:
		player.LookDirection = camera.Camera.global_basis * Vector3(move_dir.x, 0, move_dir.y)
	
	player.set_meta(&"jumping", Input.is_action_pressed("adv_jump"))
	player.set_meta(&"diving", Input.is_action_just_pressed("adv_jump"))

### Handles mouse input
func _input(event: InputEvent) -> void:
	if (event is not InputEventMouseMotion):
		return
	
	var mouse_sensitivity : Vector2 = UserSettings.GetSetting("Gameplay/MouseSensitivity")
	var mouse_event : InputEventMouseMotion = event as InputEventMouseMotion
	var delta_x : float = mouse_event.relative.y * -mouse_sensitivity.y * 0.005
	var delta_y : float = mouse_event.relative.x * -mouse_sensitivity.x * 0.005
	
	var target_rot : Vector3 = camera.TargetRotation
	target_rot.x += delta_x#clampf(target_rot.x + delta_x, deg_to_rad(rotation_limit.x), deg_to_rad(rotation_limit.y))
	target_rot.y += delta_y
	
	camera.TargetRotation = target_rot
