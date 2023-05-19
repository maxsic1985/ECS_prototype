using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace MSuhininTestovoe.B2B
{
    public sealed class SharedData
    {
        private PlayerSharedData _playerShared;
        
        public PlayerSharedData GetPlayerSharedData => _playerShared;

        public async Task Init()
        {
            AsyncOperationHandle<PlayerSharedData> handlePlayer =
                Addressables.LoadAssetAsync<PlayerSharedData>(AssetsNamesConstants.PLAYER_SHARED_DATA);
            await handlePlayer.Task;

            _playerShared = handlePlayer.Result;
        }
    }
}