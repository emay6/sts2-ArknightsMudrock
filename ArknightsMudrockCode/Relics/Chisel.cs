using ArknightsMudrock.ArknightsMudrockCode.Commands;
using ArknightsMudrock.ArknightsMudrockCode.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace ArknightsMudrock.ArknightsMudrockCode.Relics;

public class Chisel() : ArknightsMudrockRelic
{
    public override bool ShowCounter => true;

    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(6),
        new ShieldVar(1)
    ];

    private bool IsActivating
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            UpdateDisplay();
        }
    }

    [SavedProperty]
    private int EnergySpent
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            UpdateDisplay();
        }
    }

    public override int DisplayAmount => IsActivating ? DynamicVars.Energy.IntValue : EnergySpent % DynamicVars.Energy.IntValue;

    private void UpdateDisplay()
    {
        if (IsActivating)
        {
            Status = RelicStatus.Normal;
        }
        else
        {
            Status = EnergySpent == DynamicVars.Energy.IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
        }
        InvokeDisplayAmountChanged();
    }

    public override async Task AfterEnergySpent(CardModel card, int amount)
    {
        if (card.Owner != Owner) return;

        EnergySpent += amount;

        if (EnergySpent >= DynamicVars.Energy.IntValue)
        {
            await DoActivateVisuals();
            await ShieldCmd.GainShield(DynamicVars[ShieldVar.Key].IntValue, Owner);
            EnergySpent %= DynamicVars.Energy.IntValue;
        }
        
        InvokeDisplayAmountChanged();
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(0.5f);
        IsActivating = false;
    }
}