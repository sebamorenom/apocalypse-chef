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

    public static int Hurt0 = Animator.StringToHash("Hurt0");
    public static int Hurt1 = Animator.StringToHash("Hurt1");
    public static int Hurt2 = Animator.StringToHash("Hurt2");

    public static int Die = Animator.StringToHash("Die");
}

public class ZombieAI : MonoBehaviour
{
    [Header("AI Parameters")] [SerializeField]
    public float reTargettingProbability = 0f;

    [Header("Stats")] [SerializeField] public float attackRange;
    [SerializeField] public float attackDamage;

    [Header("Thresholds")] [SerializeField]
    public float minHitForceThreshold;

    [Header("Utilities")] [SerializeField] public GameInfo gameInfo;

    private float moveTime, scratchTime, attackTime, danceTime, hurt0Time, hurt1Time, hurt2Time, dieTime;

    [Header("Miscellaneous")] private float timeBeforeDisappearance;

    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private Health _ownHealth;
    private Animator _animator;
    private Collider _coll;

    public Transform objective;
    public Health objectiveHealth;

    private Transform _distraction;
    public bool distracted;
    private bool _dancing;
    private bool _hurt;
    private bool _isDying;


    private Rigidbody _rb;
    private int _randomInt;
    private AiStates _aiStates;
    private AnimationClip[] _animClips;

    private ContactPoint[] _contactPoints;
    private Rigidbody _collidedRb;

    private IEnumerator behaviourCoroutine;

    private int currentHurtAnimation = 0;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _ownHealth = GetComponent<Health>();
        _coll = GetComponent<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = true;
        _rb = GetComponent<Rigidbody>();
        behaviourCoroutine = Behaviour();
        if (TryGetComponent<Animator>(out _animator))
        {
            GetAnimationTimes();
        }

        _contactPoints = new ContactPoint[5];
        Initiate();
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
            StopAllCoroutines();
            Die();
        }
    }

    private IEnumerator Behaviour()
    {
        while (!_ownHealth.dead)
        {
            if (objective != null)
            {
                yield return NextAction();
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
    }

    private IEnumerator NextAction()
    {
        if (_hurt)
        {
            return PlayHurtAnimation();
        }

        if (_dancing)
        {
            return Dance();
        }

        if (!distracted && Random.value > 1 - reTargettingProbability)
        {
            return Think();
        }

        if ((objective.position - _transform.position).magnitude > attackRange)
        {
            return Move();
        }

        if (((objective.position - _transform.position).magnitude <= attackRange))
        {
            return Attack();
        }

        return null;
    }


    private IEnumerator Move()
    {
        if (_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = false;
        }

        _navMeshAgent.destination = objective.position;
        _animator?.SetTrigger(AiStates.Move);
        yield return new WaitForSeconds(moveTime - 0.2f);
    }

    private IEnumerator Think()
    {
        if (!_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = true;
        }

        _animator.SetTrigger(AiStates.Scratch);
        RandomObjective();
        yield return new WaitForSeconds(scratchTime);
    }

    private IEnumerator Attack()
    {
        if (!_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = true;
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
        if (!_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = true;
        }

        _animator?.SetTrigger(AiStates.Dance);
        yield return new WaitForSeconds(danceTime);
        _dancing = false;
    }

    private IEnumerator PlayHurtAnimation()
    {
        if (!_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = true;
        }

        currentHurtAnimation = Random.Range(0, 2);
        switch (currentHurtAnimation)
        {
            case 0:
                _animator?.Play(AiStates.Hurt0);
                yield return new WaitForSeconds(hurt0Time);
                break;
            case 1:
                _animator?.Play(AiStates.Hurt1);
                yield return new WaitForSeconds(hurt1Time);
                break;
            case 2:
                _animator?.Play(AiStates.Hurt2);
                yield return new WaitForSeconds(hurt2Time);
                break;
        }

        _hurt = false;
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
                case "Hurt0":
                    hurt0Time = animClip.length;
                    break;
                case "Hurt1":
                    hurt1Time = animClip.length;
                    break;
                case "Hurt2":
                    hurt2Time = animClip.length;
                    break;
                case "Die":
                    dieTime = animClip.length;
                    break;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        collision.GetContacts(_contactPoints);
        foreach (var contact in _contactPoints)
        {
            if ((contact.thisCollider != null &&
                 contact.thisCollider.gameObject.layer == LayerMask.NameToLayer("Zombie") &&
                 contact.otherCollider != null)
               )
            {
                _collidedRb = contact.otherCollider.attachedRigidbody;
                if (_collidedRb != null && _collidedRb.velocity.magnitude >= minHitForceThreshold)
                {
                    _ownHealth.Hurt(_collidedRb.velocity.magnitude);
                    _hurt = true;
                }
            }
        }
    }

    private void Die()
    {
        if (!_isDying)
        {
            _isDying = true;
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
            _animator?.SetTrigger(AiStates.Die);
            _coll.enabled = false;
            _rb.isKinematic = true;
            Destroy(gameObject, dieTime + 3);
        }
    }
}