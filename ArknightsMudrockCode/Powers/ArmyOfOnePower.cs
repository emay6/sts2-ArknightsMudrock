#region

using ArknightsMudrock.ArknightsMudrockCode.Cards.Rare;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class ArmyOfOnePower() : TemporaryDensityPower
{
    public override AbstractModel OriginModel => ModelDb.Card<ArmyOfOne>();
}