using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public struct AiStates
{
    public static int Move = Animator.StringToHash("Move");
    public static int Attack = Animator.StringToHash("Attack");
    public static int Think = Animator.StringToHash("Think");
}

public class ZombieAI : MonoBehaviour
{
    [SerializeField] public float reTargettingProbability = 0f;
    [SerializeField] public float thinkingTime;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackDamage;
    [SerializeField] public float timeBetweenActions;
    [Header("Utilities")] [SerializeField] public GameInfo gameInfo;


    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private Health _ownHealth;
    private Animator _animator;

    private Transform objective;
    private Health objectiveHealth;

    private Transform _distraction;
    public bool distracted;

    private int _randomInt;
    private AiStates _aiStates;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _ownHealth = GetComponent<Health>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void Initiate()
    {
        RandomObjective();
        StartCoroutine(Think());
    }


    private void RandomObjective()
    {
        _randomInt = Random.Range(0, gameInfo.objectivesTransform.Length);
        objective = gameInfo.objectivesTransform[_randomInt];
        objectiveHealth = gameInfo.objectivesHealth[_randomInt];
        _animator.SetTrigger(AiStates.Think);
    }

    private IEnumerator Think()
    {
        while (!_ownHealth.dead)
        {
            if (!objective.IsUnityNull())
            {
                if (!distracted && Random.value > 1 - reTargettingProbability)
                {
                    RandomObjective();
                    yield return new WaitForSeconds(thinkingTime);
                }

                if ((objective.position - _transform.position).magnitude > attackRange)
                    Move();
                else
                {
                    Attack();
                }
            }
            else
            {
                if (!distracted)
                {
                    gameInfo.ResizeObjectivesArray();
                }
                else
                {
                    distracted = false;
                }

                RandomObjective();
                yield return new WaitForSeconds(thinkingTime);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }


    private void Move()
    {
        _navMeshAgent.destination = objective.position;
        _animator.SetTrigger(AiStates.Move);
    }

    private void Attack()
    {
        objectiveHealth.Hurt(attackDamage);
        _animator.SetTrigger(AiStates.Attack);
    }

    public void Distract(Transform distractionTransform)
    {
        objective = distractionTransform;
        distracted = true;
    }
}