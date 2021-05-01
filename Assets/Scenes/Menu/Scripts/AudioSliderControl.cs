using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AudioSource = UnityEngine.AudioSource;
using GameObject = UnityEngine.GameObject;

public class AudioSliderControl : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI sliderPercentageText;
    public List<string> tagsList;
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

