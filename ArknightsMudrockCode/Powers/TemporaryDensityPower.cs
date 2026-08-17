using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public abstract class TemporaryDensityPower : CustomPowerModel, ITemporaryPower
{
    private bool _shouldIgnoreNextInstance;
    
    public override PowerType Type => !IsPositive ? PowerType.Debuff : PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public abstract AbstractModel OriginModel { get; }

    public PowerModel InternallyAppliedPower => ModelDb.Power<DensityPower>();
    
    protected virtual bool IsPositive => true;
    
    private int Sign => !IsPositive ? -1 : 1;

    public override LocString Title
    {
        get
        {
            switch (OriginModel)
            {
                case CardModel cardModel:
                    return cardModel.TitleLocString;
                case PotionModel potionModel:
                    return potionModel.Title;
                case RelicModel relicModel:
                    return relicModel.Title;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public override LocString Description => new LocString("powers", this.IsPositive ? $"{MainFile.ModId.ToUpperInvariant()}-TEMPORARY_DENSITY_POWER.description" : $"{MainFile.ModId.ToUpperInvariant()}-TEMPORARY_DENSITY_DOWN.description");

    protected override string SmartDescriptionLocKey => !this.IsPositive ? $"{MainFile.ModId.ToUpperInvariant()}-TEMPORARY_DENSITY_DOWN.smartDescription" : $"{MainFile.ModId.ToUpperInvariant()}-TEMPORARY_DENSITY_POWER.smartDescription";

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            var items = new List<IHoverTip>();
            IEnumerable<IHoverTip> collection;

            switch (this.OriginModel)
            {
                case CardModel card:
                    collection = [HoverTipFactory.FromCard(card)];
                    break;
                case PotionModel potion:
                    collection = [HoverTipFactory.FromPotion(potion)];
                    break;
                case RelicModel relic:
                    collection = HoverTipFactory.FromRelic(relic);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            items.AddRange(collection);
            items.Add(HoverTipFactory.FromPower<DensityPower>());
            return items;
        }
    }

    public void IgnoreNextInstance() => _shouldIgnoreNextInstance = true;

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<DensityPower>(new ThrowingPlayerChoiceContext(), target, Sign * amount, applier,
                cardSource, true);
        }
    }
    
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (amount == Amount || power != this) return;
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            await PowerCmd.Apply<DensityPower>(choiceContext, Owner, Sign * amount, applier, cardSource, true);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        // we want to delay the power getting removed until after the other side takes their turn 
        if (participants.Contains<Creature>(Owner)) return;
        
        this.Flash();
        await PowerCmd.Remove(this);
        await PowerCmd.Apply<DensityPower>(choiceContext, Owner, -Sign * Amount, Owner, null);
    }
}