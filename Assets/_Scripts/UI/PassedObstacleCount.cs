using TMPro;
using UnityEngine;

namespace UI
{
    public class PassedObstacleCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name, _count;

        public void SetNameAndCount(string name, int count)
        {
            _name.text = name + ": ";
            _count.text = count.ToString();
        }
    }
}