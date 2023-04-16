using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Mathf;

public class BeholderAI : MonoBehaviour
{
    private NavMeshAgent aiNavMesh;
    private Collider triggerCol;
    public GameObject player;

    public float rotSpeed;
    public Vector2 speedMinMax;
    public float speedUpRange;

    private enum State
    {
        LookFor,
        Hunt
    }

    private State currentState;

    void Start()
    {
        //Getting AI Nav Component and Trigger Collider
        aiNavMesh = GetComponent<NavMeshAgent>();
        triggerCol = player.GetComponent<Collider>();
    }

    void Update()
    {
        /*if(playerCheck == true)
        {
            aiNavMesh.SetDestination(player.transform.position);
        }*/

        switch (currentState)
        {
            case State.LookFor:
                Spin();
                break;
            case State.Hunt:
                aiNavMesh.SetDestination(player.transform.position);

                if (Vector3.Distance(player.transform.position, transform.position) < speedUpRange)
                {
                    aiNavMesh.speed = Lerp(speedMinMax.x, speedMinMax.y, Vector3.Distance(player.transform.position, transform.position) / speedUpRange);
                }
                else
                {
                    aiNavMesh.speed = speedMinMax.x;
                }
                break;
        }
    }

    void Spin()
    {
        transform.Rotate(Vector3.up* rotSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider triggerCol)
    {
        if (triggerCol.gameObject.tag == "Player")
        {
            currentState = State.Hunt;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
