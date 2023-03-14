using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public struct AiStates
{
    public static int Move = Animator.StringToHash("Move");
    public static int Attack = Animator.StringToHash("Attack");
    public static int Think = Animator.StringToHash("Think");
    public static int Dance = Animator.StringToHash("Dance");
}

public class ZombieAI : MonoBehaviour
{
    [SerializeField] public float reTargettingProbability = 0f;
    [SerializeField] public float thinkingTime;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackDamage;
    [SerializeField] public float timeBetweenActions = 0.5f;
    [SerializeField] public float forceToRagdoll;
    private Collider _ragdollCollider;
    [Header("Utilities")] [SerializeField] public GameInfo gameInfo;

    private float scratchTime, attackTime, danceTime;


    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private Health _ownHealth;
    private Animator _animator;

    public Transform objective;
    public Health objectiveHealth;

    private Transform _distraction;
    public bool distracted;
    private bool _dancing;


    private Rigidbody _rb;
    private int _randomInt;
    private AiStates _aiStates;
    private AnimationClip[] _animClips;

    private bool ragdollMode;
    private float _startRagdollTime;
    private float _currentRagdollTime;
    public float _ragdollDuration;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _ownHealth = GetComponent<Health>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _ragdollCollider = GetComponent<BoxCollider>();
        if (TryGetComponent<Animator>(out _animator))
        {
            GetAnimationTimes();
        }

        _navMeshAgent.isStopped = true;
        Invoke(nameof(Initiate), 0.5f);
    }

    public void Initiate()
    {
        RandomObjective();
        StartCoroutine(Behaviour());
    }


    private void RandomObjective()
    {
        _randomInt = Random.Range(0, gameInfo.objectivesTransform.Count);
        if (gameInfo.objectivesTransform.Count == 1)
        {
            _randomInt = 0;
        }

        objective = gameInfo.objectivesTransform[_randomInt];
        objectiveHealth = gameInfo.objectivesHealth[_randomInt];
    }

    private IEnumerator Behaviour()
    {
        while (!_ownHealth.dead)
        {
            if (!ragdollMode)
            {
                if (objective != null)
                {
                    if (_dancing)
                    {
                        _dancing = false;
                        Dance();
                        yield return new WaitForSeconds(danceTime);
                    }

                    if (!distracted && Random.value > 1 - reTargettingProbability)
                    {
                        Think();
                        yield return new WaitForSeconds(scratchTime);
                    }

                    if ((objective.position - _transform.position).magnitude > attackRange)
                    {
                        Move();
                    }
                    else
                    {
                        Attack();
                        yield return new WaitForSeconds(attackTime);
                    }
                }
                else
                {
                    if (distracted)
                    {
                        distracted = false;
                    }

                    Think();
                    yield return new WaitForSeconds(scratchTime);
                }
            }

            yield return new WaitForSeconds(timeBetweenActions);
        }

        _navMeshAgent.isStopped = true;

        Debug.Log("StoppedThinking");
    }


    private void Move()
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = objective.position;
        _animator?.SetTrigger(AiStates.Move);
    }

    private void Think()
    {
        _navMeshAgent.isStopped = true;
        _animator.SetTrigger(AiStates.Think);
        RandomObjective();
    }

    private void Attack()
    {
        _navMeshAgent.isStopped = true;
        _animator?.SetTrigger(AiStates.Attack);
    }

    private void HurtObjective()
    {
        objectiveHealth.Hurt(attackDamage);
        if (objectiveHealth.dead)
        {
            gameInfo.RemoveFromArrays(objective, objectiveHealth);
            objective = null;
            objectiveHealth = null;
        }
    }

    public void Distract(Transform distractionTransform)
    {
        objective = distractionTransform;
        distracted = true;
    }

    private void MakeDance()
    {
        _dancing = true;
    }

    private void Dance()
    {
        _navMeshAgent.isStopped = true;
        _animator?.SetTrigger(AiStates.Dance);
    }

    public void GetAnimationTimes()
    {
        _animClips = _animator.runtimeAnimatorController.animationClips;
        foreach (var animClip in _animClips)
        {
            switch (animClip.name)
            {
                case "Scratch":
                    scratchTime = animClip.length;
                    break;
                case "Attack":
                    attackTime = animClip.length;
                    break;
                case "Dance":
                    danceTime = animClip.length;
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.impulse.magnitude);
        if (collision.impulse.magnitude > forceToRagdoll)
        {
            StartCoroutine(ToRagdoll());
        }
    }

    private IEnumerator ToRagdoll()
    {
        ragdollMode = true;
        _ragdollCollider.enabled = false;
        _rb.velocity = Vector3.zero;
        _startRagdollTime = _currentRagdollTime = Time.fixedTime;
        _animator.enabled = false;
        _rb.isKinematic = false;
        Debug.Log(_rb.velocity.magnitude);
        while (_currentRagdollTime < _startRagdollTime + _ragdollDuration)
        {
            _currentRagdollTime += Time.fixedDeltaTime;
            yield return null;
        }

        _rb.isKinematic = true;
        _animator.enabled = true;
        _rb.velocity = Vector3.zero;
        _ragdollCollider.enabled = true;
        ragdollMode = false;
    }
}