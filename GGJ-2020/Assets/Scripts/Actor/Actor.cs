﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{

    [HideInInspector] public NavMeshAgent agent;
    // [HideInInspector] public Animator animator;
    [HideInInspector] public Damageable damageable;
    [HideInInspector] public Damageable damageableTarget;

    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimationEventListener animationEvent;

    [HideInInspector] public ActorVisualHandler visualHandler;
    [HideInInspector] public Coroutine currentTask;

    public bool isHover = false;
    bool isResource;




    void Awake()
    {
        print("chec here " + visualHandler);
        damageable = gameObject.GetComponent<Damageable>();
        animator = GetComponentInChildren<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        animationEvent = gameObject.GetComponentInChildren<AnimationEventListener>();
        animationEvent.attackEvent.AddListener(Attack);
        visualHandler = gameObject.GetComponent<ActorVisualHandler>();
        isResource = gameObject.GetComponent<Resource>() ? true : false;
    }

    public void Update()
    {
        animator.SetFloat("Speed", Mathf.Clamp(agent.velocity.magnitude, 0, 1));
    }

    public void SetDestination(Vector3 destination)
    {
        if (agent == null)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
        agent.destination = destination;
    }

    public WaitUntil WaitForNavMesh()
    {
        return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }

    void Attack()
    {
        if (damageableTarget)
            damageableTarget.Hit(10);
    }

    public void AttackTarget(Damageable target)
    {
        StopTask();
        damageableTarget = target;

        currentTask = StartCoroutine(StartAttack());

        IEnumerator StartAttack()
        {
            while (damageableTarget)
            {
                SetDestination(damageableTarget.transform.position);
                yield return WaitForNavMesh();
                while (damageableTarget && Vector3.Distance(damageableTarget.transform.position, transform.position) < 4f)
                {
                    yield return new WaitForSeconds(1);
                    if (damageableTarget)
                    {
                        animator.SetTrigger("Attack");
                    }
                }

            }

            currentTask = null;
        }
    }

    public virtual void StopTask()
    {
        damageableTarget = null;
        if (currentTask != null)
            StopCoroutine(currentTask);
    }

    private void OnMouseEnter()
    {
        isHover = true;
    }
    private void OnMouseExit()
    {
        isHover = false;
    }
}
