using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(CameraConfig), fileName = nameof(CameraConfig))]
    public class CameraConfig : ScriptableObject
    {
        [field:SerializeField] public float VerticalOffset { get; private set; }
        [field:SerializeField] public float DepthOffset { get; private set; }
        [field:SerializeField] public Vector3 Tilt { get; private set; }

        public Vector3 GetOffset()
        {
            return new Vector3(DepthOffset, VerticalOffset,0);
        }
    }
}