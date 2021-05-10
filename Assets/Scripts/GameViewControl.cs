using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.GameView.Scripts
{
    public class GameViewControl : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadSceneAsync("Scenes/Menu");
                Debug.Log("Pressed escape: back to Menu scene");
            }
        }
    }
}
