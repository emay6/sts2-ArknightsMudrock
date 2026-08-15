#region

using ArknightsMudrock.ArknightsMudrockCode.Character;
using BaseLib.Abstracts;
using BaseLib.Utils;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Potions;

[Pool(typeof(ArknightsMudrockPotionPool))]
public abstract class ArknightsMudrockPotion : CustomPotionModel;