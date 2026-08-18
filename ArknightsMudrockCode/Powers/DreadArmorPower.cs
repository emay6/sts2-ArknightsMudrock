#region

using ArknightsMudrock.ArknightsMudrockCode.Cards.Common;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class DreadArmorPower() : TemporaryStrengthPower, ICustomModel
{
    public override AbstractModel OriginModel => ModelDb.Card<DreadArmor>();
    
    protected override bool IsPositive => false;
}
