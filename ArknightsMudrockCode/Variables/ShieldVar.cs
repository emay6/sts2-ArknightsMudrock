#region

using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Variables;

public class ShieldVar : DynamicVar
{
    public const string Key = "Shield";

    public ShieldVar(decimal shieldCount) : base(Key, shieldCount)
    {
        this.WithTooltip();
    }
}