using UnityEngine;
using TMPro;

public static class ColorLerpUtility
{
    public static void UpdateTextColor(float currentValue,
                                      float minValue,
                                      float maxValue,
                                      Color startColor,
                                      Color targetColor,
                                      TMP_Text textComponent)
    {
        if (textComponent == null)
        {
            return;
        }
        if (Mathf.Approximately(minValue, maxValue))
        {
            textComponent.color = targetColor;
            return;
        }
        float t = Mathf.Clamp01((currentValue - minValue) / (maxValue - minValue));
        Color lerpedColor = Color.Lerp(startColor, targetColor, t);
        textComponent.color = lerpedColor;
    }
}