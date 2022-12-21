using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{   
    [Header("Reference")]
    
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject root;
    [SerializeField] private PlayerUI UI;

    [Header("Variable for rotation camera")]
    [SerializeField] private float BottomClamp, TopClamp;
    [SerializeField] private float minZoom, maxZoom;
    [SerializeField] private float speedRotation;
    [Range(0.0f, 0.3f)]
    [SerializeField] private float RotationSmoothTime = 0.12f;

    [Header("Variable for movement")]
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private float GroundedRadius;
    [SerializeField] private float GroundedOffset;
    [SerializeField] private float JumpHeight;
    [SerializeField] private float Gravity;
    

    [Header("Variable for target")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float radiusScan;

    [Header("CharacterClass")]
    [SerializeField] private CharacterClass Character;

    //Variable for player data
    private int _health;
    private int _mana;
    private float _moveSpeed, _runSpeed;
    private int _sensitiveMouse;
    
    //Variable for camera rotation
    private Cinemachine3rdPersonFollow _3rdPerson;
    private float _cameraYaw, _cameraPitch;
    
    //Variable for movement
    private Vector3 spherePosition;
    private CharacterController controler;  
    private float _targetRotation = 0.0f;
    private Vector3 targetDirection = Vector3.zero;
    private float _rotationVelocity;
    private float _targetSpeed;
    private float _verticalVelocity;
    private bool _grounded;

    //Variable for animation
    private Animator animator;
    private bool _hasAnimator;
    private float _animationBlend;
    private bool _canAttack = true;

    //Variable for combo
    private int _combo;

    //Variable for target
    private Transform _target;

    //New input system
    private PlayerInput _input;

    void Awake() {
        controler = GetComponent<CharacterController>();
        _3rdPerson = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
         animator = GetComponent<Animator>();
        //Instantiate model
        Instantiate(Character.Model, transform);
        //Add animation controller
        animator.runtimeAnimatorController= Resources.Load<RuntimeAnimatorController>(Character.DirAnimation);
        //Add animation avatar
        animator.avatar = Character.Avatar;
        
        //Add new input system
        _input = new PlayerInput();
        
        //Add 
        _health = 0;
        _mana = 0;
        _moveSpeed = 10;
        _sensitiveMouse = 10;
        
        
        
        _input.Player.Attack.performed += ctx => Attack();
        _input.Player.Skill.performed += ctx => Skill();
        _input.Player.Roll.performed += ctx => Roll();
        _input.Player.Jump.performed += ctx => Jump();
        
        _input.Player.Zoom.performed += ctx => Zoom(ctx.ReadValue<float>());
    }

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _hasAnimator = TryGetComponent(out animator);
    }

    void Update() {
        Move();
        CameraRotation();
        TargetCheck();
        GroundedCheck();
        ApllyGravity();
    }

    void OnEnable() {
        _input.Enable();
    }

    void OnDisable() {
        _input.Disable();
    }

    //Movement player
    void Move() {
        Vector2 move = _input.Player.Move.ReadValue<Vector2>();

        if(move != Vector2.zero) {
            _targetRotation = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);
            
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            
            targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            _targetSpeed = _moveSpeed;
        }
        else {
            _targetSpeed = 0.0f;
        }
        controler.Move(targetDirection.normalized * (_targetSpeed * Time.deltaTime) 
            + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        if(_hasAnimator) {
            Character.DoAnimation(animator, "Speed", (_targetSpeed == _moveSpeed)? 1.0f : 0.0f);
        }
    }

    //Rotation camera arount root;
    void CameraRotation() {
        Vector2 rotate = _input.Player.Look.ReadValue<Vector2>();
        _cameraYaw += rotate.x * Time.deltaTime * _sensitiveMouse;
        _cameraPitch += rotate.y * Time.deltaTime * _sensitiveMouse;

        _cameraPitch= Mathf.Clamp(_cameraPitch, BottomClamp, TopClamp);

        root.transform.rotation = Quaternion.Euler(_cameraPitch, _cameraYaw, 0.0f);
    }
    
    //Check player stand on ground not jump or fly
    void GroundedCheck() {
        spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z); 
        _grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    //Apply gravity for player
    void ApllyGravity() {
        if(_grounded) {
            if(_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }
        }
        _verticalVelocity += Gravity * Time.deltaTime;
    }
    
    //Do jump
    void Jump() {
        if(_grounded) {
            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity); 
            
            if(_hasAnimator) {
                Character.DoAnimation(animator, "Jump");
            }
        }
    }

    void Attack() {
        if(_canAttack) {
            Debug.Log(_combo);
            Character.DoAttack(animator, _combo);
            _canAttack = false;
        }
    }

    //Start combo
    public void StartCombo() {
        _canAttack = true;
        _combo = Character.Start_Combo(_combo);
    }
    
    //Finish combo animation
    public void FinishComboAnim() {
        _canAttack = true;
        _combo = Character.Finish_Anim();
    }

    //Do skill
    void Skill() {
        Character.DoSkill(animator);
    }

    //Do roll 
    void Roll() {
        if(_grounded) {
            Character.DoRoll(animator);
        }
    }

    //Zoom in or out camera
    void Zoom(float value) {
        float zoom = map(value,-240f,240f,-0.3f,0.3f);
        float newDistance = _3rdPerson.CameraDistance + zoom;
        _3rdPerson.CameraDistance = Mathf.Clamp(newDistance, minZoom, maxZoom);
    }

    //Select Target
    void TargetCheck() {
        //Scan Enemies in radius and get first enemy
        Collider[] enemies = Physics.OverlapSphere(transform.position,radiusScan, targetLayer);
        foreach(Collider enemy in enemies) {
            if(enemy.gameObject != gameObject) {
                _target = enemy.transform;
                break;
            }
        }

        //Check target in view of camera and in player's target distance 
        UI.ShowTargetCrosshair(mainCamera.GetComponent<Camera>(), transform, _target, radiusScan);
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }

    //Draw line or spehere to check
    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(root.transform.position, root.transform.position + root.transform.forward);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radiusScan);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(spherePosition, GroundedRadius);
    }
}
