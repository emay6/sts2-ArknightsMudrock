using ArknightsMudrock.ArknightsMudrockCode.Cards;
using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Keywords;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Uncommon;

public class SoilsEmbrace() : ArknightsMudrockCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ShieldVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Debris>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await ShieldCmd.GainShield(DynamicVars[ShieldVar.Key].IntValue, Owner, play);

        
        if (CombatState != null)
        {
            var hand = PileType.Hand.GetPile(Owner).Cards.Where(c => c.IsTransformable).ToList();

            foreach (var card in hand)
            {
                var transformCard = CombatState.CreateCard<Debris>(Owner);
                await CardCmd.Transform(card, transformCard);
            }
        }
        // await CardCmd.Discard(choiceContext, hand.Where(c => c.Keywords.Contains(MudrockKeywords.Inertial)));
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}