#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Uncommon;

public class CrushingPeak() : ArknightsMudrockCard(3,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var combatState = CombatState ?? throw new InvalidOperationException("Crushing Peak requires an active combat.");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(combatState)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
        PlayerCmd.EndTurn(this.Owner, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }
}
