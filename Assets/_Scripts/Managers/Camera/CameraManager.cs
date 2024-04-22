using System.Collections;
using PlayerLogic;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CameraConfig _cameraConfig;
        private Player Target => GameManager.Instance.PlayerManager.CurrentPlayer;

        private bool followPlayer = false;
        
        public IEnumerator Init()
        {
            yield return new WaitUntil(() => GameManager.Instance.PlayerManager.Initialized && GameManager.Instance.PlayerManager.CurrentPlayer != null);
            followPlayer = true;
        }

        private void LateUpdate()
        {
            if (!followPlayer || Camera.main == null) return;

            var cam = Camera.main;

            cam.transform.rotation = Quaternion.LookRotation(Target.transform.forward, Vector3.up);
            cam.transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles + _cameraConfig.Tilt);
            cam.transform.position = Target.transform.position + (-Target.transform.forward * -_cameraConfig.DepthOffset) + (-Target.transform.up * -_cameraConfig.VerticalOffset);
        }
    }
}