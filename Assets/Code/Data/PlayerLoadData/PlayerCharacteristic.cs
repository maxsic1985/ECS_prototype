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


        public int GetMaxBoxCountForPooling() => CurrentScore switch
        {
            <=0 => 1,
            <=1 => 1,
            <=2 => 2,
            <=4 => 3,
            <=8 => 4,
            <=10 => 5,
            _ => 6
        };


        public void LoadInitValue()
        {
            CurrentScore = _baseScore;
        }
    }
}