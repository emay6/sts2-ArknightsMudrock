#region

using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Combat;

#endregion

namespace ArknightsMudrock.ArknightsMudrockCode.Extensions;

public static class CombatStateTrackerExtension
{
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "NotifyCombatStateChanged")]
    private static extern void NotifyCombatStateChanged(CombatStateTracker tracker, string caller);
    
    extension(CombatStateTracker tracker)
    {
        private void OnShieldChanged(int _, int __)
        {
            NotifyCombatStateChanged(tracker, nameof(OnShieldChanged));
        }

        private void OnShieldValueChanged(int _, int __)
        {
            NotifyCombatStateChanged(tracker, nameof(OnShieldValueChanged));
        }

        private void OnShieldEnergyValueChanged(int _, int __)
        {
            NotifyCombatStateChanged(tracker, nameof(OnShieldEnergyValueChanged));
        }

        public void SubscribeShieldChanged(PlayerCombatStateExtension.ShieldCombatState shieldCombatState)
        {
            shieldCombatState.ShieldChanged += tracker.OnShieldChanged;
        }

        public void UnsubscribeShieldChanged(PlayerCombatStateExtension.ShieldCombatState shieldCombatState)
        {
            shieldCombatState.ShieldChanged -= tracker.OnShieldChanged;
        }

        public void SubscribeShieldValueChanged(PlayerCombatStateExtension.ShieldCombatState shieldCombatState)
        {
            shieldCombatState.ShieldValueChanged += tracker.OnShieldValueChanged;
        }

        public void UnsubscribeShieldValueChanged(PlayerCombatStateExtension.ShieldCombatState shieldCombatState)
        {
            shieldCombatState.ShieldValueChanged -= tracker.OnShieldValueChanged;
        }

        public void SubscribeShieldEnergyChanged(PlayerCombatStateExtension.ShieldCombatState shieldCombatState)
        {
            shieldCombatState.EnergyChanged += tracker.OnShieldEnergyValueChanged;
        }

        public void UnsubscribeShieldEnergyChanged(PlayerCombatStateExtension.ShieldCombatState shieldCombatState)
        {
            shieldCombatState.EnergyChanged -= tracker.OnShieldEnergyValueChanged;
        }
    }
}