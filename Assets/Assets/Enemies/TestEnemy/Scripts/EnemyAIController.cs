using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    private NavMeshAgent aiNavMesh;
    private Collider triggerCol;
    public GameObject player;
    private bool playerCheck;

    void Start()
    {
        //Getting AI Nav Component and Trigger Collider
        aiNavMesh = GetComponent<NavMeshAgent>();
        triggerCol = player.GetComponent<Collider>();

        playerCheck = false;
    }

    void Update()
    {
        if(playerCheck == true)
        {
            aiNavMesh.SetDestination(player.transform.position);
        }
    }

    void OnTriggerEnter(Collider triggerCol)
    {
        if(triggerCol.gameObject.tag == "Player")
        {
            playerCheck = true;
        }
    }
}
