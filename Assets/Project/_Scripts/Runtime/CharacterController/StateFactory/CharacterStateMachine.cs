using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Runtime.Entity.CharacterController.States.BaseStates;
using Project._Scripts.Runtime.Entity.EntitySystem;
using Project._Scripts.Runtime.Entity.EntitySystem.Entities;
using Project._Scripts.Runtime.Library.Controller;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Entity.CharacterController.StateFactory
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(UnityEngine.CharacterController))]
    
    
    public class CharacterStateMachine : MonoBehaviour
    {
    #region State
        private CharacterStateFactory _states;
    
        public CharacterBaseState CurrentState { get; set; }

        public CharacterBaseState PreviousState { get; set; }

        public CharacterBaseState NextState { get; set; }
    #endregion

    #region Components
        [Header("Components")]
        [HideInInspector] public UnityEngine.CharacterController CharacterController;
        [HideInInspector] public Animator Animator;
        public AnimatorOverrideController AnimatorOverrideController { get; set; }
        public LivingEntity LivingEntity { get; set; }
        public Unit EntityBase { get; set; }

        public FullBodyBipedIK FullBodyBipedIK { get; set; }
    #endregion

    #region Fields

    #region Movement
        [Header("Movement")]
        public float WalkingSpeed = 5f;
        public float RunningSpeed = 7.5f;
        [HideInInspector] public float DefaultWalkingSpeed;

        public const float Gravity = -9.81f;
        
        public Func<bool> CanPlayerMove { get; set; }
        public HashSet<MovementCondition> WalkConditions
        {
            get => _walkConditions;
            set => _walkConditions = value;
        }
        public HashSet<MovementCondition> RotateConditions
        {
            get => _rotateConditions;
            set => _rotateConditions = value;
        }

        [Space]
        [HideInInspector] public Vector3 InputDirection;

        [Range(0f, .1f)]
        [SerializeField] private float _minSpeedTreshold = .002f;

        [Range(1.5f, 3f)] public float SlowdownAccelerationMultiplier = 2f;
        public float SpeedAccelerationSpeed => _minSpeedTreshold;

        public float MinSpeedTreshold => _minSpeedTreshold;
        public bool IsMovementButtonPressed { get; set; }

        public float CurrentMovementSpeed { get; set; }

        private Vector3 _velocity;
    
    #endregion

    #region Rotation
        [Header("Rotation")]

        [Range(2f, 25f)]public float RotationSpeed = 15f;
        [HideInInspector] public float DefaultRotationSpeed;

        [Range(0.0f, 1.0f)][SerializeField] private float _rotationSmoothSpeed = .75f;
        public float RotationSmoothSpeed => _rotationSmoothSpeed;

        public Func<bool> CanPlayerRotate { get; set; }

        public bool IsRotating { get; set; }

        public bool IsHardRotating { get; set; }

        public Quaternion PreviousRotation { get; set; }

        [HideInInspector] public Vector3 AppliedVelocity;

    #endregion

    #region Jump
        [Header("Jump")]
        public LayerMask GroundLayers;
        [SerializeField] [Range(5f, 25f)] private float _jumpForce = 12.5f;
        [Range(0f, 1f)] [SerializeField] private float _groundCheckerDistance = .535f;
        [Range(0f, .5f)] [SerializeField] private float _coyoteTime = .1f;
        [Range(0f, 1f)] [SerializeField] private float _movementControlOnAir = .45f;
        [Range(0f, 1f)] [SerializeField] private float _rotationControlOnAir = .25f;

        public bool IsLanded => _coyoteTimer > 0f;
        private float _coyoteTimer;
        
        public Transform GroundCheckerPoint;
        public float JumpForce => _jumpForce;
        public float GroundCheckerDistance => _groundCheckerDistance;
        public float CoyoteTime => _coyoteTime;
        public float MovementControlOnAir => _movementControlOnAir;
        public float RotationControlOnAir => _rotationControlOnAir;
        #endregion

        private HashSet<MovementCondition> _walkConditions;
        private HashSet<MovementCondition> _rotateConditions;

        public Transform CameraTransform { get; set; }

        public float RotateAngle { get; set; }
        public float RotateAngleLimit => 1.75f;
    #endregion
        

    #region Animation Hash
        private static int _isMovingHash;
        private static int _velocityHash;
        private static int _isInteractingHash;
        private static int _hardRotate;
    #endregion

        public Action OnAnimatorMoveCallback;

    #region Unity Functions
        private void Awake()
        {
            Animator = GetComponent<Animator>();

            _states = new CharacterStateFactory(this);
            CurrentState = _states.Initialize();
            CurrentState.EnterState();
        
            CurrentState.InitializeState();
        
            _isMovingHash = Animator.StringToHash("IsMoving");
            _velocityHash = Animator.StringToHash("Velocity");
            _isInteractingHash = Animator.StringToHash("IsInteracting");
            
            InputController.ControllerInput.CharacterController.Move.started += MovementConfiguration;
            InputController.ControllerInput.CharacterController.Move.canceled += MovementConfiguration;
            InputController.ControllerInput.CharacterController.Move.performed += MovementConfiguration;

            WalkConditions = new HashSet<MovementCondition>();
            RotateConditions = new HashSet<MovementCondition>();

            CanPlayerMove = CanWalk;
            
            CanPlayerMove += () => WalkConditions.All(x => x.Condition?.Invoke() == true);
        
            CanPlayerRotate = CanRotatePlayer;

            CanPlayerRotate += () => RotateConditions.All(x => x.Condition?.Invoke() == true);
        }
        public void OnEnable()
        {
            Init();
        }
        public void FixedUpdate()
        {
            CurrentState.FixedUpdateState();
        }
        protected void Update()
        {
            CurrentState.UpdateState();

            SetAnimations();

            UpdateCoyoteTime();
        }

        public void LateUpdate()
        {
            CurrentState.LateUpdateState();
        }

        public void OnDrawGizmos()
        {
            CurrentState?.DrawGizmos();
        }
    #endregion


    #region Init / DeInit
        protected void Init()
        {
            AnimatorOverrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
            Animator.runtimeAnimatorController = AnimatorOverrideController;
            CharacterController = GetComponent<UnityEngine.CharacterController>();
            LivingEntity = GetComponent<LivingEntity>();
            EntityBase = LivingEntity as Unit;
            FullBodyBipedIK = GetComponent<FullBodyBipedIK>();

            PreviousRotation = transform.rotation;

            DefaultWalkingSpeed = WalkingSpeed;
            DefaultRotationSpeed = RotationSpeed;

            _coyoteTimer = CoyoteTime;

            CameraTransform = ManagerContainer.Instance.GetInstance<CameraManager>().MainCamera.transform;
        }
    
    
        // ReSharper disable once Unity.PreferNonAllocApi
        public bool CanWalk()
        {
            var position = transform.position;
            var groundCheckerPosition = position + transform.forward * GroundCheckerDistance + transform.up;
            var groundCheckerSize = new Vector3(.05f, 5f, .05f);

            var colliders = Physics.OverlapBox(groundCheckerPosition, groundCheckerSize, Quaternion.identity, GroundLayers);

            var canMove =colliders.Length > 0;

            return canMove;
        }

        private bool CanRotatePlayer()
        {
            // return RotateConditions.All(x => x.Condition?.Invoke() == true);
            return true;
        }
    #endregion

    #region Condition Configuration
        public bool IsFalling() => _coyoteTimer < 0 && !IsGrounded();
        public void UpdateCoyoteTime()
        {
            _coyoteTimer = IsGrounded() ? CoyoteTime : _coyoteTimer -= Time.deltaTime;
        }
        public bool ReadyForJump()
        {
            return IsGrounded() || (!IsGrounded() && IsLanded);
        }
        public bool IsGrounded()
        {
            var groundCheckerSize = new Vector3(GroundCheckerDistance / 3f, GroundCheckerDistance, GroundCheckerDistance / 3f);
            var groundCheckerPosition = GroundCheckerPoint.position;
            var isGrounded = Physics.OverlapBox(groundCheckerPosition, groundCheckerSize / 2, Quaternion.identity, GroundLayers).Length > 0f;
            return isGrounded;
        }

        #endregion


    #region Input
        
        /// <summary>
        /// Gets the player movement input
        /// </summary>
        /// <param name="context"></param>
        private void MovementConfiguration(InputAction.CallbackContext context)
        {
            InputDirection = context.ReadValue<Vector2>();

            IsMovementButtonPressed = InputDirection != Vector3.zero;
        }
    #endregion
    
    
    
    #region Animation Setup
        /// <summary>
        /// Sets the animations
        /// </summary>
        private void SetAnimations()
        {
            Animator.SetBool(_isMovingHash, IsMovementButtonPressed);
            Animator.SetFloat(_velocityHash, CurrentMovementSpeed);
        }
    #endregion
    
        private void OnAnimatorMove()
        {
            if (OnAnimatorMoveCallback == null)
            {
                Animator.ApplyBuiltinRootMotion();
            }
            
            else
                OnAnimatorMoveCallback?.Invoke();
        }
    
    #region Acceleration

        public void HandleGravity(Vector3 direction, float speedMultiplier = Gravity)
        {
            //Gravity Apply
            var gravityVector = direction * (speedMultiplier * Time.deltaTime);
            CharacterController.Move(gravityVector);
        }
    
        /// <summary>
        /// Setting up the acceleration for transitions and blends
        /// </summary>
        public void AccelerationConfiguration(float multiplier = 1f, bool rotationSmooth = true)
        {
            RotateAngle = (transform.forward - AppliedVelocity.normalized).magnitude;
        
            //Rotation checker
            IsRotating = RotateAngle > .5f;

            IsHardRotating = RotateAngle > RotateAngleLimit && CurrentMovementSpeed > 3f;

            var speedOnRotation = IsMovementButtonPressed && CurrentMovementSpeed < MinSpeedTreshold ? 1 : IsRotating ? RotationSmoothSpeed : 1f;
        
            speedOnRotation = IsMovementButtonPressed && CurrentMovementSpeed < MinSpeedTreshold ? 1 : IsHardRotating ? speedOnRotation/2 : speedOnRotation;

            if (!rotationSmooth) speedOnRotation = 1f;
        
            //Updating the acceleration speed according the character's rotation action
            var targetSpeed = IsMovementButtonPressed && CanPlayerMove?.Invoke() == true ? WalkingSpeed * speedOnRotation * InputDirection.magnitude : 0f;

            var accelerationSpeed = IsMovementButtonPressed ? SpeedAccelerationSpeed : SpeedAccelerationSpeed * SlowdownAccelerationMultiplier;

        #if UNITY_EDITOR
            accelerationSpeed = accelerationSpeed * 3;
        #endif
            bool isRunning = CurrentState.Factory.WalkState.IsRunning;

            accelerationSpeed *= isRunning ? .5f : 1f;

            accelerationSpeed *= IsMovementButtonPressed ? 1f : .35f;
            
            //Update the current movement speed according to the acceleration
            CurrentMovementSpeed = Mathf.LerpUnclamped(CurrentMovementSpeed, CanPlayerMove?.Invoke()==true ? targetSpeed * multiplier : 0f, accelerationSpeed);
        }
        
        public void RotationConfiguration(float multiplier = 1f)
        {
            if (CanPlayerRotate() == false) return;

            if (RotateAngle > RotateAngleLimit) IsHardRotating = true;

            RotationSpeed =   IsRotating ? (IsHardRotating ? DefaultRotationSpeed/1.5f : RotationSpeed) : DefaultRotationSpeed;

            RotationSpeed *= CurrentMovementSpeed > 2f ? 1 : 1.5f;

            //Get the current rotation
            Quaternion currentRotation = transform.rotation;

            var lookDirection = new Vector3(AppliedVelocity.x, 0, AppliedVelocity.z);
            // lookDirection.y = lookDirection == Vector3.zero ?  .0025f : 0f;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                //Update the rotation of the character according to it's direction
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, RotationSpeed * multiplier * Time.deltaTime);

            }
        
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (PreviousRotation != currentRotation)
            {
                PreviousRotation = currentRotation;
            }
        }

        public void MoveCharacter(Vector3 direction, float speedMultiplier = 1f)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y).normalized;
            
            AppliedVelocity = new Vector3(
                (moveDirection.x * CurrentMovementSpeed),
                0f,
                (moveDirection.z * CurrentMovementSpeed));

            AppliedVelocity = CameraTransform.TransformDirection(AppliedVelocity);

            var rotation = Quaternion.AngleAxis(-CameraTransform.eulerAngles.x, CameraTransform.right);

            AppliedVelocity = rotation * AppliedVelocity;

            //-----------------------------MOVING CHARACTER--------------------------------\\

            if (CanPlayerMove == null || CanPlayerMove() != true)
                return;

            var targetVelocity = IsMovementButtonPressed ? !IsHardRotating ? AppliedVelocity : AppliedVelocity / SlowdownAccelerationMultiplier : transform.forward * CurrentMovementSpeed/1.5f;

            CharacterController.Move(targetVelocity * (speedMultiplier * Time.deltaTime));
            
            //------------------------------------------------------------------------------\\
        }
        
    #endregion
    
    
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var groundCheckerSize = new Vector3(GroundCheckerDistance/3f, GroundCheckerDistance, GroundCheckerDistance/3f);
            Gizmos.DrawCube(GroundCheckerPoint.position, groundCheckerSize);
        }
    }
    
    
    public class MovementCondition
    {
        public readonly Func<bool> Condition;
        public MovementCondition(Func<bool> condition)
        {
            Condition = condition;
        }
    }
}