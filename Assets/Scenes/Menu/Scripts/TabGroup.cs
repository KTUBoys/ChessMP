using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> TabButtons;
    public Color TabIdle;
    public Color TabHover;
    public Color TabActive;
    public TabButton SelectedTab;
    public List<GameObject> ObjectsToSwap;

    public void Subscribe(TabButton button)
    {
        if (TabButtons == null)
        {
            TabButtons = new List<TabButton>();
        }

        TabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (SelectedTab != null || button != SelectedTab)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().color = TabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        SelectedTab = button;
        ResetTabs();
        button.GetComponentInChildren<TextMeshProUGUI>().color = TabActive;

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < ObjectsToSwap.Count; i++)
        {
            if (i == index)
            {
                ObjectsToSwap[i].SetActive(true);
            }
            else
            {
                ObjectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(var button in TabButtons)
        {
            if (SelectedTab != null && button == SelectedTab) { continue; } 
            button.GetComponentInChildren<TextMeshProUGUI>().color = TabIdle;
        }
    }
}
