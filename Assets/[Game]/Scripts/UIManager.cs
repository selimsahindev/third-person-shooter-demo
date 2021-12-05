using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MainGame
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        [SerializeField] private TextMeshProUGUI weaponModeText;
        [SerializeField] private TextMeshProUGUI bulletTypeText;

        public void SetWeaponModeText(WeaponMode mode)
        {
            weaponModeText.text = $"Weapon Mode: {mode.ToString()}";
        }

        public void SetBulletTypeText(BulletType type)
        {
            bulletTypeText.text = $"Bullet Type: {type.ToString()}";
        }
    }
}
