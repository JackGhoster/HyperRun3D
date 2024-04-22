using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WinScreenUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private Button  _continueButton;
        [SerializeField] private PassedObstaclesScroll _passedObstaclesScroll; 

        private IEnumerator Start()
        {
            SetContainerActive(false);
            
            yield return new WaitUntil(() => GameManager.Instance.Initialized);
            
            yield return new WaitUntil(() => GameManager.Instance.PlayerManager.Initialized);
            
            _continueButton.onClick.AddListener(ContinueActions);
            GameManager.Instance.Won += OnWon;
        }

        private void OnWon()
        {
            _passedObstaclesScroll.FetchPassedFloors();
            SetContainerActive(true);
        }

        private void ContinueActions()
        {
            _passedObstaclesScroll.SetContainerActive(false);
            SetContainerActive(false);
            GameManager.Instance.Restart();
        }
        
        public void SetContainerActive(bool value) => _container.gameObject.SetActive(value);
    }
}