using ArknightsMudrock.ArknightsMudrockCode.Hooks;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class WombOfTheColossiPower() : ArknightsMudrockPower, IAfterShieldLost
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ShieldVar(0)];


    public async Task AfterShieldLost(PlayerChoiceContext choiceContext, Player player, Creature? source = null, ValueProp? props = null)
    {
        if (player != Owner.Player) return;

        // causes state divergence sometimes in multiplayer...?
        // await CardPileCmd.Draw(choiceContext, Amount, player);

        var target = player.Creature;
        if (CombatState.CurrentSide == CombatSide.Enemy)
        {
            await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, target, Amount, target, null, silent: true);
        }
        else if (CombatState.CurrentSide == CombatSide.Player)
        {
            await CardPileCmd.Draw(choiceContext, Amount, player);
        }
    }
}