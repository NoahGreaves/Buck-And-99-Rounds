using UnityEngine.AI;
using UnityEngine;

public class Enemy_Basic : Enemy
{
    [Header("Movement")]
    [SerializeField] private float _targetDistanceThreshold = 0.5f;

    [Header("Damage")]
    [SerializeField] private float _damageOnContact = 10f;

    private void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    protected override void Move()
    {
        base.Move();

        // Get Player Vehicle from FOV and set destination
        GetTargets(Fov.VisibleTargets);
        if (targetPlayer == null)
            return;
        RotateToLookAt(targetPlayer);

        agent.SetDestination(targetPlayer.transform.position);


        // if enemy is moving between multiple points
        //if (!agent.pathPending && agent.remainingDistance < _targetDistanceThreshold)
        //    ChangeDestination();
    }

    private void ChangeDestination()
    {
    }
}
