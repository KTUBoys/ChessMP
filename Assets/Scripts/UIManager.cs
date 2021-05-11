using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject ConnectionScreen;
    [SerializeField] private GameObject MenuScreen;
    [SerializeField] private GameObject Logo;
    [SerializeField] private TextMeshProUGUI ConnectionStatusText;
    public void OnPlayOnlineClick()
    {
        ConnectionScreen.SetActive(true);
        MenuScreen.SetActive(false);
        Logo.SetActive(false);
    }

    public void SetConnectionStatusText(string text)
    {
        if (ConnectionStatusText is null) 
        {
            return;
        }
        ConnectionStatusText.text = text;
    }
}
