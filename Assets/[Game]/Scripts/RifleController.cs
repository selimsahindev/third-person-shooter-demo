using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class RifleController : MonoBehaviour
    {
        private Cinemachine.CinemachineImpulseSource impulseSource;

        private void Awake()
        {
            impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();

            EventManager.Instance.onPlayerPulledTheTrigger += Fire;
        }

        private void Fire()
        {
            // Generate impulse for virtual cameras in the scene
            impulseSource.GenerateImpulse(CameraManager.Instance.mainCamera.transform.forward);
        }

        private void OnDestroy()
        {
            EventManager.Instance.onPlayerPulledTheTrigger -= Fire;
        }
    }
}
