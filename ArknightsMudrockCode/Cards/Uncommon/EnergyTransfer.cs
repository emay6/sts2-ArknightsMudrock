#region

using ArknightsMudrock.ArknightsMudrockCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Uncommon;

public class EnergyTransfer() : ArknightsMudrockCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    private const string EnergyGainKey = "EnergyGain";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedVar(EnergyGainKey).WithMultiplier((card, _) => card.Owner.Creature.GetPowerAmount<MomentumPower>())
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<MomentumPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var amount = ((CalculatedVar) DynamicVars[EnergyGainKey]).Calculate(Owner.Creature);
        if (amount > 0)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PlayerCmd.GainEnergy(amount, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}