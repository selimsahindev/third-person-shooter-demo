using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        public ObjectPool<GameObject> ShellPool { get; private set; }
        public ObjectPool<ParticleSystem> DirtImpactPool { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            PopulateObjectPools();
        }

        private void PopulateObjectPools()
        {
            GameObject shellsParent = new GameObject("EmptyShells");
            GameObject shellPrefab = Resources.Load<GameObject>("EmptyShell");

            ShellPool = new ObjectPool<GameObject>(200, () => {
                GameObject newItem = Instantiate(shellPrefab, shellsParent.transform);
                newItem.SetActive(false);
                return newItem;
            }, pushItem => {
                pushItem.transform.SetParent(shellsParent.transform);
                pushItem.SetActive(false);
            });

            GameObject bulletImpactsParent = new GameObject("BulletImpacts");
            ParticleSystem dirtImpactPrefab = Resources.Load<ParticleSystem>("Particles/BulletImpactDirt");

            DirtImpactPool = new ObjectPool<ParticleSystem>(200, () => {
                ParticleSystem newItem = Instantiate(dirtImpactPrefab, bulletImpactsParent.transform);
                newItem.gameObject.SetActive(false);
                return newItem;
            }, pushItem => {
                pushItem.transform.SetParent(bulletImpactsParent.transform);
                pushItem.gameObject.SetActive(false);
            });
        }
    }
}
