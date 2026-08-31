#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Uncommon;

public class CrushingPeak() : ArknightsMudrockCard(4,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(16, ValueProp.Move)];

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
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card == this && !IsClone) 
            ReduceCostBy(CombatManager.Instance.History.CardPlaysFinished.Count(
                e => 
                e.CardPlay.Card.Type == CardType.Attack 
                && e.CardPlay.Card.Owner == Owner 
                && e.HappenedThisTurn(CombatState)));
        
        return Task.CompletedTask;
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && cardPlay.Card.Type == CardType.Attack)
            ReduceCostBy(1);
        
        return Task.CompletedTask;
    }

    private void ReduceCostBy(int amount) => this.EnergyCost.AddThisTurn(-amount);

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}
