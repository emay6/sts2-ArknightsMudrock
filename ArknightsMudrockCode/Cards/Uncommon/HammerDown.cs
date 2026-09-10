#region

using ArknightsMudrock.ArknightsMudrockCode.Powers;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Uncommon;

public sealed class HammerDown : ArknightsMudrockCard
{
    public HammerDown()
        : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(20, ValueProp.Move),
        new PowerVar<NoMomentumPower>(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
        await CommonActions.ApplySelf<NoMomentumPower>(choiceContext, this);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(6);
}
