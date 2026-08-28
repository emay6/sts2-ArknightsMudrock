#region

using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Common;

public class NaturesWard() : ArknightsMudrockCard(2,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ShieldVar(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await ShieldCmd.GainShield(DynamicVars[ShieldVar.Key].IntValue, Owner, play);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        int shieldCount = Owner.PlayerCombatState?.ShieldState()?.Shields ?? 0;
        
        if (card == this && shieldCount == 0)
        {
            modifiedCost = originalCost - 1;
            return true;
        } 
        else
        {
            modifiedCost = originalCost;
            return false;
        }
    }
}