class_name DokiPlayerScheme

extends PlayerScheme

func _physics_process(delta : float):
	super(delta)
	
	var game3D : FantomeGame = FantomeEngine.GetGame()
	var player : DokiCharacter = game3D.Player

	player.AttemptCombo = Input.is_action_just_pressed("fight_rush")
	player.AttemptFinisher = Input.is_action_just_pressed("fight_fin")
	player.AttemptDodge = Input.is_action_just_pressed("adv_jump")
