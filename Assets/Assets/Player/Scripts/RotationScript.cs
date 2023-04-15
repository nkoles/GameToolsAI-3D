using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    private float rot;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0 , rot, 0);

        rot += 2;
    }
}
