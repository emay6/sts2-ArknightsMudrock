using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace ArknightsMudrock.ArknightsMudrockCode.Variables;

public class ObsidianCalculatedDamage : DamageVar
{
    public const string Key = "ObsidianCalculatedDamage";

    public ObsidianCalculatedDamage() : base(Key, 0, ValueProp.Move)
    {
        // forces green number
        WasJustUpgraded = true;
    }
}