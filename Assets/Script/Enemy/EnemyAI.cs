using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyState currentState;
    [SerializeField] private Enemy Enemy;
    [SerializeField] private LayerMask layerTarget;
    public Transform tower;

    //Variable for data enemy
    private int _health, _maxHealth;
    public int Health => _health;
    public int MaxHealth => _maxHealth;
    private int _damage;
    private float _delayAttack;
    //Variable for move and animation
    private Transform _target;
    private NavMeshAgent nav;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        SetData(Enemy);

        tower = GameObject.Find("Tower Defense").transform;

        _target = tower;
    }

    // Update is called once per frame
    void Update()
    {   
        if(PhotonNetwork.IsMasterClient) {
            if(!nav.isOnNavMesh) {
                nav.Warp(transform.position);
            }

            switch(currentState) {
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Die:
                    break;
                default:
                    Move();
                    break;
            }
        }
    } 
    void Idle() {
        if(Vector3.Distance(transform.position, _target.position) <= Enemy.RangeAttack && _delayAttack > Time.time) {
            Enemy.DoAnimation(animator,"Walk",false);
        }
        else {
            SetState(EnemyState.Move);
        }
    }

    void Move() {
        if(Vector3.Distance(transform.position, _target.position) <= Enemy.RangeAttack) {
            SetState(EnemyState.Attack);
        }
        else {
            if(nav.isOnNavMesh) nav.SetDestination(_target.position);
            Enemy.DoAnimation(animator,"Walk",true);
        }
    }

    void Attack() {
        if(_delayAttack <= Time.time) {
            transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
            Enemy.DoAnimation(animator,"Attack");
            _delayAttack = Time.time + Enemy.DelayTime;
            if(nav.isOnNavMesh) nav.SetDestination(transform.position);
        } 
    }

    public void DoAttack() {
        Enemy.DoAttack(this, animator, layerTarget);
        if(_target != tower) {
            if(_target.gameObject.GetComponent<PlayerController>().CurrentState == PlayerState.Dead) {
                _target = tower;
            }
        }
    }

    public void FinishAttack() {
        SetState(EnemyState.Idle);
    }

    void Die() {
        Enemy.DoAnimation(animator,"Die");
        if(nav.isOnNavMesh) nav.SetDestination(transform.position);
        GetComponent<CapsuleCollider>().enabled = false;
        Invoke("AudoDisable", 2);
    }

    void AudoDisable() {
        gameObject.SetActive(false);
    }

    void SetData(Enemy _enemy) {
        this._health = this._maxHealth = _enemy.Health;
        this._damage = _enemy.Damage;
        this.nav.speed = _enemy.Speed;
        _delayAttack = Time.time;
    }

    void SetState(EnemyState newState) {
        currentState = newState;
    }

    [PunRPC]
    public void TakeDame(int damage) {
        _health -= damage;

        if(_health <= 0) {
            _health = 0;
            SetState(EnemyState.Die);
            Die();
        }
        else {
            Enemy.DoAnimation(animator,"GetHit");
        }
        
        if(_target == tower) {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(0f, 0.5f, 0f), 8f,layerTarget);
            foreach(Collider Player in hitColliders) {
                PlayerController player = Player.GetComponent<PlayerController>();
                if(player != null) {
                    _target = player.gameObject.transform;
                }
            }
        }
    }

    [PunRPC]
    public void ReBorn() {
        _health = _maxHealth;
        GetComponent<CapsuleCollider>().enabled = true;
        SetState(EnemyState.Move);
        _target = tower;
        gameObject.SetActive(true);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, 0.5f, 0f) + transform.forward, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, 0.5f, 0f), 8f);
    }
}
