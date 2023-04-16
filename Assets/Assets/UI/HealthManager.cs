using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private Image healthB;
    private PlayerController playerH;
    
    void Start()
    {
        healthB = GetComponent<Image>();
        playerH = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        healthB.fillAmount = playerH.health;
    }
}
