using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    public float speed;

    //Health Variables
    public float health;
    private Collider playerHB;
    public bool hit;
    public int hitAm;

    void Start()
    {
        playerHB = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement Script
        float xDir = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float zDir = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(xDir, 0, zDir);

        //Damage Calculation
        health = 1 - 0.33f*hitAm;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && other.isTrigger == false)
        {
            hit = true;
            hitAm ++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy" && other.isTrigger == false)
        {
            hit = false;
        }
    }
}
