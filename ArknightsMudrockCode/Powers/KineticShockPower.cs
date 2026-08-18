using ArknightsMudrock.ArknightsMudrockCode.Powers;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class KineticShockPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != Owner || command._sourceType != AttackCommand.SourceType.Card ||
            !command.DamageProps.IsPoweredAttack()) return Task.CompletedTask;

        command.WithValueProp(command.DamageProps | ValueProp.Unblockable);
        return Task.CompletedTask;
    }
}