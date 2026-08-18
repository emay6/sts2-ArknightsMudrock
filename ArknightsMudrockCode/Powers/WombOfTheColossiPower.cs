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
using MegaCrit.Sts2.Core.ValueProps;

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class WombOfTheColossiPower() : ArknightsMudrockPower, IAfterShieldLost
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ShieldVar(0)];


    public async Task AfterShieldLost(ICombatState combatState, Player player, Creature? source = null, ValueProp? props = null)
    {
        if (player != Owner.Player) return;

        await CardPileCmd.Draw(new HookPlayerChoiceContext(player, player.NetId, GameActionType.Combat), Amount,
            player);
    }
}