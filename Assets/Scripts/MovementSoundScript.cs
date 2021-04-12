using UnityEngine;

namespace Assets.Scripts
{
    public class MovementSoundScript : MonoBehaviour
    {
        public AudioSource pieceTakeSound;
        public AudioSource pieceMoveSound;

        private void Awake()
        {
            pieceTakeSound = GameObject.FindGameObjectWithTag("PieceTakeSound").GetComponent<AudioSource>();
            pieceMoveSound = GameObject.FindGameObjectWithTag("PieceMoveSound").GetComponent<AudioSource>();
        }

        public void MoveAPiece()
        {
            pieceMoveSound.Play();
        }
        public void TakeAPiece()
        {
            pieceTakeSound.Play();
        }
    }
}
