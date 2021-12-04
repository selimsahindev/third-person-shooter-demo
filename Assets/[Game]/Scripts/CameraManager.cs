using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace MainGame
{
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        [SerializeField] private CinemachineVirtualCamera playerFollowCamera;
        [SerializeField] private CinemachineVirtualCamera playerAimCamera;

        protected override void Awake()
        {
            base.Awake();

            EventManager.Instance.onPlayerStartedAiming += SwitchToAimCamera;
            EventManager.Instance.onPlayerStoppedAiming += SwitchToFollowCamera;

            SwitchToFollowCamera();
        }

        private void SwitchToFollowCamera()
        {
            playerFollowCamera.gameObject.SetActive(true);
            playerAimCamera.gameObject.SetActive(false);
        }

        private void SwitchToAimCamera()
        {
            playerFollowCamera.gameObject.SetActive(false);
            playerAimCamera.gameObject.SetActive(true);
        }
    }
}
