using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    public GameObject popup;
    private TextMeshProUGUI text;
    private Image image;

    private IEnumerator fade;
    private bool active;

    public void StartFadePopup()
    {
        if (active)
        {
            StopCoroutine(fade);
            active = false;
        }

        text = popup.GetComponentInChildren<TextMeshProUGUI>();
        image = popup.GetComponentInChildren<Image>();

        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        popup.SetActive(true);

        fade = FadePopup(0.5f); 
        StartCoroutine(fade);
        active = true;
    }

    IEnumerator FadePopup(float fadeSpeed)
    {
        yield return new WaitForSeconds(1);

        while (text.color.a > 0.0f && image.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * fadeSpeed));
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime * fadeSpeed));
            yield return null;
        }

        popup.SetActive(false);
        active = false;
    }
}
