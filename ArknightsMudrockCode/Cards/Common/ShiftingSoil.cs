#region

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Common;

public class ShiftingSoil() : ArknightsMudrockCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int cardCount = DynamicVars.Cards.IntValue;
        await CardPileCmd.Draw(choiceContext, cardCount, Owner);

        IEnumerable<CardModel> cardsToDiscard = await CardSelectCmd.FromHandForDiscard(
            choiceContext,
            Owner,
            new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, cardCount),
            null,
            this);

        await CardCmd.Discard(choiceContext, cardsToDiscard);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
