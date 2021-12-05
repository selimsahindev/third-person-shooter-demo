using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Random = UnityEngine.Random;

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
        [SerializeField] private float recoil = 0.25f; 
        [SerializeField] private Transform debugTransform;
        [SerializeField] private LayerMask aimLayerMask = new LayerMask();
        [SerializeField] private RifleController rifle;
 
        private bool isAiming = false;
        private bool isPullingTheTrigger = false;
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
            EventManager.Instance.onWeaponIsFired += OnWeaponIsFired;

            Cursor.visible = false;
        }

        private void Update()
        {
            HandlePlayerInput();
            HandleMovement();
            HandleRotation();
            HandleRaycast();
        }

        private void LateUpdate()
        {
            HandleCameraRotation();
        }

        private void HandlePlayerInput()
        {
            if (Input.GetMouseButtonDown(0) && isAiming)
            {
                if (!isPullingTheTrigger) PullTheTrigger();
            }

            if (Input.GetMouseButtonUp(0) && isAiming)
            {
                if (isPullingTheTrigger) ReleaseTrigger();
            }

            if (Input.GetMouseButtonDown(1))
            {
                SetAim(true);
            }

            if (Input.GetMouseButtonUp(1))
            {
                SetAim(false);

                if (isPullingTheTrigger) ReleaseTrigger();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                EventManager.Instance.OnWeaponModeChanged();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                EventManager.Instance.OnBulletTypeChanged();
            }
        }

        private void OnWeaponIsFired(BulletType type)
        {
            PlayBulletImpact();

            //if (type == BulletType.Massive)
            //{
            //    Transform massive = rifle.massiveBullet.transform;
            //    massive.gameObject.SetActive(true);
            //    massive.SetParent(null);
            //    massive.DOJump(hit.point, 2f, 1, Vector3.Distance(massive.position, hit.point) * 0.3f).OnComplete(() => {
            //        massive.GetComponent<MassiveBullet>().Reload();
            //    });
            //}

            // Apply recoil
            // Multiply by 5 if the current weapon mode is set to multiple
            targetPitch -= recoil * Random.Range(0.25f, 1f);
        }

        private void PlayBulletImpact()
        {
            ParticleSystem impact = GameManager.Instance.DirtImpactPool.Pop();

            impact.transform.SetParent(null);
            impact.gameObject.SetActive(true);
            impact.transform.position = hit.point;
            impact.Play();

            StartCoroutine(Delay(3f, () => GameManager.Instance.DirtImpactPool.Push(impact)));
        }

        private void PullTheTrigger()
        {
            isPullingTheTrigger = true;
            EventManager.Instance.OnPlayerPulledTheTrigger();
        }

        private void ReleaseTrigger()
        {
            isPullingTheTrigger = false;
            EventManager.Instance.OnPlayerReleasedTheTrigger();
        }

        private void HandleRotation()
        {
            if (isAiming || !isAiming && input.sqrMagnitude > 0.005f)
            {
                // Look towards the same direction with the camera
                RotateToPosition(cameraFollowTarget.forward);
            }
        }

        private void RotateToPosition(Vector3 lookPos)
        {
            lookPos.y = 0f;

            Vector3 lookTarget = transform.position + lookPos;
            Vector3 aimDirection = (lookTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 25f);
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

        private IEnumerator Delay(float time, System.Action onComplete)
        {
            yield return new WaitForSeconds(time);
            onComplete.Invoke();
        }
    }
}
