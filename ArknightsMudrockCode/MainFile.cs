#region

using System.Reflection;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "ArknightsMudrock"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();
        
        ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
    }
}