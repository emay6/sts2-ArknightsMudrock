using BaseLib.Abstracts;
using ArknightsMudrock.ArknightsMudrockCode.Extensions;
using Godot;

namespace ArknightsMudrock.ArknightsMudrockCode.Character;

public class ArknightsMudrockRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => ArknightsMudrock.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}