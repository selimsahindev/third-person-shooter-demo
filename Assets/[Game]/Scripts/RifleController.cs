using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainGame
{
    public enum WeaponMode
    {
        Auto, SemiAuto, Multiple
    }

    public class RifleController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem fireParticle;
        [SerializeField] private Transform shellExitPoint;
        [Tooltip("Cooldown between two consecutive shots.")]
        [SerializeField] private float fireRate = 0.1f;
        [SerializeField] private WeaponMode mode = WeaponMode.Auto;

        private Cinemachine.CinemachineImpulseSource impulseSource;
        private Coroutine shootingCoroutine;

        private void Awake()
        {
            impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();

            EventManager.Instance.onPlayerPulledTheTrigger += StartFire;
            EventManager.Instance.onPlayerReleasedTheTrigger += StopFire;
            EventManager.Instance.onWeaponModeChanged += ChangeMode;
        }

        private void ChangeMode()
        {
            int maxEnumValue = Convert.ToInt32(Enum.GetValues(typeof(WeaponMode)).Cast<WeaponMode>().Last());
            mode = (WeaponMode)((Convert.ToInt32(mode) + 1) % maxEnumValue);
        }

        private void StartFire()
        {
            if (mode == WeaponMode.Auto && shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(FireCoroutine());
            }
            if (mode == WeaponMode.SemiAuto)
            {
                Fire();
            }
        }

        private void StopFire()
        {
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }

        private IEnumerator FireCoroutine()
        {
            while (true)
            {
                Fire();
                yield return new WaitForSeconds(fireRate);
            }
        }

        private void Fire()
        {
            DischargeEmptyShell();
            PlayFireParticle();

            // Generate impulse for virtual cameras in the scene
            impulseSource.GenerateImpulse(CameraManager.Instance.PlayerAimCamera.transform.forward);

            EventManager.Instance.OnWeaponIsFired();
        }

        private void PlayFireParticle()
        {
            fireParticle.transform.localRotation = Quaternion.Euler(Random.Range(0f, 45f), -90f, 0f);
            fireParticle.Play();
        }

        private void DischargeEmptyShell()
        {
            Rigidbody shellRb = GameManager.Instance.ShellPool.Pop().GetComponent<Rigidbody>();

            shellRb.transform.SetParent(null);
            shellRb.gameObject.SetActive(true);
            shellRb.transform.localScale = Vector3.one * 0.054f;
            shellRb.transform.position = shellExitPoint.position;
            shellRb.transform.rotation = shellExitPoint.rotation * Quaternion.Euler(90f, 0f, 0f);

            shellRb.AddForce(transform.right * 2f + transform.up + transform.forward * 1.4f, ForceMode.VelocityChange);
            shellRb.AddTorque(new Vector3(Random.Range(0f, 1f), 0f, Random.Range(-1f, 1f)));

            StartCoroutine(Delay(10f, () => GameManager.Instance.ShellPool.Push(shellRb.gameObject)));
        }

        private void OnDestroy()
        {
            EventManager.Instance.onPlayerPulledTheTrigger -= Fire;
        }

        private IEnumerator Delay(float time, System.Action onComplete)
        {
            yield return new WaitForSeconds(time);
            onComplete.Invoke();
        }
    }
}
