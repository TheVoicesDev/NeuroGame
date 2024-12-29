extends Area3D

@export var character : DokiCharacter
@export var distance_threshold : float = 3.0

var player : DokiCharacter
var is_intersecting : bool = false
var timer : Timer = Timer.new()

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	player = FantomeEngine.GetGame().Player
	
	body_entered.connect(on_body_entered)
	body_exited.connect(on_body_exited)
	
	timer.timeout.connect(on_timer_done)
	add_child(timer)

func _physics_process(delta: float) -> void:
	if character.Health <= 0:
		character.MoveStrength = 0
		character.AttemptCombo = false
		timer.stop()
		return
	
	if not is_intersecting:
		character.MoveStrength = 0
		return
	
	character.LookDirection = character.global_position.direction_to(player.global_position)
	character.MoveStrength = 1
	
	var distance : float = character.global_position.distance_to(player.global_position)
	if distance > distance_threshold:
		timer.stop()
		character.AttemptCombo = false
		return
	
	if timer.is_stopped():
		character.AttemptCombo = false
		timer.start(0.45)

func on_timer_done() -> void:
	character.AttemptCombo = true

func on_body_entered(body : Node3D):
	if body != player:
		return
	
	is_intersecting = true

func on_body_exited(body : Node3D):
	if body != player:
		return
	
	is_intersecting = false
