using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace ArknightsMudrock.ArknightsMudrockCode.Relics;

public class ClayDoll() : ArknightsMudrockRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ShieldVar(1)];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom) await ShieldCmd.GainShield(DynamicVars[ShieldVar.Key].IntValue, Owner);
    }
}