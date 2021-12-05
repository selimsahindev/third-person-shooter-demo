using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassiveBullet : MonoBehaviour
{
    private Transform originalParent;
    private Vector3 startPos;

    private void Awake()
    {
        originalParent = transform.parent;
        startPos = transform.localPosition;
    }

    public void Reload()
    {
        transform.SetParent(originalParent);
        transform.localPosition = startPos;
        gameObject.SetActive(false);
    }
}
