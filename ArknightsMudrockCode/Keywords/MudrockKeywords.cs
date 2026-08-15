#region

using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Keywords;

public static class MudrockKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Inertial;
}