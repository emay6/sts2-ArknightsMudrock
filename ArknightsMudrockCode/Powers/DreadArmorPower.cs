#region

using ArknightsMudrock.ArknightsMudrockCode.Cards.Common;
using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Powers;

public class DreadArmorPower() : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<DreadArmor>();
    
    public string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
    
    protected override bool IsPositive => false;
}
