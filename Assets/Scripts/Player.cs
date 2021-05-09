using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player
    {
        public List<GameObject> Pieces;
        public List<GameObject> CapturedPieces;

        public string Name { get; }
        public readonly int Forward;
        public readonly bool White;

        public Player(string name, bool positiveZMovement, bool white)
        {
            Name = name;
            White = white;
            Pieces = new List<GameObject>();
            CapturedPieces = new List<GameObject>();

            if (positiveZMovement) Forward = 10;
            else Forward = -10;
        }
    }
}