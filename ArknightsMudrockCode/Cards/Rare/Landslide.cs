#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class Landslide() : ArknightsMudrockCard(5,
    CardType.Attack, CardRarity.Rare,
    TargetType.RandomEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7, ValueProp.Move),
        new RepeatVar(8)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var combatState = CombatState ?? throw new InvalidOperationException("Landslide requires an active combat.");

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .FromCard(this)
            .TargetingRandomOpponents(combatState)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
}
