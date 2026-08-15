#region

using Godot;
using MegaCrit.Sts2.Core.Localization;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Utils;

public class MudrockResources
{
    public static Texture2D ShieldIcon =>
        ResourceLoader.Load<Texture2D>("res://ArknightsMudrock/images/ui/combat/shield.png");
    
    public const string NShieldIconPath = "res://ArknightsMudrock/scenes/combat/shield_icon.tscn";

    public static LocString ShieldLocStringTitle =>
        new LocString("static_hover_tips", "ARKNIGHTSMUDROCK-SHIELD.title");

    public static LocString ShieldLocStringUiDescription =>
        new LocString("static_hover_tips", "ARKNIGHTSMUDROCK-SHIELD.uiDescription");

    public static LocString ShieldLocStringDescription =>
        new LocString("static_hover_tips", "ARKNIGHTSMUDROCK-SHIELD.description");
}