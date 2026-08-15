#region

using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards;

public class Steadfast() : ArknightsMudrockCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ShieldVar(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await ShieldCmd.GainShield(DynamicVars[ShieldVar.Key].IntValue, play.Card.Owner, play);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }
}