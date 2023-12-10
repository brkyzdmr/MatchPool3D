
using MoreMountains.NiceVibrations;
using UnityEngine;

public class VibrationService : Service, IVibrationService
{
    public VibrationService(Contexts contexts) : base(contexts)
    {
    }

    public void PlayHaptic(HapticTypes hapticType)
    {
        Debug.Log("Haptic Played: " + hapticType);
        MMVibrationManager.Haptic(hapticType);
    }

    public void StopAllHaptics()
    {
        MMVibrationManager.StopAllHaptics();
    }
}
