using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    [SerializeField] private DroneStateUI _droneStateUI;
    [SerializeField] private LineRenderer _lineRenderer;
    [Header("Параметры патрулирования")]
    [SerializeField] private float _patrolRadius = 10f;
    [SerializeField] private float _patrolDelay = 2f;

    private NavMeshAgent _agent;
    private Resource _currentTarget;
    private Base _homebase;
    private Faction _faction;
    private State _state = State.Idle;   
    private float _timer;
    private float _patrolTimer;

    private enum State
    {
        Idle,
        Traveling,
        Collecting,
        Returning
    }
        
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        UI.Instance.droneSpeedSlider.onValueChanged.AddListener(OnSpeedSliderChanged);
        SetState(State.Idle);
    }

    void Update()
    {
        switch (_state)
        {
            case State.Idle:
                HandleIdle();
                break;

            case State.Traveling:
                HandleTraveling();
                break;

            case State.Collecting:
                HandleCollecting();
                break;

            case State.Returning:
                HandleReturning();
                break;
        }
        DrawAgentPath();
    }

    public void SetHomebase(Base homebase)
    {
        _homebase = homebase;
    }
    public void SetFaction(Faction faction)
    {
        _faction = faction;
    }

    private void HandleIdle()
    {
        _patrolTimer -= Time.deltaTime;

        if (_patrolTimer <= 0f)
        {
            Vector3 patrolPoint = GetRandomPatrolPoint();
            _agent.SetDestination(patrolPoint);
            _patrolTimer = _patrolDelay;
        }
    }

    private void HandleTraveling()
    {
        if(_currentTarget == null)
        {
            _state = State.Idle;
            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            SetState(State.Collecting);
        }
    }

    private void HandleCollecting()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f && _currentTarget != null)
        {
            _currentTarget.Collect();
            _currentTarget = null;
            _agent.SetDestination(_homebase.transform.position);
            SetState(State.Returning);
        }
    }

    private void HandleReturning()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _homebase.AddResource();
            SetState(State.Idle);
        }
    }

    private void SetState(State state)
    {
        _state = state;
        _droneStateUI.SetStatusText(state.ToString());
        if (state == State.Collecting)
        {
            _agent.isStopped = true;
        }
        else
        {
            _agent.isStopped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_state != State.Idle)
            return;
        
        Resource node = other.GetComponent<Resource>();
        if (node != null && !node.IsTaken)
        {
            if (node.TryReserve())
            {
                _currentTarget = node;
                _timer = node.collectTime;
                _agent.SetDestination(node.transform.position);
                SetState(State.Traveling);
            }
        }
    }

    private Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * _patrolRadius;
        randomDir.y = 0;

        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }

    private void OnSpeedSliderChanged(float newSpeed)
    {
        _agent.speed = newSpeed * 2;
        _agent.acceleration = newSpeed;
    }

    private void DrawAgentPath()
    {
        if (_lineRenderer == null || !UI.Instance.visualizePathToggle.isOn)
        {
            if (_lineRenderer != null)
                _lineRenderer.positionCount = 0;
            return;
        }

        if (_agent.hasPath && _agent.path.corners.Length > 1)
        {
            _lineRenderer.positionCount = _agent.path.corners.Length;
            _lineRenderer.SetPositions(_agent.path.corners);
        }
        else
        {
            _lineRenderer.positionCount = 0;
        }
    }
}


