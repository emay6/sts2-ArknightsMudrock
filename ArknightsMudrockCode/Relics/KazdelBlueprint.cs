using ArknightsMudrock.ArknightsMudrockCode.Relics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace ArknightsMudrock.ArknightsMudrockCode.Relics;

public class KazdelBlueprint() : ArknightsMudrockRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || Owner.PlayerCombatState?.TurnNumber != 1) return;

        var validCards = PileType.Hand.GetPile(Owner).Cards
            .Where(c => c.EnergyCost.GetWithModifiers(CostModifiers.None) > 0 || c.BaseStarCost > 0).ToList();
        var card = Owner.RunState.Rng.CombatCardSelection.NextItem(validCards);
        if (card != null)
        {
            Flash();
            card.SetToFreeThisTurn();
        }
    }
}