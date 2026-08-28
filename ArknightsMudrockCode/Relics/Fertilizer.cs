using ArknightsMudrock.ArknightsMudrockCode.Keywords;
using ArknightsMudrock.ArknightsMudrockCode.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArknightsMudrock.ArknightsMudrockCode.Relics;

public class Fertilizer() : ArknightsMudrockRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(2, ValueProp.Unpowered)];

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        if (card.Owner != Owner || !card.Keywords.Contains(MudrockKeywords.Inertial)) return Task.CompletedTask;
        
        Flash();
        return CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
    }
}