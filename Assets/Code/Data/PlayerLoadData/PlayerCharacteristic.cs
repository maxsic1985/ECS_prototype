using System;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    [Serializable]
    public sealed class PlayerCharacteristic
    {
        [SerializeField] private int _id;
        [SerializeField] private string _playerName;
        [SerializeField] private Sprite _icon;
       
        [SerializeField] private float _baseSpeed;

        [SerializeField] private int _baseCoins;
        [SerializeField] private int _currentCoins;

        [SerializeField] private int _baseScore;
        [SerializeField] private int _currentScore;

        [SerializeField] private PlayerWrenchCharacteristic _playerWrenchCharacteristic;
        [SerializeField] private PlayerLivesCharacteristic _playerLivesCharacteristic;
        
     
       

        public int GetCurrentCoins => _currentCoins;
        public PlayerWrenchCharacteristic GetWrench => _playerWrenchCharacteristic;
        public PlayerLivesCharacteristic GetLives => _playerLivesCharacteristic;
        


        public PlayerCharacteristic(PlayerCharacteristic playerCharacteristic)
        {
            _id = playerCharacteristic._id;
            _playerName = playerCharacteristic._playerName;
            _icon = playerCharacteristic._icon;
          
            _baseSpeed = playerCharacteristic._baseSpeed;
            
            
            _playerLivesCharacteristic = playerCharacteristic._playerLivesCharacteristic;
            _playerWrenchCharacteristic = playerCharacteristic._playerWrenchCharacteristic;
        }


      

        public int UpdateCoins(int value)
        {
            return _currentCoins = value;
        }


        public int UpdateScore(int value)
        {
            return _currentScore = value;
        }
        


        public int AddCoins(int value)
        {
            return UpdateCoins(_currentCoins + value);
        }

        public int AddScore(int value)
        {
            return UpdateScore(_currentScore + value);
        }


        
     

        public void LoadInitValue()
        {
          
            _currentCoins = _baseCoins;
            _currentScore = _baseScore;
          _playerWrenchCharacteristic.LoadInitValue();
          _playerLivesCharacteristic.LoadInitValue();
        }
    }
}