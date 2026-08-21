#region

using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Hooks;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Common;

public class NaturesWard() : ArknightsMudrockCard(2,
    CardType.Skill, CardRarity.Common,
    TargetType.Self), IAfterShieldGained, IAfterShieldLost
{
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

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone) return Task.CompletedTask;
        UpdateEnergyCost();
        return Task.CompletedTask;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner) return Task.CompletedTask;
        UpdateEnergyCost();
        return Task.CompletedTask;
    }

    public Task AfterShieldGained(ICombatState combatState, Player player)
    {
        if (player != Owner) return Task.CompletedTask;
        UpdateEnergyCost();
        return Task.CompletedTask;
    }

    public Task AfterShieldLost(PlayerChoiceContext choiceContext, Player player, Creature? source = null, ValueProp? props = null)
    {
        if (player != Owner) return Task.CompletedTask;
        UpdateEnergyCost();
        return Task.CompletedTask;
    }

    private void UpdateEnergyCost()
    {
        int shieldCount = Owner.PlayerCombatState?.ShieldState()?.Shields ?? 0;
        if (shieldCount == 0)
        {
            EnergyCost.SetThisTurn(EnergyCost.GetWithModifiers(CostModifiers.All) - 1);
        } 
        else
        {
            EnergyCost.SetThisTurn(EnergyCost.GetWithModifiers(CostModifiers.All));
        }
    }
}