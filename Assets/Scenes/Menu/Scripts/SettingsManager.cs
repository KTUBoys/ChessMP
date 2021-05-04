using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // --- Resolution var.
    private List<Resolution> resolutions;
    private Resolution currentResolution;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    // ---

    // --- Quality var.
    private List<string> qualityList;
    public TMP_Dropdown qualityDropdown;
    // ---

    // --- Audio var.
    public Slider backgroundMusicSlider;
    public Slider soundsSlider;

    public TextMeshProUGUI backgroundMusicSliderPercentageText;
    public TextMeshProUGUI soundsSliderPercentageText;

    public List<string> backgroundMusicTagsList;
    public List<string> soundsTagsList;

    private List<AudioSource> audioSources;
    // ---

    void Start()
    {
        // --- Resolution control
        // Current resolution is screen resolution by default
        currentResolution.width = Screen.currentResolution.width;
        currentResolution.height = Screen.currentResolution.height;

        resolutions = new List<Resolution>();
        AddResolutionOptions();
        // ---

        // --- Quality control
        AddQualityOptions();
        // ---

        // --- Audio control
        backgroundMusicSliderPercentageText.text = $"{Convert.ToInt32(backgroundMusicSlider.value * 100)}%";
        soundsSliderPercentageText.text = $"{Convert.ToInt32(soundsSlider.value * 100)}%";

        SetAudioSourcesVolume(backgroundMusicTagsList, backgroundMusicSlider.value);
        SetAudioSourcesVolume(soundsTagsList, soundsSlider.value);

        backgroundMusicSlider.onValueChanged.AddListener(delegate { BGMusic_SliderValueChanged(); });
        soundsSlider.onValueChanged.AddListener(delegate { Sounds_SliderValueChanged(); });
        // ---
    }
    
    // Resolution control
    public void OnResolutionChanged(int resolutionIndex)
    {
        currentResolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, fullscreenToggle.isOn);
    }

    public void OnFullscreenModeChanged()
    {
        Screen.SetResolution(currentResolution.width, currentResolution.height, fullscreenToggle.isOn);
    }

    private void AddResolutionOptions()
    {
        resolutions = Screen.resolutions.Where(x => x.refreshRate.Equals(60) && x.width >= 1024 && x.height >= 768)
            .ToList();
        resolutions.Reverse(); // Reverse resolutions order from biggest to smallest

        List<string> resolutionsList = new List<string>();
        resolutions.ForEach(x => resolutionsList.Add($"{x.width} x {x.height}"));
        resolutionDropdown.AddOptions(resolutionsList);
    }

    // Quality control
    public void OnQualityChanged(int qualityValue)
    {
        QualitySettings.SetQualityLevel(qualityValue, true);
        Debug.Log($"Current quality level: {QualitySettings.GetQualityLevel()}");
    }

    private void AddQualityOptions()
    {
        qualityList = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityList);
    }

    // Audio control
    public void BGMusic_SliderValueChanged()
    {
        backgroundMusicSliderPercentageText.text = $"{Convert.ToInt32(backgroundMusicSlider.value * 100)}%";
        SetAudioSourcesVolume(backgroundMusicTagsList, backgroundMusicSlider.value);
    }

    public void Sounds_SliderValueChanged()
    {
        soundsSliderPercentageText.text = $"{Convert.ToInt32(soundsSlider.value * 100)}%";
        SetAudioSourcesVolume(soundsTagsList, soundsSlider.value);
    }

    private void SetAudioSourcesVolume(List<string> tagsList, float value)
    {
        if (tagsList.Any())
        {
            try
            {
                foreach (var tag in tagsList)
                {
                    audioSources = GameObject.FindGameObjectWithTag(tag).GetComponents<AudioSource>().ToList();
                    audioSources.ForEach(x => x.volume = value);
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log($"{e.InnerException}: A non-existing tag contains the tag list!");
                throw;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
    }
}
