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
        [SerializeField] private GameObject _winnerPopUp;
        /// <summary>
        /// Time in seconds
        /// </summary>
        [SerializeField] private float _animTime;
        void Start()
        {
            LeanTween.moveLocal(_winnerPopUp, Vector3.zero, _animTime)
                .setEaseInExpo()
                .setEaseOutBack();
        }
    }
}
