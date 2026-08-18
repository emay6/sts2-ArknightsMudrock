#region

using ArknightsMudrock.ArknightsMudrockCode.Powers;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Common;

public class DreadArmor() : ArknightsMudrockCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.AllEnemies)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("StrengthLoss",2M)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        var combatState = CombatState ?? throw new InvalidOperationException("Dread Armor requires an active combat.");
        foreach (Creature hittableEnemy in combatState.HittableEnemies)
        {
            await PowerCmd.Apply<DreadArmorPower>(choiceContext, hittableEnemy,
                DynamicVars["StrengthLoss"].BaseValue, Owner.Creature, this);
        }
    }
    
    protected override void OnUpgrade() => DynamicVars["StrengthLoss"].UpgradeValueBy(1M);
}
