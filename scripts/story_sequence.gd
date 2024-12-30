extends Control

@export var sequence : NeuroStorySequence
@export var next_scene : String

@export_group("Debug")
@export var index : int = -1
@export var pic_index : int = 0
@export var current : NeuroSequenceData
@export var time_passed : float = 0

@export_group("References")
@export var image : TextureRect
@export var next_button : Button

var transitioning : bool = false

func _ready() -> void:
	advance()
	
	Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	AudioManager.PlayMusic(sequence.Music)
	AudioManager.Music.volume_db = linear_to_db(0.6)

func _process(delta: float) -> void:
	if Input.is_key_pressed(KEY_SPACE):
		index = sequence.Data.size() - 1
		advance()
		return
	
	time_passed += delta
	
	if time_passed > current.FrameRate:
		advance_image()
		time_passed = 0

func advance() -> void:
	if index >= sequence.Data.size() - 1:
		if not transitioning:
			TransitionManager.SwitchMainScene("neuro_transition", next_scene)
		
		transitioning = true
		return
	
	index += 1
	current = sequence.Data[index]
	
	time_passed = 0
	pic_index = 0
	image.texture = current.Images[pic_index]

func advance_image() -> void:
	if pic_index >= current.Images.size() - 1 and not current.Loop:
		return
	
	pic_index = (pic_index + 1) % current.Images.size()
	image.texture = current.Images[pic_index]
