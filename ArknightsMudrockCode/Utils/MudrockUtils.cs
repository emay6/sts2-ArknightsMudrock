using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using ArknightsMudrock.ArknightsMudrockCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

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

    public static (int?, decimal) CalculateRedirectData(AttackIntent intent, IEnumerable<Creature> targets, Creature owner) 
    {
        var targetList = targets.ToList();
        var target = targetList.Find(c => c.HasPower<RedirectPower>());
        
        if (target == null 
            || owner.CombatState == null 
            || owner.CombatState.Enemies.FirstOrDefault(c => c.Monster?.IntendsToAttack == true) != owner) return (null, 0);
        
        var hitCount = target.Player?.PlayerCombatState?.ShieldState()?.Shields;
        if (hitCount == 0) return (null, 0);
        
        int totalDamage = intent.GetTotalDamage(targetList, owner);
        return (hitCount, totalDamage);
    }
}