#region

using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using MudrockCharacter = ArknightsMudrock.ArknightsMudrockCode.Character.ArknightsMudrock;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Nodes;

[GlobalClass]
public partial class NShieldRings : Control
{
	private static readonly float[] RotationSpeeds = [0.075f, -0.085f, 0.068f, -0.092f, 0.08f];
	private const float VisibilityFadeDuration = 0.2f;
	private const float ShieldFlashDuration = 0.1f;
	private const int ShieldFlashFrameCount = 15;

	private Player? _player;
	private bool _isListening;
	private bool _isLocalPlayer;
	private NCreature? _creature;
	private Node2D[] _rotationRoots = [];
	private TextureRect[] _rings = [];
	private NCreatureStateDisplay? _stateDisplay;
	private Tween? _visibilityTween;
	private bool _targetVisible;
	private Sprite2D? _shieldFlash;
	private double _shieldFlashElapsed;

	public override void _Ready()
	{
		_rotationRoots = [
			GetNode<Node2D>("RingRotation1"),
			GetNode<Node2D>("RingRotation2"),
			GetNode<Node2D>("RingRotation3"),
			GetNode<Node2D>("RingRotation4"),
			GetNode<Node2D>("RingRotation5")
		];
		_rings = [
			GetNode<TextureRect>("RingRotation1/ShieldRing1"),
			GetNode<TextureRect>("RingRotation2/ShieldRing2"),
			GetNode<TextureRect>("RingRotation3/ShieldRing3"),
			GetNode<TextureRect>("RingRotation4/ShieldRing4"),
			GetNode<TextureRect>("RingRotation5/ShieldRing5")
		];
		_shieldFlash = GetNode<Sprite2D>("ShieldFlash");
		_shieldFlash.Visible = false;
		Modulate = new Color(1f, 1f, 1f, 0f);
		Visible = false;
	}

	public void Initialize(Player player, bool isLocalPlayer)
	{
		_player = player;
		_isLocalPlayer = isLocalPlayer;
		_creature = FindCreatureAncestor(GetParent());
		_stateDisplay = FindStateDisplay(_creature);
		ConnectShieldChangedSignals();
		Refresh();
	}

	public override void _Process(double delta)
	{
		for (var index = 0; index < _rotationRoots.Length; index++)
			_rotationRoots[index].Rotate((float)(RotationSpeeds[index] * delta));

		if (_shieldFlash?.Visible == true)
		{
			_shieldFlashElapsed += delta;
			var frame = (int)(_shieldFlashElapsed / ShieldFlashDuration * ShieldFlashFrameCount);
			if (frame >= ShieldFlashFrameCount)
			{
				_shieldFlash.Visible = false;
			}
			else
			{
				_shieldFlash.Frame = frame;
			}
		}

		if (_player?.Creature.IsAlive != true)
		{
			QueueFree();
			return;
		}

		RefreshVisibility();
	}

	private void ConnectShieldChangedSignals()
	{
		if (_player == null || _isListening) return;
		var shieldState = _player.PlayerCombatState?.ShieldState();
		shieldState?.ShieldChanged += OnShieldChanged;
		shieldState?.MaxShieldChanged += OnMaxShieldChanged;
		_isListening = true;
	}

	private void DisconnectShieldChangedSignals()
	{
		if (_player == null || !_isListening) return;
		var shieldState = _player.PlayerCombatState?.ShieldState();
		shieldState?.ShieldChanged -= OnShieldChanged;
		shieldState?.MaxShieldChanged -= OnMaxShieldChanged;
		_isListening = false;
	}

	private void OnShieldChanged(int previousValue, int newValue)
	{
		if (newValue < previousValue)
			PlayShieldBreakFlash();

		Refresh();
	}
	private void OnMaxShieldChanged(int _, int __) => Refresh();

	private void PlayShieldBreakFlash()
	{
		if (_shieldFlash == null) return;

		_shieldFlashElapsed = 0;
		_shieldFlash.Frame = 0;
		_shieldFlash.Visible = true;
	}

	private void Refresh()
	{
		var shieldState = _player?.PlayerCombatState?.ShieldState();
		var maxShields = Math.Clamp(shieldState?.MaxShieldCount ?? 0, 0, _rings.Length);
		var currentShields = Math.Clamp(shieldState?.Shields ?? 0, 0, maxShields);

		for (var index = 0; index < _rings.Length; index++)
		{
			var ring = _rings[index];
			ring.Visible = index < maxShields;
			ring.SelfModulate = new Color(1f, 1f, 1f, index < currentShields ? 1f : 0.22f);
		}

		RefreshVisibility();
	}

	private void RefreshVisibility()
	{
		var shieldState = _player?.PlayerCombatState?.ShieldState();
		var maxShields = Math.Clamp(shieldState?.MaxShieldCount ?? 0, 0, _rings.Length);
        var currentShields = Math.Clamp(shieldState?.Shields ?? 0, 0, maxShields);
        var isMudrock = _player?.Character is MudrockCharacter;
        var hasRingData = isMudrock ? maxShields > 0 : currentShields > 0;
        var isMultiplayer = _player?.Creature.CombatState?.Players.Count > 1;
        var hasFocusedOtherPlayer = isMultiplayer == true
            && _player != null
            && _creature != null
            && HasFocusedOtherPlayer(_player.Creature, _creature.GetTree().Root);
        var isFocused = _creature?.IsFocused == true;
        var shouldShowForFocus = isFocused
            || (_isLocalPlayer && (isMultiplayer != true || !hasFocusedOtherPlayer));

		var shouldShow = _player?.Creature.IsAlive == true
			&& hasRingData
			&& shouldShowForFocus
			&& (_stateDisplay == null || _stateDisplay.Visible);

		if (shouldShow == _targetVisible) return;

		_targetVisible = shouldShow;
		_visibilityTween?.Kill();

		if (shouldShow)
		{
			Visible = true;
			_visibilityTween = CreateTween();
			_visibilityTween.TweenProperty(this, "modulate:a", 1f, VisibilityFadeDuration)
				.SetTrans(Tween.TransitionType.Cubic)
				.SetEase(Tween.EaseType.Out);
		}
		else
		{
			_visibilityTween = CreateTween();
			_visibilityTween.TweenProperty(this, "modulate:a", 0f, VisibilityFadeDuration)
				.SetTrans(Tween.TransitionType.Cubic)
				.SetEase(Tween.EaseType.In);
			_visibilityTween.TweenCallback(Callable.From(() => Visible = false));
		}
	}

	private static NCreature? FindCreatureAncestor(Node? node)
	{
		while (node != null)
		{
			if (node is NCreature creature) return creature;
			node = node.GetParent();
		}

		return null;
	}

    private static bool HasFocusedOtherPlayer(Creature owner, Node node)
    {
        foreach (var child in node.GetChildren())
        {
            if (child is NCreature creature
                && creature.IsFocused
                && creature.Entity.IsPlayer
                && creature.Entity != owner)
                return true;

            if (HasFocusedOtherPlayer(owner, child)) return true;
        }

		return false;
	}

	private static NCreatureStateDisplay? FindStateDisplay(Node? node)
	{
		if (node == null) return null;
		if (node is NCreatureStateDisplay stateDisplay) return stateDisplay;

		foreach (var child in node.GetChildren())
		{
			var result = FindStateDisplay(child);
			if (result != null) return result;
		}

		return null;
	}

	public override void _ExitTree()
	{
		DisconnectShieldChangedSignals();
		base._ExitTree();
	}
}
