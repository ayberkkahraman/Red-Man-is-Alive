using System;
using UnityEngine;

namespace Project._Scripts.Runtime.Entity.EntitySystem.Entities
{
    public class EnemyUnit : Unit
    {
        public GameObject Coin;
        public Transform GroundCheckerPoint;
        public LivingEntity Target { get; set; }
        public LayerMask GroundLayers;

        private Vector3 _defaultPosition;
        private Vector3 _nextPosition;
        protected Vector3 TargetPosition;
        protected bool IsInTarget;
        [Range(0f, 10f)] public float PatrollingDistance;
        [Range(0f, 3f)] public float WaitTimeInTarget;
        private float _patrollingTimer;
        public enum State
        {
            Idle,
            Patrolling,
            Chase,
            Attack
        }

        public State CurrentState;
        
        public float ChaseSpeed = 15f;
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public bool IsAttacking;
        public Rigidbody2D Rigidbody {
            get;
            set;
        }

        public Action OnDieCallback;

        public bool CanMove = true;
        private GameObject _coin;

        protected override void Start()
        {
            base.Start();

            _defaultPosition = transform.position;
            _nextPosition = _defaultPosition + transform.right * PatrollingDistance;
            TargetPosition = _nextPosition;
            
            _patrollingTimer = WaitTimeInTarget;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Rigidbody = GetComponent<Rigidbody2D>();

            CurrentState = State.Idle;

            OnDieHandler += DropCoin;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnDieHandler -= DropCoin;
        }

        public override void Update()
        {
            base.Update();
            
            UpdateState();
        }

        public void SetRotation(Vector3 target)
        {
            if(IsAttacking) return;
            
            transform.rotation = (target.x - transform.position.x) switch
            {
                > 0 => new Quaternion(0, 0, 0, 1),
                < 0 => new Quaternion(0, 180, 0, 1),
                _ => transform.rotation
            };
        }

        public bool CanChase()
        {
            return Physics2D.RaycastAll(GroundCheckerPoint.position, -GroundCheckerPoint.up, 2f, GroundLayers).Length > 0;
        }
        
        public void Chase()
        {
            if (!CanChase())
            {
                StopMovement();
                return;
            }

            if(CanMove == false) return;
            
            Move();
        }

        protected virtual void Move()
        {
            Animator.SetBool(IsMoving, true);
            Vector2 targetVelocity = new Vector2(transform.right.x * .2f *ChaseSpeed, Rigidbody.velocity.y);
            Rigidbody.velocity = targetVelocity;
        }

        protected virtual void StopMovement()
        {
            Rigidbody.velocity = new Vector2(0, Physics2D.gravity.y);
            CurrentState = State.Idle;
            Animator.SetBool(IsMoving, false);
        }

        protected virtual void Patrol()
        {
            if(CurrentState == State.Attack) return;
            
            IsInTarget = Vector2.Distance(TargetPosition, transform.position) <= .03f;

            if (!IsInTarget)
            {
                CurrentState = State.Patrolling;
                Move();
            }
            else
            {
                CurrentState = State.Idle;
                if (_patrollingTimer > 0)
                {
                    _patrollingTimer -= Time.deltaTime;
                    StopMovement();
                }
                else
                {
                    TargetPosition = TargetPosition == _defaultPosition ? _nextPosition : _defaultPosition;
                    _patrollingTimer = WaitTimeInTarget;
                    
                    SetRotation(TargetPosition);
                }
            }
        }

        public void UpdateState()
        {
            if (AnyTargetInArea())
            {
                SetRotation(Target.transform.position);
                
                if(Target.Health <= 0) return;
                
                if (Vector2.Distance(Target.transform.position, transform.position) <= UnitData.DamageRadius)
                {
                    if (CurrentCooldown < DefaultCooldown)
                    {
                        StopMovement();
                    }
                    else
                    {
                        if(!CanMove) return;
                        
                        CheckForAttack();
                    }
                }
                else
                {
                    CurrentState = State.Chase;
                    Chase();
                }
            }

            else
            {
                Patrol();
            }
        }

        protected virtual bool AnyTargetInArea()
        {
            if (Health <= 0) return false;
            
            var areaCenter = transform.position + (Vector3.up * transform.localScale.y / 2);
            var areaSize = 2 * new Vector3(UnitData.TargetDetectRange, .25f, 0f);
            Collider2D[] enemiesInRange = Physics2D.OverlapBoxAll(areaCenter, areaSize, 0f, UnitData.TargetLayers);

            if (enemiesInRange.Length == 0)
            {
                if (Target != null) Target = null;
                return false;
            }

            Target = enemiesInRange[0].GetComponent<LivingEntity>();
            return true;
        }

        public void DropCoin()
        {
            Invoke(nameof(SpawnCoin), .1f);
        }

        public virtual void SpawnCoin()
        {
            _coin = Instantiate(Coin, transform.position + Vector3.up*2.5f, Quaternion.identity);
            // _coin.GetComponent<Collider2D>().enabled = false;
            // ActivateCoin();
        }

        public void ActivateCoin()
        {
            _coin.GetComponent<Collider2D>().enabled = true;
        }

        public void ANIM_EVENT_EndAttack()
        {
            CurrentState = State.Idle;
            IsAttacking = false;
        }
    }
}