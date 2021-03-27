using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundScript : MonoBehaviour
{
    private AudioSource pieceTakeSound;
    private AudioSource pieceMoveSound;

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
