#region

using ArknightsMudrock.ArknightsMudrockCode.Utils;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Nodes.Combat;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Nodes;

public static class MudrockAddedNodes
{
    public static readonly AddedNode<NCombatUi, NShieldIcon> NShieldIcon = new
    (ui =>
    {
        var shieldIcon = PreloadManager.Cache.GetScene(MudrockResources.NShieldIconPath)
            .Instantiate<NShieldIcon>();
        ui.AddChild(shieldIcon);
        return shieldIcon;
    });
}