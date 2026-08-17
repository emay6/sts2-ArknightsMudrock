#region

using ArknightsMudrock.ArknightsMudrockCode.Cards.Common;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class DefenseUpBetaPower() : TemporaryDensityPower
{
    public override AbstractModel OriginModel => ModelDb.Card<DefenseUpBeta>();
}