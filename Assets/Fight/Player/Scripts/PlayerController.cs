using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Text;

public class PlayerController : MonoBehaviour
{
    public bool isAttacking = false; // TODO private
    public bool isBlocking = false;   // TODO private

    //private bool ResetAttacked = false;


    // Constants
    private const float MAX_SPEED = 5f;

    // References
    private InputSystem inputSystemActions;
    private InputAction move;
    private Health health;
    private FajtovPlayerAnimator fajtovAnimator;
    private Rigidbody rb;

    // Private vars
    private GlobalState.Player _whichPlayer = GlobalState.Player.one;
    private CharacterData _characterData;
    private GameObject _attackPoint;
    private bool _facingRight = true;
    private Vector3 _currentForce = Vector3.zero;
    private bool _isGrounded;
    private LayerMask _enemyMask;


    // Initializes the character controller with needed data
    public void Initialize(GlobalState.Player whichPlayer, bool facingRight, int enemyMask)
    {
        _whichPlayer = whichPlayer;
        _facingRight = facingRight;
        _enemyMask = enemyMask;
        _characterData = (whichPlayer == GlobalState.Player.one)? GlobalState.Player1Character : GlobalState.Player2Character;

        // Get attack point and set it up
        _attackPoint = transform.GetChild(0).gameObject;
        _attackPoint.transform.localPosition = new Vector3(_characterData.attackPointOffset.x, _characterData.attackPointOffset.y, 0); 

        // DEBUG
        if (!_characterData.isValid)
        {
            Debug.Log("Invalid character added, getting default");
            _characterData = GlobalState.GetCharacterForDebug();
        }

        if (fajtovAnimator == null) fajtovAnimator = GetComponent<FajtovPlayerAnimator>();
        fajtovAnimator.Initialize(ref _characterData);
    }


    // Hide attack point, get components
    private void Start()
    {
        _attackPoint = transform.GetChild(0).gameObject;
        _attackPoint.SetActive(false);
        rb = GetComponent<Rigidbody>();
        fajtovAnimator = GetComponent<FajtovPlayerAnimator>();
        health = GetComponent<Health>();
        InitializeInputSystem();
    }
    
    
    // Creates a new input system and adds the controls of the character based on PlayerOne or PlayerTwo
    private void InitializeInputSystem()
    {
        inputSystemActions = new InputSystem();

        // Input keys for player one
        if (_whichPlayer == GlobalState.Player.two)
        {
            inputSystemActions.PlayerRight.Jump.started += JumpCB;
            inputSystemActions.PlayerRight.Attack.started += AttackCB;
            inputSystemActions.PlayerRight.Block.started += BlockCB;
            inputSystemActions.PlayerRight.Block.canceled += BlockCB;
            move = inputSystemActions.PlayerRight.Move;
            inputSystemActions.PlayerRight.Enable();
        }

        // Input keys for player two
        else if (_whichPlayer == GlobalState.Player.one)
        {
            inputSystemActions.PlayerLeft.Jump.started += JumpCB;
            inputSystemActions.PlayerLeft.Attack.started += AttackCB;
            inputSystemActions.PlayerLeft.Block.started += BlockCB;
            inputSystemActions.PlayerLeft.Block.canceled += BlockCB;
            move = inputSystemActions.PlayerLeft.Move;
            inputSystemActions.PlayerLeft.Enable();
        }
        else
        {
            Debug.LogWarning(gameObject.name + " - Player is neither Player one or two");
            return;
        }
    }

    // // Odebrání akcí při zakázání objektu
    // private void OnDisable()
    // {
    //     if (_whichPlayer == GlobalState.Player.two)
    //     {
    //         // Odpojení akcí skoku a útoku
    //         inputSystemActions.PlayerRight.Jump.started -= Jump;
    //         inputSystemActions.PlayerRight.Attack.started -= Attacking;

    //         // Zakázání akcí hráče
    //         inputSystemActions.PlayerRight.Disable();
    //     }
    //     else if (_whichPlayer == GlobalState.Player.one)
    //     {
    //         // Odpojení akcí skoku a útoku
    //         inputSystemActions.PlayerLeft.Jump.started -= Jump;
    //         inputSystemActions.PlayerLeft.Attack.started -= Attacking;

    //         // Zakázání akcí hráče
    //         inputSystemActions.PlayerLeft.Disable();
    //     }
    //     else
    //     {
    //         return;
    //     }
    // }


    private void FixedUpdate()
    {
        Movement();
        CheckForFlip();
    }


    // Gets the forward vector of the camera // TODO maybe not necessary?
    private Vector3 GetCameraForward()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }


    // Gets the right vector of the camera // TODO maybe not necessary?
    private Vector3 GetCameraRight()
    {
        Vector3 right = Camera.main.transform.right;
        right.y = 0;
        return right.normalized;
    }


    // Moves the player based on velocity, limits and input
    private void Movement()
    {
        // TODO put in anim controller
        float _move = rb.velocity.magnitude / MAX_SPEED;
        if (_move > 0.01f && _isGrounded)
        {
            fajtovAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Move);
            //animator.SetBool("Move", true);
        }
        else
        {
            fajtovAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Idle);
            //animator.SetBool("Move", false);
        }

        // Get current input values
        Vector2 moveInput = move.ReadValue<Vector2>();
        _currentForce += moveInput.x * _characterData.speed * GetCameraRight() * Time.fixedDeltaTime;
        _currentForce += moveInput.y * GetCameraForward() * _characterData.speed * Time.fixedDeltaTime;
        
        // Add force and reset
        rb.AddForce(_currentForce, ForceMode.Impulse);
        _currentForce = Vector3.zero;

        // Velocity limits
        Vector3 horizontalVelocity = rb.velocity;
        if (Mathf.Abs(horizontalVelocity.x) > MAX_SPEED)
        {
            horizontalVelocity.x = Mathf.Sign(horizontalVelocity.x) * MAX_SPEED;
            rb.velocity = horizontalVelocity;
        }
    }


    // Block callback for the input system
    private void BlockCB(InputAction.CallbackContext obj)
    {
        if (isAttacking)
        {
            fajtovAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Idle);
            //animator.SetBool("Block", false); // TODO put in anim controller
            isBlocking = false;
            return;
        }

        if (obj.started)
        {
            fajtovAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Block);
            //animator.SetBool("Block", true); // TODO put in anim controller
            isBlocking = true;
        }
        else 
        { 
            fajtovAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Idle);
            //animator.SetBool("Block", false); // TODO put in anim controller
            isBlocking = false;
        }
    }


    // Check if the player switched movement direction and flip him
    private void CheckForFlip()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();

        if (moveInput.x > 0 && !_facingRight || moveInput.x < 0 && _facingRight)
        {
            _facingRight = !_facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }


    // Attack callback for input system
    private void AttackCB(InputAction.CallbackContext obj)
    {
        if (!isAttacking && !isBlocking)
        {
            isAttacking = true;
            FreezePlayer();

            //animator.SetTrigger("Attack"); // TODO put in anim controller
            fajtovAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Attack);
            StartCoroutine(DoAttack());
            StartCoroutine(WaitForEndAttack());
        }
    }

    public IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(_characterData.attackFrameIdx * CharacterLoader.FRAME_DELAY);
        
        Collider[] hitEnemy = Physics.OverlapSphere(_attackPoint.transform.position, _characterData.attackSize, _enemyMask);
        foreach (Collider enemy in hitEnemy)
        {
            if(enemy.gameObject == this)
            {
                continue;
            }
            Debug.Log("hit " + enemy.name);

            PlayerController enemyPlayer = enemy.GetComponent<PlayerController>();
            if(enemyPlayer != null) enemyPlayer.TakeDamage(_characterData.damage);
        }
    }


    private IEnumerator WaitForEndAttack()
    {
        yield return new WaitForSeconds(_characterData.attackAnim.frames.Length * CharacterLoader.FRAME_DELAY);
        isAttacking = false;
        UnFreezePlayer();
    }


    public void TakeDamage(int damage)
    {
        if (isBlocking) return;
        health.TakeDamage(damage);
    }

    public void FreezePlayer()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
    }

    public void UnFreezePlayer()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
    }

    // Debug method for drawing the overlap attack sphere
    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;
        Gizmos.DrawWireSphere(_attackPoint.transform.position, _characterData.attackSize);
    }

    public GlobalState.Player GetWhichPlayer()
    {
        return _whichPlayer;
    }


    // Jump callback for the inputAction
    private void JumpCB(InputAction.CallbackContext obj)
    {
        if (_isGrounded)
        {
            _isGrounded = false;
            fajtovAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Jump);
            //animator.SetTrigger("Jump");    // TODO put in anim controller
            _currentForce += Vector3.up * _characterData.jump;
        }
    }

    
    // Check the floor upon enter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }


    // Check the floor upon exit
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

}
