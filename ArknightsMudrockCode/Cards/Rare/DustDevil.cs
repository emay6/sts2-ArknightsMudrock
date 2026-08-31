#region

using ArknightsMudrock.ArknightsMudrockCode.Keywords;
using ArknightsMudrock.ArknightsMudrockCode.Powers;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class DustDevil() : ArknightsMudrockCard(3,
    CardType.Attack, CardRarity.Rare,
    TargetType.AllEnemies)
{
    private const string CalculatedHitsKey = "CalculatedHits";
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MudrockKeywords.Inertial];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(10, ValueProp.Move),
        new DynamicVar("HitCount", 3)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var numHits = DynamicVars["HitCount"].IntValue;
        await CommonActions.CardAttack(this, play, hitCount: numHits, vfx: "vfx/vfx_giant_horizontal_slash").Execute(choiceContext);
        
        if (CombatState == null) return;
        
        List<PileType> piles = [PileType.Draw, PileType.Hand, PileType.Discard];
        foreach (var pile in piles)
        {
            if (pile != PileType.Hand)
                CardCmd.PreviewCardPileAdd(
                    await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Debris>(Owner), pile, Owner));
            else
                await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Debris>(Owner), pile, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}