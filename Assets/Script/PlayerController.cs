using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Photon.Pun; 

public class PlayerController : MonoBehaviour
{   
    [Header("Reference")]
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject root;
    [SerializeField] private ListCharacter ListCharacter;

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

    public LayerMask TargetLayer => targetLayer;

    //Variable character class
    private CharacterClass Character;

    //Variable for player data
    private string _name;
    public string Name => _name;
    private PlayerState currentState;
    private int _health, _maxHealth;
    
    public int Health => _health;
    public int MaxHealth => _maxHealth;

    private int _stamina;
    private float _walkSpeed, _runSpeed, _currentSpped;
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
    private bool _canSkill = true;

    //Variable for combo
    private int _combo;
    private bool _hasEnemyOnScreen;

    //Variable for skill
    private float _resetSkill;
    public float ResetSkill => _resetSkill;

    //Variable for target
    private PlayerUI UI;
    private Transform _target;

    //New input system
    private PlayerInput _input;

    //Photon view
    private PhotonView view;

    void Awake() {
        controler = GetComponent<CharacterController>();
        _3rdPerson = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
        SetState(PlayerState.Idle);
        
        //Get character class
        Character = ListCharacter.GetCharacter((int)view.Owner.CustomProperties["CharacterClass"]);
        //Instantiate model
        Instantiate(Character.Model, transform);
        //Set player Data
        SetDataPlayer(Character);
        //Add animation controller
        animator.runtimeAnimatorController= Resources.Load<RuntimeAnimatorController>(Character.DirAnimation);
        //Add animation avatar
        animator.avatar = Character.Avatar;

        //Add new input system
        _input = new PlayerInput();
        
        //Set data player  
        _sensitiveMouse = 10;
        
        if(!view.IsMine) {
            mainCamera.SetActive(false);
            vcam.gameObject.SetActive(false);
        }
    
        if(view.IsMine) {
            UI = FindObjectOfType<PlayerUI>();

            _input.Player.Attack.performed += ctx => Attack();
            _input.Player.Skill.performed += ctx => Skill();
            _input.Player.Roll.performed += ctx => Roll();
            _input.Player.Jump.performed += ctx => Jump();
            _input.Player.ChangeModeMove.performed += ctx => ChangeModeMove();

            _input.Player.Zoom.performed += ctx => Zoom(ctx.ReadValue<float>());
        }

        
    }

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _hasAnimator = TryGetComponent(out animator);
    }

    void Update() {
        switch (currentState) {
            case PlayerState.Dame: 
            
                break;
            case PlayerState.Stun: 
            
                break;
            default: 
                if(view.IsMine) {
                    Move();
                    CameraRotation();
                }
                break;
        }
        if(view.IsMine) {
            TargetCheck();
        }
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
            _targetSpeed = _currentSpped;
        }
        else {
            _targetSpeed = 0.0f;
        }

        //Can not move if player not in Idle state
        if(currentState != PlayerState.Idle) {
            _targetSpeed = 0.0f;  
        } 

        //If rool speed = 10;
        if(currentState == PlayerState.Roll) {
            _targetSpeed = 5.0f; 

            controler.Move(transform.forward * (_targetSpeed * Time.deltaTime));
        }

        controler.Move(targetDirection.normalized * (_targetSpeed * Time.deltaTime) 
            + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        if(_hasAnimator) {
            Character.DoAnimation(this, animator, "Speed", map(_targetSpeed,0,10,0,1));
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
        if(_grounded && currentState == PlayerState.Idle) {
            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            if(_hasAnimator) {
                Character.DoAnimation(this, animator, "Jump");
            }
        }
    }

    void Attack() {
        if(_grounded && _canAttack && (currentState == PlayerState.Idle || currentState == PlayerState.Attack)) {
            AutoRotationToEnemy();
            Character.DoAnimation(this, animator, "Attack"+_combo);
            
            _canAttack = false;
            SetState(PlayerState.Attack);
        }
    }

    public void DoAttack() {
        Character.DoAttack(this, animator, _combo);
    }

    //Auto rotaion look at first enemy
    void AutoRotationToEnemy() {
        if(_hasEnemyOnScreen && _target!= null && _input.Player.Move.ReadValue<Vector2>() == Vector2.zero) {
            transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
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
        SetState(PlayerState.Idle);
        _canSkill = true;
    }

    //Do skill
    void Skill() {
        if(_grounded && _canSkill &&_resetSkill < Time.time) {
            AutoRotationToEnemy();
            Character.DoAnimation(this, animator, "Skill");
            _canSkill = false;
        }
    }

    public void DoSkill() {
        _resetSkill= Time.time + Character.DoSkill(this, animator);
    }

    //Do roll 
    void Roll() {
        if(_grounded && currentState == PlayerState.Idle) {
            Character.DoRoll(this, animator);
            SetState(PlayerState.Roll);
        }
        
    }

    public void FinishRollAnim() {
        SetState(PlayerState.Idle);
    }

    //Zoom in or out camera
    void Zoom(float value) {
        float zoom = map(value,-240f,240f,-0.3f,0.3f);
        float newDistance = _3rdPerson.CameraDistance + zoom;
        _3rdPerson.CameraDistance = Mathf.Clamp(newDistance, minZoom, maxZoom);
    }

    void ChangeModeMove() {
        _currentSpped = (_currentSpped == _runSpeed)? _walkSpeed : _runSpeed;
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
        _hasEnemyOnScreen = UI.ShowTargetCrosshair(mainCamera.GetComponent<Camera>(), transform, _target, radiusScan);
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }

    public void SetName(string newName) {
        _name = newName;
    }

    void SetDataPlayer(CharacterClass Character){
        this._health = this._maxHealth = Character.Health;
        this._stamina = Character.Stamina;
        this._walkSpeed = Character.WalkSpeed;
        this._runSpeed = Character.RunSpeed;
        _currentSpped = _runSpeed;

        _resetSkill = Time.time;
    }

    public void SetState(PlayerState newState) {
        currentState = newState;
    }

    public CharacterClass GetCharacterClass() {
        return Character;
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

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0f) + transform.forward * 0.5f, transform.position + new Vector3(0f, 0.5f, 0f) + transform.forward * 2);
    }
}
