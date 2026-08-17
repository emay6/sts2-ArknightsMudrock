#region


using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Uncommon;

public sealed class HammerDown : ArknightsMudrockCard
{
    public HammerDown()
        : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars
    {
        get
        {
            return [
                new CalculationBaseVar(0M),
                new ExtraDamageVar(14M),
                new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature?, Decimal>) ((card, _) =>
                    CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>(e =>
                        e.HappenedThisTurn(card.CombatState) &&
                        e.CardPlay.Card.Type == CardType.Attack &&
                        e.CardPlay.Card.Owner == card.Owner)))
            ];
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade() => DynamicVars.ExtraDamage.UpgradeValueBy(4M);
}
