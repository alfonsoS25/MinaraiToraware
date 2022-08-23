using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    [SerializeField]
    private GameObject Follow;

    [SerializeField]
    private float OffsetX;
    [SerializeField]
    private float OffsetY;
    // Update is called once per frame
    [SerializeField]
    private void FixedUpdate()
    {
        transform.position = new Vector3(Follow.transform.position.x, Follow.transform.position.y+ -OffsetY, Follow.transform.position.z - OffsetX);//Follow.transform.position;
    }
}
