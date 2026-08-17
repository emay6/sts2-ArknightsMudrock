#region

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class SculptorPower() : ArknightsMudrockPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<GiantRock>()];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;

        var cards = await CardSelectCmd.FromHand(choiceContext, player,
            new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, Amount), c => c.IsTransformable, this);

        foreach (var card in cards)
        {
            var transform = CombatState.CreateCard<GiantRock>(player);
            await CardCmd.Transform(card, transform);
        }
    }
}