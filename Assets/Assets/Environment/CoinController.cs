using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public CoinScript coinManag;
    public float rotSpeed;
    private float initialRot;
    private bool coinPickedUp;


    void Start()
    {
        coinManag = FindObjectOfType<CoinScript>();

        rotSpeed = 0.5f;
        initialRot = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,  initialRot += rotSpeed, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.isTrigger == false && coinPickedUp == false)
        {
            coinManag.coinAm++;
            coinPickedUp = true;
            Destroy(gameObject);
        }
    }
}
