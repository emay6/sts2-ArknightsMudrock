#region

using ArknightsMudrock.ArknightsMudrockCode.Powers;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Keywords;

public class InertialKeyword() : CustomSingletonModel(HookType.Combat)
{
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal momentumGain = 1;

        if (cardPlay.Card.Keywords.Contains(MudrockKeywords.Inertial))
        {
            await CommonActions.ApplySelf<MomentumPower>(choiceContext, cardPlay.Card, momentumGain);
        }
        else
        {
            await PowerCmd.Remove<MomentumPower>(cardPlay.Card.Owner.Creature);
        }
    }
}