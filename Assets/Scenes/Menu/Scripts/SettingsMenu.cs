using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Button FirstSelectedButton;

    void Start()
    {
        FirstSelectedButton.Select();
    }
}
