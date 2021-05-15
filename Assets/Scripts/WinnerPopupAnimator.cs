using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class WinnerPopupAnimator : MonoBehaviour
    {
        /// <summary>
        /// Winner pop up canvas
        /// </summary>
        [SerializeField] private GameObject winnerPopUp;
        /// <summary>
        /// Time in seconds
        /// </summary>
        [SerializeField] private float animTime;
        void Start()
        {
            LeanTween.moveLocal(winnerPopUp, Vector3.zero, animTime);
        }
    }
}
