extends StaticBody3D

@export var character : FantomeCharacter

func _ready() -> void:
	character.StateController.Damaged.connect(on_damaged)

func on_damaged(health_loss : int, instigator : FantomeCharacter) -> void:
	if character.Health > 0:
		return
	
	queue_free()
