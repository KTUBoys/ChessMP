using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualityControl : MonoBehaviour
{
    private List<string> qualityList;
    public TMP_Dropdown dropdown;

    void Start()
    {
        AddQualityOptions();
    }

    private void AddQualityOptions()
    {
        qualityList = new List<string>(QualitySettings.names);
        dropdown.AddOptions(qualityList);
    }

    public void OnQualityChanged(int qualityValue)
    {
        QualitySettings.SetQualityLevel(qualityValue, true);
        Debug.Log($"Current quality level: {QualitySettings.GetQualityLevel()}");
    }
}
