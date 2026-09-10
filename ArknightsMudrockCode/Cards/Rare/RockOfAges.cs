#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class RockOfAges() : ArknightsMudrockCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)

{
    public const string _increaseKey = "Increase";
    public Decimal _extraDamageFromPlays;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(10, ValueProp.Move),
        new DynamicVar("Increase",7)];

    public Decimal ExtraDamageFromPlays
    {
        get => this._extraDamageFromPlays;
        set
        {
            this.AssertMutable();
            this._extraDamageFromPlays = value;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
        EnergyCost.AddThisCombat(1);
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + DynamicVars["Increase"].BaseValue;
        ExtraDamageFromPlays += DynamicVars["Increase"].BaseValue;
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + ExtraDamageFromPlays;
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(3);
    }
}