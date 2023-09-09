using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPUpdateColorFromBrightness : MonoBehaviour
{
    private Color32 _skyboxMaterial;
    private Texture _skyboxTex;

    [SerializeField] private static readonly Color32 _sceneSkyboxBrightness = new Color32(145, 145, 145, 255);

    private static readonly int _combinedSkyboxRGBA = _sceneSkyboxBrightness.r + _sceneSkyboxBrightness.g +
                                                      _sceneSkyboxBrightness.b + _sceneSkyboxBrightness.a;

    [SerializeField] private List<TextMeshProUGUI> _uiTextMeshProAsset;
    [SerializeField] private List<string> _shaderColorNames = new List<string>() { "_Tint", "_Color" };
    [SerializeField] private List<string> _skyboxTextureNames = new List<string>() { "_Tex", "_MainTexture" };
    [SerializeField] private Color _lowBrightnessColor = Color.white;
    [SerializeField] private Color _highBrightnessColor = Color.black;
    [SerializeField] private bool _useSkyboxTexture = true;

    private void Start()
    {
        SetTextColor();
    }

    private void SetTextColor()
    {
        if (RenderSettings.skybox == null) return; // If no skybox is found return
        foreach (string i in _shaderColorNames) // For every string inside Shader Color Names List check if a name matches
        {
            if (RenderSettings.skybox.HasColor(i))
            {
                _skyboxMaterial = RenderSettings.skybox.GetColor(i); // If a name matches get the color
            }
        }

        if (_useSkyboxTexture) // If Use Skybox Texture is checked
        {
            foreach (string _skyboxTexture in _skyboxTextureNames)
            {
                if (RenderSettings.skybox.HasColor(_skyboxTexture))
                {
                    _skyboxTex = RenderSettings.skybox.GetTexture(_skyboxTexture);
                }
            }
        }

        int _combinedRGB = _skyboxMaterial.r + _skyboxMaterial.g + _skyboxMaterial.b +
                           _skyboxMaterial.a; // Combine RGBA into one int value

        if (_combinedRGB < _combinedSkyboxRGBA || _skyboxTex != null) // If the combined value is lower than a set brightness value
        {
            foreach (TextMeshProUGUI tmpText in _uiTextMeshProAsset)
            {
                tmpText.color = _lowBrightnessColor;
            }
        }
        else
        {
            foreach (TextMeshProUGUI tmpText in _uiTextMeshProAsset)
            {
                tmpText.color = _highBrightnessColor;
            }
        }
    }
}