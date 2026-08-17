#region

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
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8, ValueProp.Move),
        new DynamicVar("Increase",8)];

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
        ArgumentNullException.ThrowIfNull(play.Target, nameof(play.Target));
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this).Targeting(play.Target).WithHitFx("vfx/vfx_attack_blunt").Execute(choiceContext);
        DamageVar damage = this.DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.DynamicVars["Increase"].BaseValue;
        this.ExtraDamageFromPlays += this.DynamicVars["Increase"].BaseValue;
        EnergyCost.AddThisCombat(1);
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = this.DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.ExtraDamageFromPlays;
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(6M);
    }
}