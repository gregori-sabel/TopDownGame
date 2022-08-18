using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 1f;
    private float speed;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    bool canMove = true; 
    Vector2 movementInput;
    SpriteRenderer spriteRenderer; 
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollision = new List<RaycastHit2D>();
    public SwordAttack swordAttack;
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      animator = GetComponent<Animator>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      speed = baseSpeed;
    }

    void Update(){
      if (Input.GetKeyDown(KeyCode.LeftShift)){
        speed = 1.5f;
        Invoke("StopRunning", 1);
      }
    }

    void FixedUpdate() {
      if(canMove){
        if(movementInput != Vector2.zero){
          transform.position = new Vector2(transform.position.x, transform.position.y) + movementInput * speed * Time.deltaTime;

          animator.SetBool("isMoving", true);
        } else {
          animator.SetBool("isMoving", false);
        }
        if(movementInput.x < 0) {
          spriteRenderer.flipX = true;
        } else if(movementInput.x > 0) {
          spriteRenderer.flipX = false;
        }
      }
    }

    void OnMove(InputValue movementValue) {
      movementInput = movementValue.Get<Vector2>();
    }

    void OnFire() {
      animator.SetTrigger("swordAttack");
    }

    public void SwordAttack() {
      LockMovement();

      if(spriteRenderer.flipX){
        swordAttack.AttackLeft();
      } else {
        swordAttack.AttackRight();      
      }     

    }
    public void EndSwordAttack(){
      UnlockMovement();
      swordAttack.StopAttack();
    }

    public void LockMovement(){
      canMove = false;
    }
    public void UnlockMovement(){
      canMove = true;
    }

    private void StopRunning(){
      speed = baseSpeed;
    }
}
