using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scenes.Menu.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadSceneAsync("Scenes/GameView_SinglePlayer");
            Debug.Log("Switch scene to GameView");
        }

        public void Exit()
        {
            Debug.Log("Exit");
            Application.Quit();
        }

        // Resets button scaling to avoid stuck scaling on animations
        public void ResetButtonScale()
        {
            foreach (var btn in gameObject.GetComponentsInChildren<Button>())
            {
                btn.transform.localScale = new Vector3(1, 1);
            }
        }
    }
}