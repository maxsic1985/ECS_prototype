using System;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    [Serializable]
    public sealed class PlayerLivesCharacteristic
    {
        [SerializeField] private int _baseLives;
        [SerializeField] private int _currentLives;
        [SerializeField] private int _maxLives;

        public ref int GetCurrrentLives => ref _currentLives;
        public int GetMaxLives => _maxLives;


        public PlayerLivesCharacteristic(PlayerLivesCharacteristic playerCharacteristic)
        {
            _baseLives = playerCharacteristic._baseLives;
            _maxLives = playerCharacteristic._maxLives;
        }

        internal void LoadInitValue()
        {
            _currentLives = _baseLives;
        }

        public int UpdateLives(int value)
        {
            return _currentLives = value;
        }

        public int AddLives(int value)
        {
            return UpdateLives(_currentLives + value);
        }
    }
}