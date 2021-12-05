using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class EventManager : SingletonBehaviour<EventManager>
    {
        public Action onPlayerStartedAiming;
        public Action onPlayerStoppedAiming;
        public Action onPlayerPulledTheTrigger;
        public Action onPlayerReleasedTheTrigger;
        public Action onWeaponModeChanged;
        public Action onWeaponIsFired;

        public void OnPlayerStartedAiming()
        {
            onPlayerStartedAiming?.Invoke();
        }

        public void OnPlayerStoppedAiming()
        {
            onPlayerStoppedAiming?.Invoke();
        }

        public void OnPlayerPulledTheTrigger()
        {
            onPlayerPulledTheTrigger?.Invoke();
        }

        public void OnPlayerReleasedTheTrigger()
        {
            onPlayerReleasedTheTrigger?.Invoke();
        }

        public void OnWeaponModeChanged()
        {
            onWeaponModeChanged?.Invoke();
        }

        public void OnWeaponIsFired()
        {
            onWeaponIsFired?.Invoke();
        }
    }
}
