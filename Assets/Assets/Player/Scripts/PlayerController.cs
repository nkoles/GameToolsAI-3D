using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   //Movement Variables
   public float speed;

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float zDir = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(xDir, 0, zDir);
    }
}
