using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI
{
    public class PassedObstaclesScroll : MonoBehaviour
    {

        [SerializeField] private RectTransform _container;
        [SerializeField] private RectTransform _content;
        [SerializeField] private PassedObstacleCount _prefab;

        private void Awake()
        {
            SetContainerActive(false);
        }


        public void FetchPassedFloors()
        {
            SetContainerActive(true);
            
            var fm = GameManager.Instance.FloorManager;
            var dangerousFloorTypes = fm.GetDangerousFloorTypes();

            Clear();
            
            foreach (var floorType in dangerousFloorTypes)
            {
                var count = Instantiate(_prefab, _content.transform).GetComponent<PassedObstacleCount>();
                
                count.SetNameAndCount(floorType.ToString(), fm.GetCountOfPassedByType(floorType));
            }
        }

        private void Clear()
        {
            var counts = _content.GetComponentsInChildren<PassedObstacleCount>();
            if(counts.Length == 0) return;
            foreach (var count in counts)
            {
                if(count.gameObject != null) Destroy(count.gameObject);
            }
        }

        public void SetContainerActive(bool value) => _container.gameObject.SetActive(value);
        
        
    }
}