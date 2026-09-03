#region

using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Common;

public class Behemoth() : ArknightsMudrockCard(4,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(27, ValueProp.Move),
        new ShieldVar(0)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
    }

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        int shieldCount = Owner.PlayerCombatState?.ShieldState()?.Shields ?? 0;

        if (card != this)
        {
            modifiedCost = originalCost;
            return false;
        }
        
        modifiedCost = originalCost - shieldCount;
        return true;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(9);
    }
}