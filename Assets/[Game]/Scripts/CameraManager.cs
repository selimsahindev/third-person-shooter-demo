using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace MainGame
{
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        [SerializeField] private CinemachineVirtualCamera playerFollowCamera;
        public CinemachineVirtualCamera PlayerFollowCamera => playerFollowCamera;

        [SerializeField] private CinemachineVirtualCamera playerAimCamera;
        public CinemachineVirtualCamera PlayerAimCamera => playerAimCamera;

        public Camera mainCamera { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            mainCamera = Camera.main;

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
