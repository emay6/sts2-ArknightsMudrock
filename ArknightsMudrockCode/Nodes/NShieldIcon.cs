#region

using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Utils;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Nodes;

[GlobalClass]
public partial class NShieldIcon : Control
{
	public static readonly Color FontColor = new Color("ffffff");
	public static readonly Color FontShadowColor = new Color("00000040");
	public static readonly Color FontOutlineColor = new Color("616161");

	private Player? _player;
	private bool _isListening;
	private HoverTip _hoverTip;
	private LocString _descLocString = null!;
	private MegaLabel _shieldCountLabel = null!;
	private MegaLabel _shieldValueLabel = null!;
	private int _shieldCount;
	private int _shieldValue;
	private int _energyValue;

	public void Initialize(Player player)
	{
		_player = player;
		
		var shieldCombatState = _player.PlayerCombatState?.ShieldState();
		_shieldCount = shieldCombatState?.Shields ?? 0;
		_shieldValue = shieldCombatState?.ShieldValue ?? Character.ArknightsMudrock.BaseShieldValue;
		_energyValue = shieldCombatState?.EnergyValue ?? Character.ArknightsMudrock.BaseShieldEnergyValue;
		
		_descLocString.Add("energyPrefix", EnergyIconHelper.GetPrefix(_player.Character.CardPool));
		
		ConnectShieldChangedSignals();
		UpdateShieldIconInfo(_shieldCount, _shieldValue, _energyValue, true);
		RefreshVisibility();
	}

	public override void _Ready()
	{
		_shieldCountLabel = CreateLabel(FontColor, FontShadowColor, FontOutlineColor, 22);
		_shieldValueLabel = CreateLabel(FontColor, FontShadowColor, FontOutlineColor, 28);
		
		GetNode<MarginContainer>("%ShieldCountContainer").AddChild(_shieldCountLabel);
		GetNode<MarginContainer>("%ShieldValueContainer").AddChild(_shieldValueLabel);

		_descLocString = MudrockResources.ShieldLocStringUiDescription;
		// UpdateHoverTip(_shieldCount, _shieldValue, _energyValue);
		
		Connect(Control.SignalName.MouseEntered, Callable.From(OnHovered));
		Connect(Control.SignalName.MouseExited, Callable.From(OnUnhovered));
		
		// UpdateShieldIconInfo(_shieldCount, _shieldValue, _energyValue, true);
		
		Visible = false;
	}

	// gives an error when ran in _Ready() before Initialize() is called but doesn't really matter (maybe fix later)
	private void UpdateHoverTip(int shieldCount, int shieldValue, int energyValue)
	{
		_descLocString.Add("ShieldCount", shieldCount);
		_descLocString.Add("ShieldValue", shieldValue);
		_descLocString.Add("EnergyValue", energyValue);
		_hoverTip = new HoverTip(MudrockResources.ShieldLocStringTitle, _descLocString,
			MudrockResources.ShieldIcon);
	}

	private static MegaLabel CreateLabel(Color fontColor, Color fontShadowColor, Color fontOutlineColor, int fontSize)
	{
		var label = new MegaLabel();
		label.MaxFontSize = 28;
		label.AutoSizeEnabled = false;
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment = VerticalAlignment.Center;
		label.AddThemeColorOverride("font_color", fontColor);
		label.AddThemeColorOverride("font_shadow_color", fontShadowColor);
		label.AddThemeColorOverride("font_outline_color", fontOutlineColor);
		label.AddThemeConstantOverride("shadow_offset_x", 3);
		label.AddThemeConstantOverride("shadow_offset_y", 3);
		label.AddThemeConstantOverride("outline_size", 15); 
		label.AddThemeConstantOverride("shadow_outline_size", 15);
		label.AddThemeFontOverride("font", BaseResources.FontKreonBoldSpaceOne);
		label.AddThemeFontSizeOverride("font_size", fontSize);
		label.Text = "1";

		return label;
	}
	
	// maybe change this later feels a bit messy
	private void UpdateShieldIconInfo(int shieldCount = -1, int shieldValue = -1, int energyValue = -1, bool initialSetup = false)
	{
		if (!initialSetup
			&& (_shieldCount == shieldCount 
			 && _shieldValue == shieldValue 
			 && _energyValue == energyValue)
			) return;
		
		if (shieldCount != -1)
		{
			_shieldCount = shieldCount;
			_shieldCountLabel.SetTextAutoSize($"×{shieldCount.ToString()}");
		}
		
		if (shieldValue != -1)
		{
			_shieldValue = shieldValue;
			_descLocString["ShieldValue"] = shieldValue;
			_shieldValueLabel.SetTextAutoSize(shieldValue.ToString());
		}
		
		if (energyValue != -1) 
			_energyValue = energyValue;
		
		UpdateHoverTip(_shieldCount, _shieldValue, _energyValue);
		RefreshVisibility();
	}

	private void ConnectShieldChangedSignals()
	{
		if (_player == null || _isListening) return;
		var shieldCombatState = _player.PlayerCombatState?.ShieldState();
		shieldCombatState?.ShieldChanged += OnShieldChanged;
		shieldCombatState?.EnergyChanged += OnShieldEnergyChanged;
		shieldCombatState?.ShieldValueChanged += OnShieldValueChanged;
		_isListening = true;
	}

	private void DisconnectShieldChangedSignals()
	{
		if (_player == null || !_isListening) return;
		var shieldCombatState = _player.PlayerCombatState?.ShieldState();
		shieldCombatState?.ShieldChanged -= OnShieldChanged;
		shieldCombatState?.EnergyChanged += OnShieldEnergyChanged;
		shieldCombatState?.ShieldValueChanged -= OnShieldValueChanged;
		_isListening = false;
	}
	
	

	// do something with previous value?? if not, can remove later and combine all these signals into one?
	private void OnShieldChanged(int previousValue, int newValue)
	{
		_shieldCount = newValue;
		UpdateShieldIconInfo(shieldCount: _shieldCount);
	}

	private void OnShieldValueChanged(int previousValue, int newValue)
	{
		_shieldValue = newValue;
		UpdateShieldIconInfo(shieldValue: _shieldValue);
	}

	private void OnShieldEnergyChanged(int previousValue, int newValue)
	{
		_energyValue = newValue;
		UpdateShieldIconInfo(energyValue: _energyValue);
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		ConnectShieldChangedSignals();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		DisconnectShieldChangedSignals();
	}

	private void OnHovered()
	{
		// temp, change later probably
		var nHoverTipSet = NHoverTipSet.CreateAndShow(this, _hoverTip);
		nHoverTipSet?.GlobalPosition = GlobalPosition + new Vector2(-35, -120);
	}

	private void OnUnhovered()
	{
		NHoverTipSet.Remove(this);
	}
	
	private int GetPlayerShields()
	{
		return _player?.PlayerCombatState?.ShieldState()?.Shields ?? 0;
	}

	private int GetPlayerShieldValue()
	{
		return _player?.PlayerCombatState?.ShieldState()?.ShieldValue ?? Character.ArknightsMudrock.BaseShieldValue;
	}

	private void RefreshVisibility()
	{
		if (_player == null)
		{
			Visible = false;
			return;
		}

		Visible = GetPlayerShields() > 0;
	}
}
