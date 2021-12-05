using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class RifleController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem fireParticle;
        [SerializeField] private Transform shellExitPoint;

        private Cinemachine.CinemachineImpulseSource impulseSource;

        private void Awake()
        {
            impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();

            EventManager.Instance.onPlayerPulledTheTrigger += Fire;
        }

        private void Fire()
        {
            DischargeEmptyShell();
            PlayFireParticle();

            // Generate impulse for virtual cameras in the scene
            impulseSource.GenerateImpulse(CameraManager.Instance.mainCamera.transform.forward);
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
            shellRb.transform.localScale = Vector3.one * 0.075f;
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
