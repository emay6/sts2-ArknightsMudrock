extends TextureRect

# Speed of rotation in radians per second
@export var rotation_speed: float = 0.8

func _process(delta: float) -> void:
	# Spins the individual texture rect in place
	rotation += rotation_speed * delta
