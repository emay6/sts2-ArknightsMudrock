#region

using ArknightsMudrock.ArknightsMudrockCode.Character;
using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Potions;

[Pool(typeof(ArknightsMudrockPotionPool))]
public abstract class ArknightsMudrockPotion : CustomPotionModel
{
    public override string? CustomPackedImagePath => 
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();

    public override string? CustomPackedOutlinePath => 
        $"{Id.Entry.RemovePrefix()}_outline.png".PotionOutlineImagePath();
}