using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // --- SETTINGS SAVE MANAGER
    private bool firstInit;
    // --- All the setting variables
    // Resolution
    private int s_screenHeight;
    private string s_screenHeightKey = "HEIGHT";
    private int s_screenWidth;
    private string s_screenWidthKey = "WIDTH";
    private int s_resIndex;
    private string s_resIndexKey = "RESINDEX";
    // Quality
    private int s_qualityValue;
    private string s_qualityValueKey = "QUALITY";
    // Fullscreen (1 - true, 0 - false)
    private int s_isFullscreen;
    private string s_isFullscreenKey = "FULLSCREEN"; //default check for save file
    // Audio volume
    private float s_bgMusicVolume;
    private string s_bgMusicVolumeKey = "MUSIC";
    private float s_soundVolume;
    private string s_soundVolumeKey = "SOUNDS";
    // ---

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

    void Awake()
    {
        // --- Check for save file
        firstInit = !PlayerPrefs.HasKey(s_resIndexKey);
    }
    void Start()
    {
        //true - load saved data
        if (!firstInit)
        {
            Debug.LogWarning("Loaded from save");
            // --- Pulling all values from the save file
            s_soundVolume = PlayerPrefs.GetFloat(s_soundVolumeKey);
            s_bgMusicVolume = PlayerPrefs.GetFloat(s_bgMusicVolumeKey);
            s_isFullscreen = PlayerPrefs.GetInt(s_isFullscreenKey);
            s_screenWidth = PlayerPrefs.GetInt(s_screenWidthKey);
            s_screenHeight = PlayerPrefs.GetInt(s_screenHeightKey);
            s_resIndex = PlayerPrefs.GetInt(s_resIndexKey);
            s_qualityValue = PlayerPrefs.GetInt(s_qualityValueKey);
            Debug.LogError("Resolution index: " + s_resIndex);
            // ---

            // --- Resolution control and fullscreen
            resolutions = new List<Resolution>();
            AddResolutionOptions();
            OnResolutionChanged(s_resIndex);
            resolutionDropdown.value = s_resIndex;
            fullscreenToggle.isOn = Convert.ToBoolean(s_isFullscreen);
            // ---

            // --- Quality control
            AddQualityOptions();
            OnQualityChanged(s_qualityValue);
            // ---

            // --- Audio control
            backgroundMusicSlider.value = s_bgMusicVolume;
            soundsSlider.value = s_soundVolume;

            backgroundMusicSliderPercentageText.text = $"{Convert.ToInt32(backgroundMusicSlider.value * 100)}%";
            soundsSliderPercentageText.text = $"{Convert.ToInt32(soundsSlider.value * 100)}%";

            SetAudioSourcesVolume(backgroundMusicTagsList, backgroundMusicSlider.value);
            SetAudioSourcesVolume(soundsTagsList, soundsSlider.value);

            backgroundMusicSlider.onValueChanged.AddListener(delegate { BGMusic_SliderValueChanged(); });
            soundsSlider.onValueChanged.AddListener(delegate { Sounds_SliderValueChanged(); });
            // ---
        }
        //false - initialize default data
        else
        {
            Debug.LogWarning("Didn't load from save");
            //Deletes all random entries
            PlayerPrefs.DeleteAll();

            // --- Resolution control
            // Current resolution is screen resolution by default
            s_screenWidth = currentResolution.width = Screen.currentResolution.width;
            PlayerPrefs.SetInt(s_screenWidthKey, s_screenWidth);
            s_screenHeight = currentResolution.height = Screen.currentResolution.height;
            PlayerPrefs.SetInt(s_screenHeightKey, s_screenHeight);
            s_resIndex = 0;
            PlayerPrefs.SetInt(s_resIndexKey, s_resIndex);

            resolutions = new List<Resolution>();
            AddResolutionOptions();
            // ---

            // --- Quality control
            AddQualityOptions();
            s_qualityValue = QualitySettings.GetQualityLevel();
            PlayerPrefs.SetInt(s_qualityValueKey, s_qualityValue);
            // ---

            // --- Audio control
            backgroundMusicSliderPercentageText.text = $"{Convert.ToInt32(backgroundMusicSlider.value * 100)}%";
            soundsSliderPercentageText.text = $"{Convert.ToInt32(soundsSlider.value * 100)}%";

            SetAudioSourcesVolume(backgroundMusicTagsList, backgroundMusicSlider.value);
            SetAudioSourcesVolume(soundsTagsList, soundsSlider.value);
            s_bgMusicVolume = backgroundMusicSlider.value;
            s_soundVolume = soundsSlider.value;
            PlayerPrefs.SetFloat(s_bgMusicVolumeKey, s_bgMusicVolume);
            PlayerPrefs.SetFloat(s_soundVolumeKey, s_soundVolume);

            backgroundMusicSlider.onValueChanged.AddListener(delegate { BGMusic_SliderValueChanged(); });
            soundsSlider.onValueChanged.AddListener(delegate { Sounds_SliderValueChanged(); });
            // ---

            // --- Fullscreen and save
            s_isFullscreen = 0; //default fullscreen set to 0
            PlayerPrefs.SetInt(s_isFullscreenKey, s_isFullscreen);
            PlayerPrefs.Save();
            firstInit = false;
        }
    }

    // Resolution control
    public void OnResolutionChanged(int resolutionIndex)
    {
        currentResolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, fullscreenToggle.isOn);

        // --- Save to settings registry
        s_screenWidth = resolutions[resolutionIndex].width;
        s_screenHeight = resolutions[resolutionIndex].height;
        s_resIndex = resolutionIndex;
        PlayerPrefs.SetInt(s_screenWidthKey, s_screenWidth);
        PlayerPrefs.SetInt(s_screenHeightKey, s_screenHeight);
        PlayerPrefs.SetInt(s_resIndexKey, s_resIndex);
        PlayerPrefs.Save();
        // ---
    }
    public void OnResolutionChanged()
    {
        OnResolutionChanged(resolutionDropdown.value);
    }

    public void OnFullscreenModeChanged()
    {
        // --- Save to settings registry
        s_isFullscreen = Convert.ToInt32(fullscreenToggle.isOn);
        PlayerPrefs.SetInt(s_isFullscreenKey, s_isFullscreen);
        Screen.SetResolution(currentResolution.width, currentResolution.height, Convert.ToBoolean(s_isFullscreen));
        PlayerPrefs.Save();
        // ---
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
        qualityDropdown.value = qualityValue;
        // --- Save to settings registry
        s_qualityValue = qualityValue;
        PlayerPrefs.SetInt(s_qualityValueKey, s_qualityValue);
        PlayerPrefs.Save();
        // ---
    }
    public void OnQualityChanged()
    {
        int qualityValue = qualityDropdown.value;
        OnQualityChanged(qualityValue);
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

        // --- Save to settings registry
        s_bgMusicVolume = backgroundMusicSlider.value;
        PlayerPrefs.SetFloat(s_bgMusicVolumeKey, s_bgMusicVolume);
        PlayerPrefs.Save();
        // ---
    }

    public void Sounds_SliderValueChanged()
    {
        soundsSliderPercentageText.text = $"{Convert.ToInt32(soundsSlider.value * 100)}%";
        SetAudioSourcesVolume(soundsTagsList, soundsSlider.value);

        // --- Save to settings registry
        s_soundVolume = soundsSlider.value;
        PlayerPrefs.SetFloat(s_soundVolumeKey, s_soundVolume);
        PlayerPrefs.Save();
        // ---
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
