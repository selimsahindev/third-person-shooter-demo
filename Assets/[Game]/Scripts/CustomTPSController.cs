using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class CustomTPSController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        private void Update()
        {
            HandleMovement();

            if (Input.GetMouseButtonDown(1))
            {
                EventManager.Instance.OnPlayerStartedAiming();
            }
            if (Input.GetMouseButtonUp(1))
            {
                EventManager.Instance.OnPlayerStoppedAiming();
            }
        }

        private void HandleMovement()
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            transform.position = Vector3.Lerp(transform.position, transform.position + input * moveSpeed, Time.deltaTime);
        }
    }
}
