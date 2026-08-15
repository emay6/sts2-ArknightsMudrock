#region

using ArknightsMudrock.ArknightsMudrockCode.Cards;
using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Character;

public class ArknightsMudrock : PlaceholderCharacterModel
{
	public const string CharacterId = "ArknightsMudrock";

	public static readonly Color Color = new("ffffff");

	public override Color NameColor => Color;
	public override CharacterGender Gender => CharacterGender.Feminine;
	public override int StartingHp => 86;

	public static readonly int BaseShieldValue = 8;
	public static readonly int BaseShieldEnergyValue = 1;
	public static readonly int BaseMaxShieldCount = 3;

	public override IEnumerable<CardModel> StartingDeck =>
	[
		ModelDb.Card<StrikeMudrock>(),
		ModelDb.Card<StrikeMudrock>(),
		ModelDb.Card<StrikeMudrock>(),
		ModelDb.Card<StrikeMudrock>(),
		ModelDb.Card<StrikeMudrock>(),
		ModelDb.Card<StrikeMudrock>(),
		ModelDb.Card<DefendMudrock>(),
		ModelDb.Card<DefendMudrock>(),
		ModelDb.Card<DefendMudrock>(),
		ModelDb.Card<Upswing>()
	];

	public override IReadOnlyList<RelicModel> StartingRelics =>
	[
		ModelDb.Relic<BurningBlood>()
	];

	public override CardPoolModel CardPool => ModelDb.CardPool<ArknightsMudrockCardPool>();
	public override RelicPoolModel RelicPool => ModelDb.RelicPool<ArknightsMudrockRelicPool>();
	public override PotionPoolModel PotionPool => ModelDb.PotionPool<ArknightsMudrockPotionPool>();

	/*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
		override all the other methods that define those assets.
		These are just some of the simplest assets, given some placeholders to differentiate your character with.
		You don't have to, but you're suggested to rename these images. */
	public override Control CustomIcon
	{
		get
		{
			var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
			icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
			return icon;
		}
	}

	public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
	public override string CustomCharacterSelectIconPath => "char_select_mudrock.png".CharacterUiPath();
	public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
	public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
	public override string CustomMerchantAnimPath => "res://ArknightsMudrock/scenes/merchant/mudrock_merchant.tscn";
	public override string CustomCharacterSelectBg => "res://ArknightsMudrock/scenes/screens/char_select/char_select_bg_mudrock.tscn";
	public override NCreatureVisuals CreateCustomVisuals()
	{
		return NodeFactory<NCreatureVisuals>.CreateFromScene("res://ArknightsMudrock/scenes/creature_visuals/Mudrock2.tscn");
	}
}
