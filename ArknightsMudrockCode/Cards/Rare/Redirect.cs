using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Powers;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class Redirect() : ArknightsMudrockCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ShieldVar(1),
        new PowerVar<RedirectPower>(1)
    ];

    // unfortunately card does not work properly in multiplayer
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.SingleplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState != null)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await ShieldCmd.GainShield(DynamicVars[ShieldVar.Key].IntValue, Owner, play);
            // await CommonActions.Apply<RedirectPower>(choiceContext, CombatState.PlayerCreatures, this);
            await CommonActions.ApplySelf<RedirectPower>(choiceContext, this);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}