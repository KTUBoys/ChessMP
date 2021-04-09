using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.Menu.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadSceneAsync("Scenes/GameView");
            Debug.Log("Switch scene to GameView");
        }

        public void Exit()
        {
            Debug.Log("Exit");
            Application.Quit();
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
