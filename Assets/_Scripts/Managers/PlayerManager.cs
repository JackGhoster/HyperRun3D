using System;
using System.Collections;
using System.Linq;
using Game.Floor;
using PlayerLogic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [field: SerializeField] public PlayerConfig PlayerConfig { get; private set; }

        public Player CurrentPlayer { get; private set; }
        public uint CurrentFloorIndex { get; private set; }
        public AssetManager AssetManager => GameManager.Instance.AssetManager;
        public FloorManager FloorManager => GameManager.Instance.FloorManager;
        public bool Initialized { get; private set; } = false;
        public event Action PlayerDied;
        public void InvokePlayerDied() => PlayerDied?.Invoke();

        public IEnumerator Init()
        {
            yield return PreloadPlayer();

            yield return SpawnPlayerCoroutine();

            Initialized = true;
        }

        private IEnumerator PreloadPlayer()
        {
            var playerAsset = PlayerConfig.PlayerAsset;
            if (!AssetManager.PreloadedAssets.TryGetValue(playerAsset, out var go))
            {
                yield return AssetManager.HandleAssetLoading(playerAsset);
            }
        }
        
        public IEnumerator SpawnPlayerCoroutine()
        {
            yield return new WaitUntil(() => FloorManager.FinishedGenerating);
            
            var playerAsset = PlayerConfig.PlayerAsset;
            if (!AssetManager.PreloadedAssets.TryGetValue(playerAsset, out var go))
            {
                yield return AssetManager.HandleAssetLoading(playerAsset);
            }
            InstantiatePlayer(go);
        }
        
        private void InstantiatePlayer(GameObject playerGameObject)
        {
            var firstFloor = FloorManager.GeneratedFloors.FirstOrDefault();

            var position = firstFloor != null ? firstFloor.gameObject.transform.position : Vector3.zero;
            var rotation = firstFloor != null ?  Quaternion.LookRotation(firstFloor.transform.right , Vector3.up): Quaternion.identity;

            var player = Instantiate(playerGameObject, position + Vector3.up, rotation).GetComponent<Player>();
            player.Stats = BuildDefaultStats();
            player.Initialize();
            CurrentPlayer = player;
            CurrentPlayer.PlayerMovement.StartMovement();
        }

        private void FixedUpdate()
        {
            if(!Initialized || CurrentPlayer == null) return;
            
            CheckCurrentPlatform();
        }

        private void CheckCurrentPlatform()
        {

            if (Physics.Raycast(CurrentPlayer.transform.position, -Vector3.up, out var hit))
            {
                var parentFloor = hit.transform.gameObject.GetComponentInParent<Floor>();
                if (parentFloor == null) return;
                CurrentFloorIndex = parentFloor.FloorIndex;
            }
            
        }

       


        private Stats BuildDefaultStats()
        {
            var statsBuilder = new PlayerStatsBuilder();
            return statsBuilder
                .WithSpeed(PlayerConfig.DefaultPlayerSpeed)
                .WithHealth(PlayerConfig.DefaultPlayerHealth)
                .WithJumpCount(PlayerConfig.DefaultJumpCount)
                .WithJumpHeight(PlayerConfig.DefaultJumpHeight)
                .WithGravity(PlayerConfig.DefaultGravity)
                .Build();
        }

    }
}