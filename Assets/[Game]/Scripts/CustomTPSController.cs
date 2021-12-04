using System;
using System.Collections.Generic;
using UnityEngine;

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
 
        private bool isAiming = false;
        private float targetYaw;
        private float targetPitch;

        private void Update()
        {
            HandleMovement();

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

        private void HandleMovement()
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            transform.position = Vector3.Lerp(transform.position, transform.position + input * moveSpeed, Time.deltaTime);
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

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
