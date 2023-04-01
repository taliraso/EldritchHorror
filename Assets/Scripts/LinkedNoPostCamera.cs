using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedNoPostCamera : MonoBehaviour
{
    private Camera noPostCamera;
    private bool linked;

    private void Awake()
    {
        noPostCamera = transform.GetChild(0).GetComponent<Camera>();
    }

    private void OnEnable()
    {
        if (noPostCamera != null)
        {
            noPostCamera.gameObject.SetActive(true);
            linked = true;
        }
    }

    private void OnDisable()
    {
        if (noPostCamera != null)
        {
            noPostCamera.gameObject.SetActive(false);
            linked = false;
        }
    }

    private void LateUpdate()
    {
        if (linked)
        {
            if (noPostCamera != null)
            {
                noPostCamera.transform.position = this.transform.position;
            }
        }
    }
}
