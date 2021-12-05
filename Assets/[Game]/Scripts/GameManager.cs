using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        public ObjectPool<GameObject> ShellPool { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            PopulateObjectPool();
        }

        private void PopulateObjectPool()
        {
            GameObject shellsParent = new GameObject("EmptyShells");
            GameObject shellPrefab = Resources.Load<GameObject>("EmptyShell");

            ShellPool = new ObjectPool<GameObject>(100, () => {
                GameObject newItem = Instantiate(shellPrefab, shellsParent.transform);
                newItem.SetActive(false);
                return newItem;
            }, pushItem => {
                pushItem.transform.SetParent(shellsParent.transform);
                pushItem.SetActive(false);
            });
        }
    }
}
