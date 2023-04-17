using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CoinScript : MonoBehaviour
{
    public int coinAm;
    public TMP_Text coinDisp;
    private  PlayerController playerC;

    void Start()
    {
        playerC = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(coinAm == 5 || playerC.hitAm == 3 || playerC.health == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        coinDisp.text = coinAm.ToString();
    }
}
