#region

using ArknightsMudrock.ArknightsMudrockCode.Character;
using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Potions;

[Pool(typeof(ArknightsMudrockPotionPool))]
public abstract class ArknightsMudrockPotion : CustomPotionModel
{
    public override string? CustomPackedImagePath => 
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();

    public override string? CustomPackedOutlinePath => 
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PotionOutlineImagePath();
}