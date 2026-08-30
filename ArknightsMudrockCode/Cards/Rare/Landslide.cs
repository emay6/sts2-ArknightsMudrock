#region

using ArknightsMudrock.ArknightsMudrockCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;

public class Landslide() : ArknightsMudrockCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        //new DamageVar(7, ValueProp.Move),
        //new RepeatVar(8)
        new CardsVar(3)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<GiantRock>(IsUpgraded)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var combatState = CombatState ?? throw new InvalidOperationException("Landslide requires an active combat.");

        for (int i = 0; i < DynamicVars.Cards.BaseValue; ++i)
        {
            var rockCard = combatState.CreateCard<GiantRock>(Owner);
            if (IsUpgraded)
                CardCmd.Upgrade(rockCard);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(rockCard, PileType.Hand, Owner));
        }

        await Cmd.CustomScaledWait(0.3f, 0.6f);

        var giantRocks = MudrockUtils.GetDeckInCombat(Owner)
            .Where(c => c is GiantRock && !c.Keywords.Contains(CardKeyword.Unplayable));
        foreach (var giantRock in giantRocks)
        {
            await CardCmd.AutoPlay(choiceContext, giantRock, null);
        }

        /*await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .FromCard(this)
            .TargetingRandomOpponents(combatState)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);*/
    }

    /*protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars.Repeat.UpgradeValueBy(1);
    }*/
}
