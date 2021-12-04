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
    }
}
