using BaseLib.Abstracts;
using BaseLib.Utils;
using ArknightsMudrock.ArknightsMudrockCode.Character;

namespace ArknightsMudrock.ArknightsMudrockCode.Potions;

[Pool(typeof(ArknightsMudrockPotionPool))]
public abstract class ArknightsMudrockPotion : CustomPotionModel;