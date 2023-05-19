using System;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    [Serializable]
    public sealed class PlayerWrenchCharacteristic
    {
        [SerializeField] private int _baseWrench;
        [SerializeField] private int _maximumWrench;
        [SerializeField] private int _currentWrench;

        public int GetBaseWrench => _baseWrench;
        public int GetCurrentWrench => _currentWrench;


        public PlayerWrenchCharacteristic(PlayerWrenchCharacteristic playerCharacteristic)
        {
            _maximumWrench = playerCharacteristic._maximumWrench;
            _baseWrench = playerCharacteristic._baseWrench;
        }

        internal void LoadInitValue()
        {
            _currentWrench = _baseWrench;
        }

        internal int UpdateWrench(int value, ref int currentLives, int maxLives)
        {
            if (_currentWrench < _maximumWrench-1)
            {
                _currentWrench += value;
            }
            else
            {
                LoadInitValue();
                return currentLives < maxLives ? currentLives = maxLives : currentLives;
            }

            return _currentWrench;
        }
    }
}