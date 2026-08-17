#region

using ArknightsMudrock.ArknightsMudrockCode.Fields;
using ArknightsMudrock.ArknightsMudrockCode.Utils;
using MegaCrit.Sts2.Core.Entities.Players;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Extensions;

public static class PlayerCombatStateExtension
{
    public class ShieldCombatState()
    {
        public event Action<int, int>? ShieldChanged;
        public event Action<int, int>? ShieldValueChanged;
        public event Action<int, int>? EnergyChanged;
        
        public int MaxShieldCount
        {
            get;
            set => field = MudrockUtils.ClampMin(value, 0);
        } = Character.ArknightsMudrock.BaseMaxShieldCount;

        public int Shields
        {
            get;
            set
            {
                var prevAmount = field;
                field = MudrockUtils.ClampMin(value, 0);
                field = field > MaxShieldCount ?  MaxShieldCount : field;
                ShieldChanged?.Invoke(prevAmount, field);
            }
        } = 0;

        public int ShieldValue
        {
            get;
            set
            {
                int prevAmount = field;
                field = MudrockUtils.ClampMin(value, 1);
                ShieldValueChanged?.Invoke(prevAmount, field);
            }
        } = Character.ArknightsMudrock.BaseShieldValue;

        public int EnergyValue
        {
            get;
            set
            {
                int prevAmount = field;
                field = MudrockUtils.ClampMin(value, 0);
                EnergyChanged?.Invoke(prevAmount, field);
            }
        } = Character.ArknightsMudrock.BaseShieldEnergyValue;
    }

    extension(PlayerCombatState combatState)
    {
        public ShieldCombatState? ShieldState()
        {
            return MudrockField.ShieldCombatState[combatState];
        }
    }
}