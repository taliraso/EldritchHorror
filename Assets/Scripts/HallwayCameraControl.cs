using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayCameraControl : MonoBehaviour
{
    public GameObject player;
    
    void Update()
    {
        float newx = player.transform.position.x;
        newx = Mathf.Clamp(newx, -35.5f, 35.5f);
        transform.position = new Vector3(newx, transform.position.y, transform.position.z);
    }
}
