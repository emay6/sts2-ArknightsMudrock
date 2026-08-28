using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace ArknightsMudrock.ArknightsMudrockCode.Relics;

public class GuldulsCrown() : ArknightsMudrockRelic
{
    public bool ActivatedThisCombat
    {
        get;
        set
        {
            AssertMutable();
            field = value;
        }
    }

    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ShieldVar(1)];

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom) ActivatedThisCombat = false;
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature) || ActivatedThisCombat || Owner.PlayerCombatState?.ShieldState()?.Shields != 0) return;
        
        Flash();
        await ShieldCmd.GainShield(DynamicVars[ShieldVar.Key].IntValue, Owner);
        ActivatedThisCombat = true;
    }
}