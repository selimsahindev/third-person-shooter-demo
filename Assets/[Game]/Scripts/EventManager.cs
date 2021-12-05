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
        public Action onBulletTypeChanged;
        public Action<BulletType> onWeaponIsFired;

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

        public void OnBulletTypeChanged()
        {
            onBulletTypeChanged?.Invoke();
        }

        public void OnWeaponIsFired(BulletType type = BulletType.Standard)
        {
            onWeaponIsFired?.Invoke(type);
        }
    }
}
