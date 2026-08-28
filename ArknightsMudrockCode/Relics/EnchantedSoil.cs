using ArknightsMudrock.ArknightsMudrockCode.Keywords;
using ArknightsMudrock.ArknightsMudrockCode.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace ArknightsMudrock.ArknightsMudrockCode.Relics;

public class EnchantedSoil() : ArknightsMudrockRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Shop;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(MudrockKeywords.Inertial)];

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (EnchantedSoil.CanAffect(card)) 
            CardCmd.ApplyKeyword(card, MudrockKeywords.Inertial);

        return Task.CompletedTask;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom && Owner.PlayerCombatState != null)
        {
            foreach (var card in Owner.PlayerCombatState.AllCards)
            {
                if (EnchantedSoil.CanAffect(card))
                    CardCmd.ApplyKeyword(card, MudrockKeywords.Inertial);
            }
        }

        return Task.CompletedTask;
    }

    private static bool CanAffect(CardModel card)
    {
        return card.Rarity == CardRarity.Basic && card.Tags.Contains(CardTag.Strike) &&
               !card.GetKeywordsWithSources(KeywordSources.Local).Contains(MudrockKeywords.Inertial);
    }
}