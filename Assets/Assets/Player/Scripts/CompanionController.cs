using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionController : MonoBehaviour
{
    private NavMeshAgent companionAgent;
    public GameObject refPoint;
    private GameObject currentTarget;
    public GameObject flameParticles;

    private bool enemyCheck;
    private bool enemyDestroyed;

    private List<GameObject> numEnemies = new List<GameObject>();

    private enum State
    {
        AttackEnemy,
        Idle,
    }

    private State currentState;

    void Start()
    {
        companionAgent = GetComponent<NavMeshAgent>();

        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:

                SetNavTarget(refPoint);

                if(enemyCheck == true)
                {
                    currentState = State.AttackEnemy;
                }

                break;

            case State.AttackEnemy:

                GameObject closestEnemy = closestEnemyDist();
                float dist = Vector3.Distance(transform.position, closestEnemy.transform.position);

                SetNavTarget(closestEnemy);

                if(dist <= 1.5)
                {
                    Instantiate(flameParticles, transform);
                    Destroy(closestEnemy);
                    numEnemies.Clear();
                }

                if(numEnemies.Count == 0)
                {
                    enemyCheck = false;
                }

                if(enemyCheck == false)
                {
                    currentState = State.Idle;
                }

                break;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && other.isTrigger == false)
        {
            numEnemies.Add(other.gameObject);

            enemyCheck = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy" && other.isTrigger == false)
        {
            numEnemies.Remove(other.gameObject);

            if(numEnemies.Count == 0)
            {
                enemyCheck = false;
            }
        }
    }

    private GameObject closestEnemyDist()
    {
        float closestDist = float.MaxValue;
        int closestInd = 0;
        
        for (int i = 0; i < numEnemies.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, numEnemies[i].transform.position);

            if(dist<closestDist)
            {
                dist = closestDist;
                closestInd = i;
            }
        }

        return numEnemies[closestInd];
    }

    void SetNavTarget(GameObject target)
    {
        companionAgent.SetDestination(target.transform.position);
        currentTarget = target;
    }
}
