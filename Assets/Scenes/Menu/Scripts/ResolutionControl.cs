using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionControl : MonoBehaviour
{
    private List<Resolution> resolutions;
    private Resolution currentResolution;
    public TMP_Dropdown dropdown;
    public Toggle fullscreenToggle;

    void Start()
    {
        // Current resolution is screen resolution by default
        currentResolution.width = Screen.currentResolution.width;
        currentResolution.height = Screen.currentResolution.height;

        resolutions = new List<Resolution>();
        AddResolutionOptions();
    }

    private void AddResolutionOptions()
    {
        resolutions = Screen.resolutions.Where(x => x.refreshRate.Equals(60) && x.width >= 1024 && x.height >= 768)
            .ToList();
        resolutions.Reverse(); // Reverse resolutions order from biggest to smallest

        List<string> resolutionsList = new List<string>();
        resolutions.ForEach(x => resolutionsList.Add($"{x.width} x {x.height}"));
        dropdown.AddOptions(resolutionsList);
    }

    public void OnResolutionChanged(int resolutionIndex)
    {
        currentResolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, fullscreenToggle.isOn);
    }

    public void OnFullscreenModeChanged()
    {
        Screen.SetResolution(currentResolution.width, currentResolution.height, fullscreenToggle.isOn);
    }
}
