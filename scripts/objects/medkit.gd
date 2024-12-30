extends Node3D

@export var health : int = 50
@export var collision : Area3D

func _ready() -> void:
	collision.body_entered.connect(on_body_entered)

func on_body_entered(body : Node3D):
	if (body is not FantomeCharacter):
		return
	
	var chara : FantomeCharacter = body
	chara.Heal(health, null)
	
	queue_free()
