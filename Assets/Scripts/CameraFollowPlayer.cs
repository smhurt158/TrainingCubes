using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = t.position;
        transform.rotation = t.rotation;
    }
}
