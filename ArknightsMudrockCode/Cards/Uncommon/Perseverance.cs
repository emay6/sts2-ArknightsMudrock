#region

using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Uncommon;

public class Perseverance() : ArknightsMudrockCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ShieldVar(1),
        new CardsVar(1)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        int shieldAmount = IsUpgraded ? 2 : DynamicVars[ShieldVar.Key].IntValue;
        await ShieldCmd.GainShield(shieldAmount, Owner, play);
        await CommonActions.Draw(this, choiceContext);
    }

    
    protected override void OnUpgrade() => DynamicVars[ShieldVar.Key].BaseValue = 2;
}
