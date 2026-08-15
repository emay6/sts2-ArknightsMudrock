#region

using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using BaseLib.Abstracts;
using Godot;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Character;

public class ArknightsMudrockPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => ArknightsMudrock.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}