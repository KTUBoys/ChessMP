using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AudioSource = UnityEngine.AudioSource;

public class AudioSliderControl : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI sliderPercentageText;
    public string Tag;
    private List<AudioSource> audioSources;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        sliderPercentageText.text = $"{Convert.ToInt32(slider.value * 100)}%";
        SetAudioSourcesVolume(slider.value);
        slider.onValueChanged.AddListener(delegate { SliderValueChanged(); });
    }

    public void SliderValueChanged()
    {
        sliderPercentageText.text = $"{Convert.ToInt32(slider.value * 100)}%";
        SetAudioSourcesVolume(slider.value);
    }

    private void SetAudioSourcesVolume(float value)
    {
        audioSources = GameObject.FindGameObjectWithTag(Tag).GetComponents<AudioSource>().ToList();
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = value;
        }
    }
}
