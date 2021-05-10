using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum PlayerType
    {
        White,
        Black
    }

    public class Player
    {
        public List<GameObject> Pieces;

        public string Name { get; }
        public readonly int Forward;
        public readonly PlayerType PlayerType;

        public Player(string name, PlayerType playerType)
        {
            Name = name;
            White = white;
            Pieces = new List<GameObject>();

            if (playerType.Equals(PlayerType.White))
            {
                Forward = 10;
                PlayerType = PlayerType.White;
            }
            else
            {
                Forward = -10;
                PlayerType = PlayerType.Black;
            }
        }
    }
}