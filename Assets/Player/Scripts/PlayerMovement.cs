using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Text;

public class PlayerMovement : MonoBehaviour
{
    [Header("ChoosePlayer")]
    // Proměnné pro výběr hráče - levo nebo pravo
    public bool playerRight;

    // Pole pro vstupní akce hráče
    private InputSystem inputSystemActions;

    private InputAction move;
    private InputAction block;


    [Space]

    [Header("Movement")]

    // Rigidbody pro fyzikální interakce
    private Rigidbody rb;
    // Nastavení síly pohybu, skoku a maximální rychlosti
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    // Směr a síla pohybu hráče
    private Vector3 forceDirection = Vector3.zero;
    private SpriteRenderer spriteRenderer;

    [Header("Camera")]

    // Kamera sledující hráče
    [SerializeField]
    private Camera playerCamera;
    // Animator pro správu animací hráče
    private Animator animator;
    // Proměnné pro kontrolu stavů hráče
    private bool isGrounded;
    private bool facingRight = true;

    public static bool isBlocking = false;

    [Header("AttackPoint")]

    // Nastavení bodu a síly útoku
    [SerializeField]
    private GameObject attackPoint;
    [SerializeField]
    private float attackPointSize;
    [SerializeField]
    private LayerMask enemyMask;
    [SerializeField]
    public int _GivenDamage = 20;

    private bool attacked = false;
    // Inicializace proměnných při probuzení objektu
    private void Awake()
    {
        if (playerRight)
        {
            facingRight = false;
        }
    
        // Inicializace komponent
        attackPoint.SetActive(false);
        rb = this.GetComponent<Rigidbody>();
        inputSystemActions = new InputSystem();
        animator = this.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Přiřazení akcí při povolení objektu
    private void OnEnable()
    {
        if (playerRight)
        {
            inputSystemActions.PlayerRight.Jump.started += Jump;
            inputSystemActions.PlayerRight.Attack.started += Attacking;
            // Připojení akce pohybu
            move = inputSystemActions.PlayerRight.Move;
            block = inputSystemActions.PlayerRight.Block;

            inputSystemActions.PlayerRight.Enable();
        }
        else if (!playerRight)
        {
            inputSystemActions.PlayerLeft.Jump.started += Jump;
            inputSystemActions.PlayerLeft.Attack.started += Attacking;
            // Připojení akce pohybu
            move = inputSystemActions.PlayerLeft.Move;
            block = inputSystemActions.PlayerLeft.Block;

            inputSystemActions.PlayerLeft.Enable();
        }
        else
        {
            return;
        }
    }

    // Odebrání akcí při zakázání objektu
    private void OnDisable()
    {
        if (playerRight)
        {
            // Odpojení akcí skoku a útoku
            inputSystemActions.PlayerRight.Jump.started -= Jump;
            inputSystemActions.PlayerRight.Attack.started -= Attacking;

            // Zakázání akcí hráče
            inputSystemActions.PlayerRight.Disable();
        }
        else if (!playerRight)
        {
            // Odpojení akcí skoku a útoku
            inputSystemActions.PlayerLeft.Jump.started -= Jump;
            inputSystemActions.PlayerLeft.Attack.started -= Attacking;

            // Zakázání akcí hráče
            inputSystemActions.PlayerLeft.Disable();
        }
        else
        {
            return;
        }
    }

    // Hlavní metoda pro pohyb hráče
    private void FixedUpdate()
    {

        Move();
        Blocking();

        Vector2 moveInput = move.ReadValue<Vector2>();
        forceDirection += move.ReadValue<Vector2>().x * movementForce * GetCameraRight(playerCamera);
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;
        
        rb.AddForce(forceDirection, ForceMode.Impulse);

        // Resetování směru síly
        forceDirection = Vector3.zero;

        // Kontrola gravitace a rychlosti
        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        // Omezení horizontální rychlosti
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        // Kontrola otáčení hráče
        if (moveInput.x > 0 && !facingRight || moveInput.x < 0 && facingRight)
        {
            Flip(true);
        }
    }

    // Metody pro získání směru kamery
    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    // Metoda pro skok
    private void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            // Spuštění animace skoku
            animator.SetTrigger("Jump");
            // Přidání síly skoku
            forceDirection += Vector3.up * jumpForce;
        }
    }

    // Kontrola kolizí s povrchem
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Metoda pro pohyb
    private void Move()
    {
        // Výpočet aktuální rychlosti
        float _move = rb.velocity.magnitude / maxSpeed;
        // Nastavení animací pohybu
        if (_move > 0.1f && isGrounded)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    private void Blocking()
    {
        if (block.IsPressed())
        {
            animator.SetBool("Block", true);
        }
        else
        {
            animator.SetBool("Block", false);
        }
    }

    // Metoda pro otáčení hráče
    private void Flip(bool flipValue)
    {
        // Obrácení směru hráče
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Metoda pro útok
    private void Attacking(InputAction.CallbackContext obj)
    {
        if (!attacked)
        {
            animator.SetTrigger("Attack");
            rb.isKinematic = true;
            attacked = true;
        }
        
        
    }

    public void Attacked()
    {
        if (!isBlocking)
        {
            attacked = false;
            attackPoint.SetActive(true);
            Collider[] hitEnemy = Physics.OverlapSphere(attackPoint.transform.position, attackPointSize, enemyMask);
            foreach (Collider enemy in hitEnemy)
            {
                if(enemy.gameObject == this)
                {
                    continue;
                }
                Debug.Log("hit " + enemy.name);

                enemy.GetComponent<Health>().TakeDamage(_GivenDamage);
            }
            rb.isKinematic = false;
        }
    }

    // Metoda pro vykreslení bodu útoku ve scéně
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackPointSize);
    }

    // Metoda pro reakci na zranění
    public void Hurt(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Hurt");
    }

    // Metoda pro smrt hráče
    public void Die()
    {
        animator.SetTrigger("Death");

    }

}
