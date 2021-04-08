using UnityEngine;

namespace Assets.Scripts
{
    public class MovementSoundScript : MonoBehaviour
    {
        private AudioSource _pieceTakeSound;
        private AudioSource _pieceMoveSound;

        private void Awake()
        {
            _pieceTakeSound = GameObject.FindGameObjectWithTag("PieceTakeSound").GetComponent<AudioSource>();
            _pieceMoveSound = GameObject.FindGameObjectWithTag("PieceMoveSound").GetComponent<AudioSource>();
        }

        public void MoveAPiece()
        {
            _pieceMoveSound.Play();
        }
        public void TakeAPiece()
        {
            _pieceTakeSound.Play();
        }
    }
}
