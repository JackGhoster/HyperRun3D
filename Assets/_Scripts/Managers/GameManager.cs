using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] public FloorManager FloorManager { get; private set; }
        [field: SerializeField] public AssetManager AssetManager { get; private set; }
        [field: SerializeField] public PlayerManager PlayerManager { get; private set; }
        [field: SerializeField] public CameraManager CameraManager { get; private set; }
        [field: SerializeField] public EffectManager EffectManager { get; private set; }
        
        public static GameManager Instance { get; private set; }

        public bool Initialized { get; private set; } = false;

        public event Action Won;

        private void Awake()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            if(!TryInitializeSingleton()) yield break;

            // AssetManager = AssetManager.Instance;

            yield return EffectManager.Init();
            
            yield return FloorManager.Init();

            yield return PlayerManager.Init();
            
            yield return CameraManager.Init();
        }

        private bool TryInitializeSingleton()
        {
            if (GameManager.Instance != null)
            {
                Destroy(this.gameObject);
                return false;
            }

            GameManager.Instance = this;
            this.Initialized = true;
            // DontDestroyOnLoad(this.gameObject);
            return true;
        }

        public void Win()
        {
            Won?.Invoke();
            // print("win");
        }

        public void Restart()
        {
            AssetManager.CleanUp();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        
    }
}
