using TMPro;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Text resolutionText;

    void Start()
    {
        SetUpResolutionDropdown();
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    void SetUpResolutionDropdown()
    {
        Resolution[] resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        foreach (var resolution in resolutions)
        {
            options.Add(resolution.width + "x" + resolution.height);
        }
        resolutionDropdown.AddOptions(options);

        int currentResolutionIndex = System.Array.FindIndex(resolutions, r => r.width == Screen.width && r.height == Screen.height);
        resolutionDropdown.value = currentResolutionIndex >= 0 ? currentResolutionIndex : 0;
        resolutionDropdown.RefreshShownValue();

        if (resolutionText != null)
        {
            resolutionText.text = "Current Resolution: " + Screen.width + "x" + Screen.height;
        }
    }

    void OnResolutionChanged(int selectedIndex)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (selectedIndex >= 0 && selectedIndex < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[selectedIndex];
            Debug.Log($"Selected Resolution: {selectedResolution.width}x{selectedResolution.height}");
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);

            if (resolutionText != null)
            {
                resolutionText.text = "Current Resolution: " + selectedResolution.width + "x" + selectedResolution.height;
            }
        }
        else
        {
            Debug.LogWarning("Selected index is out of range of available resolutions.");
        }
    }

    public void ApplyResolution()
    {
        int selectedIndex = resolutionDropdown.value;
        Resolution[] resolutions = Screen.resolutions;
        if (selectedIndex >= 0 && selectedIndex < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[selectedIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
            Debug.Log($"Applied Resolution: {selectedResolution.width}x{selectedResolution.height}");
        }
        else
        {
            Debug.LogWarning("Selected index is out of range of available resolutions.");
        }
    }

    public void LoadResolution(SettingsData settingsData)
    {
        Screen.SetResolution(settingsData.resolutionWidth, settingsData.resolutionHeight, Screen.fullScreen);
    }

    public void SaveResolution(SettingsData settingsData)
    {
        settingsData.resolutionWidth = Screen.width;
        settingsData.resolutionHeight = Screen.height;
    }
}
