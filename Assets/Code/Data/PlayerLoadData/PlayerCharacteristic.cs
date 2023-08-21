using System;
using UnityEngine;



namespace MSuhininTestovoe.B2B
{
    [Serializable]
    public sealed class PlayerCharacteristic
    {
       
        [SerializeField] private int _baseScore;
        [field: SerializeField] public int CurrentScore { get; private set; }


        public int UpdateScore(int value)
        {
            return CurrentScore = value;
        }


        public int AddScore(int value)
        {
            return UpdateScore(CurrentScore + value);
        }


        public void LoadInitValue()
        {
            CurrentScore = _baseScore;
        }
    }
}