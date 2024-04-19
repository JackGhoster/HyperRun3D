using System.Collections;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] public FloorManager FloorManager { get; private set; }
        
        public static GameManager Instance { get; private set; }

        public bool Initialized { get; private set; } = false;

        private void Awake()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            if(!TryInitializeSingleton()) yield break;

            yield return FloorManager.Init();
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
            DontDestroyOnLoad(this.gameObject);
            return true;
        }
    }
}
