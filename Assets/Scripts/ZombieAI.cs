using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public struct AiStates
{
    public static int Move = Animator.StringToHash("Move");
    public static int Attack = Animator.StringToHash("Attack");
    public static int Scratch = Animator.StringToHash("Scratch");
    public static int Dance = Animator.StringToHash("Dance");

    public static int StandUpFaceUp = Animator.StringToHash("StandUp_FaceUp");
    public static int StandUpFaceDown = Animator.StringToHash("StandUp_FaceDown");
}

public class ZombieAI : MonoBehaviour
{
    [SerializeField] public float reTargettingProbability = 0f;
    [SerializeField] public float thinkingTime;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackDamage;
    [SerializeField] public float timeBetweenActions = 0.5f;
    [SerializeField] public float forceToRagdoll;
    [SerializeField] public Collider torsoCollider;
    private Collider _animCollider;
    private List<Collider> _ragdollColliders;
    [Header("Utilities")] [SerializeField] public GameInfo gameInfo;

    private float moveTime, scratchTime, attackTime, danceTime, standUpFaceUpTime, standUpFaceDownTime;


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

    private ContactPoint[] _contactPoints;
    private Rigidbody _collidedRb;

    private bool ragdollMode;
    private float _startRagdollTime;
    private float _currentRagdollTime;
    public float _ragdollDuration;

    private IEnumerator behaviourCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _ownHealth = GetComponent<Health>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _animCollider = GetComponent<Collider>();
        _ragdollColliders = new List<Collider>();
        behaviourCoroutine = Behaviour();
        GetRagdollColliders();
        if (TryGetComponent<Animator>(out _animator))
        {
            GetAnimationTimes();
        }

        _contactPoints = new ContactPoint[5];
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;
        Invoke(nameof(Initiate), 0.5f);
    }

    public void Initiate()
    {
        RandomObjective();

        StartCoroutine(behaviourCoroutine);
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

    private void FixedUpdate()
    {
        if (_ownHealth.dead)
        {
            StopCoroutine(behaviourCoroutine);
            DieRagdoll();
        }
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
                        yield return StartCoroutine(Dance());
                    }

                    if (!distracted && Random.value > 1 - reTargettingProbability)
                    {
                        yield return StartCoroutine(Think());
                    }

                    if ((objective.position - _transform.position).magnitude > attackRange)
                    {
                        yield return StartCoroutine(Move());
                    }
                    else
                    {
                        yield return StartCoroutine(Attack());
                    }
                }
                else
                {
                    if (distracted)
                    {
                        distracted = false;
                    }

                    yield return StartCoroutine(Think());
                }
            }

            yield return new WaitForSeconds(timeBetweenActions);
        }
    }


    private IEnumerator Move()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = objective.position;
        _animator?.SetTrigger(AiStates.Move);
        yield return new WaitForSeconds(moveTime - 0.2f);
    }

    private IEnumerator Think()
    {
        if (_navMeshAgent.enabled)
        {
            if (!_navMeshAgent.isStopped) _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
        }

        _animator.SetTrigger(AiStates.Scratch);
        RandomObjective();
        yield return new WaitForSeconds(scratchTime);
    }

    private IEnumerator Attack()
    {
        if (_navMeshAgent.enabled)
        {
            if (!_navMeshAgent.isStopped) _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
        }

        _animator?.SetTrigger(AiStates.Attack);
        yield return new WaitForSeconds(attackTime);
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

    private IEnumerator Dance()
    {
        if (_navMeshAgent.enabled)
        {
            if (!_navMeshAgent.isStopped) _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
        }

        _animator?.SetTrigger(AiStates.Dance);
        yield return new WaitForSeconds(danceTime);
    }

    private void GetAnimationTimes()
    {
        _animClips = _animator.runtimeAnimatorController.animationClips;
        foreach (var animClip in _animClips)
        {
            switch (animClip.name)
            {
                case "Move":
                    moveTime = animClip.length;
                    break;
                case "Scratch":
                    scratchTime = animClip.length;
                    break;
                case "Attack":
                    attackTime = animClip.length;
                    break;
                case "Dance":
                    danceTime = animClip.length;
                    break;
                case "StandUp_FaceUp":
                    standUpFaceUpTime = animClip.length;
                    break;
                case "StandUp_FaceDown":
                    standUpFaceDownTime = animClip.length;
                    break;
            }
        }
    }

    private void GetRagdollColliders()
    {
        foreach (var coll in GetComponentsInChildren<Collider>())
        {
            if (coll != _animCollider)
            {
                _ragdollColliders.Add(coll);
            }
        }
    }

    private Collider GetRagdollColliderClosest(Vector3 point)
    {
        float distance = float.MaxValue;
        Collider closestColl = null;
        foreach (var rColl in _ragdollColliders)
        {
            if ((point - rColl.ClosestPointOnBounds(point)).sqrMagnitude < distance)
            {
                distance = (point - rColl.ClosestPointOnBounds(point)).magnitude;
                closestColl = rColl;
            }
        }

        return closestColl;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.GetContacts(_contactPoints);
        foreach (var contact in _contactPoints)
        {
            if ((contact.thisCollider != null &&
                 contact.thisCollider.gameObject.layer == LayerMask.NameToLayer("ZombieCollider") &&
                 contact.otherCollider != null)
               )
            {
                _collidedRb = contact.otherCollider.attachedRigidbody;
                if (_collidedRb != null && _collidedRb.velocity.magnitude >= forceToRagdoll)
                {
                    _ownHealth.Hurt(_collidedRb.velocity.magnitude);
                    StartCoroutine(ToRagdoll());
                    GetRagdollColliderClosest(contact.point).attachedRigidbody.AddForceAtPosition(_collidedRb.velocity,
                        contact.point,
                        ForceMode.Impulse);
                }
            }
        }
    }

    public void StartRagdoll()
    {
        StartCoroutine(ToRagdoll());
    }

    private IEnumerator ToRagdoll()
    {
        ragdollMode = true;
        //_ragdollCollider.enabled = false;
        _rb.velocity = Vector3.zero;
        _startRagdollTime = _currentRagdollTime = Time.fixedTime;
        _navMeshAgent.enabled = false;
        _animator.enabled = false;
        _rb.isKinematic = false;

        while (_currentRagdollTime < _startRagdollTime + _ragdollDuration)
        {
            _currentRagdollTime += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        _rb.isKinematic = true;
        _navMeshAgent.enabled = true;
        _animator.enabled = true;
        _rb.velocity = Vector3.zero;
        //_ragdollCollider.enabled = true;
        yield return StartCoroutine(StandUp());
        ragdollMode = false;
    }

    private void DieRagdoll()
    {
        Debug.Log("Muerto");
        //ragdollMode = true;
        //_ragdollCollider.enabled = false;
        _animator.enabled = false;
        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        //_navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;
    }

    private IEnumerator StandUp()
    {
        if (Vector3.Dot(torsoCollider.transform.forward, Vector3.up) >= 0)
        {
            _animator.Play(AiStates.StandUpFaceUp);
            yield return new WaitForSeconds(standUpFaceUpTime);
        }
        else
        {
            _animator.Play(AiStates.StandUpFaceDown);
            yield return new WaitForSeconds(standUpFaceDownTime);
        }
    }
}