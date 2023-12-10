
using MoreMountains.NiceVibrations;

public interface IVibrationService
{
    public void PlayHaptic(HapticTypes hapticType);
    public void StopAllHaptics();
}
