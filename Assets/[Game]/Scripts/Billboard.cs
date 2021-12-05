using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace MainGame
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI bulletCountText;
        [SerializeField] private TextMeshProUGUI labelText;

        private int bulletCount = 0;

        private void Awake()
        {
            EventManager.Instance.onWeaponIsFired += UpdateBulletCount;

            //labelText.transform.DOLocalRotate();
        }

        private void UpdateBulletCount(BulletType type)
        {
            bulletCount++;
            bulletCountText.text = bulletCount.ToString();
        }
    }
}
