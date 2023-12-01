using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPosition : MonoBehaviour
{
    [SerializeField] private GameObject _item;

    private void LateUpdate()
    {
        _item.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
