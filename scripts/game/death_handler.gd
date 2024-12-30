extends Node

var game : NeuroGame
var player : DokiCharacter

var is_transitioning : bool = false

func  _ready() -> void:
	game = get_parent()
	player = game.Player
	player.StateController.Damaged.connect(on_damaged)

func on_damaged(health_loss : int, instigator : FantomeCharacter) -> void:
	if player.Health > 0 or is_transitioning:
		return
	
	is_transitioning = true
	TransitionManager.SwitchMainScene("neuro_transition", "res://root_scenes/NeuroGame.tscn")
