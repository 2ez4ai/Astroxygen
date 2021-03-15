using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent m_navMeshAgent;

    [SerializeField]
    Transform m_patrolDestinationTransform;

    bool control = true;
    float timerControl = -1.5f;
    Vector3 m_patrolOrigin;
    Vector3 m_patrolDestination;
    Vector3 m_lastDestination;

    // Start is called before the first frame update
    void Start()
    {
        m_navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_navMeshAgent.speed = Random.RandomRange(3, 12);
        m_patrolOrigin = transform.position;        // original position
        m_patrolDestination = m_patrolDestinationTransform.position;
        m_navMeshAgent.SetDestination(m_patrolDestination);
    }

    // Update is called once per frame
    void Update()
    {
        CallPatrol();
    }

    void CallPatrol()
    {
        timerControl += Time.deltaTime;
        if (timerControl > 15.0f && timerControl < 20.0f && control)
        {
            control = false;
            Still();
        }
        if (timerControl > 20.0f)
        {
            control = true;
            timerControl = -1.5f;
            m_navMeshAgent.SetDestination(m_lastDestination);
        }
        if (control)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // Swap Destination between Origin and Destination, remove Y coordinates
        if (Vector3.Distance(transform.position, m_patrolDestination) < 0.5f)
        {
            m_navMeshAgent.speed = Random.RandomRange(3, 12);
            m_navMeshAgent.SetDestination(m_patrolOrigin);
            m_lastDestination = m_patrolOrigin;
        }
        else if (Vector3.Distance(transform.position, m_patrolOrigin) < 0.5f)
        {
            m_navMeshAgent.speed = Random.RandomRange(3, 12);
            m_navMeshAgent.SetDestination(m_patrolDestination);
            m_lastDestination = m_patrolDestination;
        }
    }

    void Still()
    {
        m_navMeshAgent.SetDestination(transform.position);
    }
}
