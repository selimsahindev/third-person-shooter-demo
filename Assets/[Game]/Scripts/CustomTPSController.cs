using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MainGame
{
    public class CustomTPSController : MonoBehaviour
    {
        [SerializeField] private Transform cameraFollowTarget;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sensitivity = 10f;
        [Tooltip("Maximum pitch value (rotate up on x axis) of the camera")]
        [SerializeField] private float pitchMax = 70f;
        [Tooltip("Minimum pitch value (rotate down on x axis) of the camera")]
        [SerializeField] private float pitchMin = -30f;
        [SerializeField] private Transform debugTransform;
        [SerializeField] private LayerMask aimLayerMask = new LayerMask();
 
        private bool isAiming = false;
        private float targetYaw;
        private float targetPitch;
        private Vector3 input;
        private RaycastHit hit;
        private Animator animator;
        private Tween layerWeightTween;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();

            EventManager.Instance.onPlayerStartedAiming += OnStartedAiming;
            EventManager.Instance.onPlayerStoppedAiming += OnStoppedAiming;

            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleRaycast();

            if (Input.GetMouseButtonDown(1))
            {
                SetAim(true);
            }
            if (Input.GetMouseButtonUp(1))
            {
                SetAim(false);
            }
        }

        private void LateUpdate()
        {
            HandleCameraRotation();
        }

        private void HandleRotation()
        {
            if (isAiming)
            {
                // Look towards the raycast hit if it exists.
                // Otherwise look towards the same direction with the camera.
                RotateToPosition(hit.transform != null ? hit.point : cameraFollowTarget.forward);
            }
            else if (input.sqrMagnitude > 0.005f)
            {
                // Look towards the same direction with the camera when player moves while not aiming.
                RotateToPosition(cameraFollowTarget.forward);
            }
        }

        private void HandleRaycast()
        {
            // Crosshair is always on the center of the screen. So we can use this value:
            Vector2 centerOfTheScreen = new Vector2(Screen.width, Screen.height) / 2f;

            Ray ray = CameraManager.Instance.mainCamera.ScreenPointToRay(centerOfTheScreen);

            if (Physics.Raycast(ray, out hit, 1000f, aimLayerMask))
            {
                if (debugTransform != null)
                {
                    debugTransform.position = hit.point;
                }
            }
        }

        private void HandleMovement()
        {
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            Vector3 pos = transform.right * input.x + transform.forward * input.z;

            //transform.position = Vector3.Lerp(transform.position, transform.position + input * moveSpeed, Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, transform.position + pos * moveSpeed, Time.deltaTime);

            animator.SetFloat("Velocity", input.magnitude);
        }

        private void RotateToPosition(Vector3 lookPos)
        {
            lookPos.y = 0f;

            Vector3 lookTarget = transform.position + lookPos;
            Vector3 aimDirection = (lookTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }

        private void HandleCameraRotation()
        {
            Vector2 _input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            targetYaw += _input.x * Time.deltaTime * sensitivity * 100f;
            targetPitch -= _input.y * Time.deltaTime * sensitivity * 100f;

            // Limit values to 360
            targetYaw = ClampAngle(targetYaw, float.MinValue, float.MaxValue);
            targetPitch = ClampAngle(targetPitch, pitchMin, pitchMax);

            cameraFollowTarget.rotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
        }

        private void SetAim(bool isAiming)
        {
            this.isAiming = isAiming;
            
            if (isAiming)
            {
                EventManager.Instance.OnPlayerStartedAiming();
            }
            else
            {
                EventManager.Instance.OnPlayerStoppedAiming();
            }
        }

        private void OnStartedAiming()
        {
            layerWeightTween.Kill();
            layerWeightTween = DOTween.To(() => animator.GetLayerWeight(1), val => animator.SetLayerWeight(1, val), 1f, 0.15f);
        }

        private void OnStoppedAiming()
        {
            layerWeightTween.Kill();
            layerWeightTween = DOTween.To(() => animator.GetLayerWeight(1), val => animator.SetLayerWeight(1, val), 0f, 0.15f);
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
