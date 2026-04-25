using UnityEngine;

public static class AudioVolumeUtility
{
  public static float SliderToSourceVolume(float sliderValue)
  {
    float clampedValue = Mathf.Clamp01(sliderValue);
    return clampedValue * clampedValue;
  }
}
