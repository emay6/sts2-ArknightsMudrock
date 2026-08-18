using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace ArknightsMudrock.ArknightsMudrockCode.Utils;

public static class MudrockUtils
{
    public static int ClampMin(int value, int min)
    {
        return value <= min ? min : value;
    }
    
    public static List<CardModel> GetDeckInCombat(Player owner)
    {
        var drawPile = PileType.Draw.GetPile(owner).Cards;
        var handPile = PileType.Hand.GetPile(owner).Cards;
        var discardPile = PileType.Discard.GetPile(owner).Cards;
        return drawPile.Concat(handPile).Concat(discardPile).ToList();
    }
}