using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{   
    [Header("Reference")]
    [SerializeField] private PlayerData playerData;
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

    [Header("Variable for target")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float radiusScan;

    [Header("Weapon")]
    [SerializeField] private Weapon[] Weapons = new Weapon[2];

    //Variable for player data
    private int _health;
    private int _mana;
    private float _moveSpeed, _runSpeed;
    private int _sensitiveMouse;
    
    //Variable for camera rotation and move
    private CharacterController controler;  
    private Cinemachine3rdPersonFollow _3rdPerson;
    private float _cameraYaw, _cameraPitch;
    private float _targetRotation = 0.0f;
    private Vector3 targetDirection = Vector3.zero;
    private float _rotationVelocity;
    private float _targetSpeed;

    //Variable for animation
    private Animator animator;
    private bool _hasAnimator;
    private float _animationBlend;

    //Variable for weapon
    private int _currentWeapon = 0;

    //Variable for target
    private Transform _target;

    //New input system
    private PlayerInput _input;

    void Awake() {
        controler = GetComponent<CharacterController>();
        _3rdPerson = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        animator = GetComponent<Animator>();
        _input = new PlayerInput();
        
        _health = playerData._maxHealth;
        _mana = playerData._maxMana;
        _moveSpeed = playerData._speedMove;
        _sensitiveMouse = playerData._sensitiveMouse;
        
        animator.runtimeAnimatorController= Resources.Load<RuntimeAnimatorController>(Weapons[_currentWeapon].dirAnimation);
        
        _input.Player.Skill1.performed += ctx => Skill1();
        _input.Player.Skill2.performed += ctx => Skill2();
        _input.Player.Skill3.performed += ctx => Skill3();
        _input.Player.Skill4.performed += ctx => Skill4();

        _input.Player.Attack1.performed += ctx => Attack1();
        _input.Player.Attack2.performed += ctx => Attack2();

        _input.Player.ChangeWeapon.performed += ctx => ChangeWeapon();

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
        CheckTarget();
    }

    void OnEnable() {
        _input.Enable();
    }

    void OnDisable() {
        _input.Disable();
    }

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
        controler.Move(targetDirection.normalized * (_targetSpeed * Time.deltaTime));
        ;
        if(_hasAnimator) {
            Weapons[_currentWeapon].DoAnimation(animator, "Speed", (_targetSpeed == _moveSpeed)? 1.0f : 0.0f);
        }
    }

    void CameraRotation() {
        Vector2 rotate = _input.Player.Look.ReadValue<Vector2>();
        _cameraYaw += rotate.x * Time.deltaTime * _sensitiveMouse;
        _cameraPitch += rotate.y * Time.deltaTime * _sensitiveMouse;

        _cameraPitch= Mathf.Clamp(_cameraPitch, BottomClamp, TopClamp);

        root.transform.rotation = Quaternion.Euler(_cameraPitch, _cameraYaw, 0.0f);
    }
  
    void Attack1() {
        Debug.Log("Attack1");
    }
    
    void Attack2() {
        Debug.Log("Attack2");
    }

    void Skill1() {
        Debug.Log("Skill1");
    }
    
    void Skill2() {
        Debug.Log("Skill2");
    }

    void Skill3() {
        Debug.Log("Skill3");
    }

    void Skill4() {
        Weapons[_currentWeapon].DoSkill4();
    }

    void ChangeWeapon() {
        Debug.Log("Tab");
    }

    void Zoom(float value) {
        float zoom = map(value,-240f,240f,-0.3f,0.3f);
        float newDistance = _3rdPerson.CameraDistance + zoom;
        _3rdPerson.CameraDistance = Mathf.Clamp(newDistance, minZoom, maxZoom);
    }

    void CheckTarget() {
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

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(root.transform.position, root.transform.position + root.transform.forward);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radiusScan);
    }
}
