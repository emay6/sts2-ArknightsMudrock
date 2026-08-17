#region

using ArknightsMudrock.ArknightsMudrockCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class DesecrationPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(MudrockKeywords.Inertial),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];

    public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
    {
        if (card.Owner.Creature == Owner && card.Type == CardType.Skill)
        {
            keywords.Add(MudrockKeywords.Inertial);
            return true;
        }

        return false;
    }

    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card, bool isAutoPlay,
        ResourceInfo resources, PileType pileType, CardPilePosition position)
    {
        if (card.Owner.Creature == Owner && card.Type == CardType.Skill)
            return (PileType.Exhaust, position);

        return (pileType, position);
    }
}