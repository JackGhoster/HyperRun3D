using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DiedUIScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private Button _restartButton, _continueButton;
        [SerializeField] private PassedObstaclesScroll _passedObstaclesScroll; 

        private IEnumerator Start()
        {
            SetContainerActive(false);
            
            yield return new WaitUntil(() => GameManager.Instance.Initialized);
            
            yield return new WaitUntil(() => GameManager.Instance.PlayerManager.Initialized);
            
            _restartButton.onClick.AddListener(RestartActions);
            _continueButton.onClick.AddListener(ContinueActions);
            GameManager.Instance.PlayerManager.PlayerDied += OnDied;
        }

        private void ContinueActions()
        {
            SetContainerActive(false);
            _passedObstaclesScroll.SetContainerActive(false);
            var player = GameManager.Instance.PlayerManager.CurrentPlayer;
            if(player != null) player.Revive();
        }

        private void RestartActions()
        {
            SetContainerActive(false);
            _passedObstaclesScroll.SetContainerActive(false);
            GameManager.Instance.Restart();
        }
        
        private void OnDied()
        {
            SetContainerActive(true);
            _passedObstaclesScroll.FetchPassedFloors();
        }
        public void SetContainerActive(bool value) => _container.gameObject.SetActive(value);

        private void OnDestroy()
        {
            GameManager.Instance.PlayerManager.PlayerDied -= OnDied;
        }

    }
}