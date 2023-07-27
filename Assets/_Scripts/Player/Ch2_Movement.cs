using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2_Movement : Movement
{
    public GameObject   Clock;
    private Animator  animator;
    public static Ch2_Movement instance;
    private float playerGravityModifier = 4f;
    public bool jump = false;


    protected void Awake()
    {
        animator = GetComponent<Animator>();
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        jumpForce = 12;
        moveSpeed = 4;
        Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y + playerGravityModifier, Physics.gravity.z);
    }
    
    private void Update()
    {
        float oxygenRatio = OxygenBar.instance.OxygenRatio;
        animator.SetFloat("Depletion", oxygenRatio);
        jump = jumpable;

#if UNITY_EDITOR
        xAxis = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            animator.SetBool("isSwimming", true);
        }
        else
        {
            animator.SetBool("isSwimming", false);
        }

#elif UNITY_ANDROID
        xAxis = joystick.Horizontal;

    if (joystick.JumpButtonPressed)
    {
        JumpInput();
        animator.Play("Jumping");
    }

    if (xAxis != 0)
    {
        animator.SetBool("isSwimming", true);
    }
    else
    {
        animator.SetBool("isSwimming", false);
    }
#endif

    }



    private void FixedUpdate()
    {
        base.Move();
        Jump();
        if (jumpable)
        {
            //ClockManager.instance.StartClockReload();
        }
    }

    protected override void Jump()
    {
        if (!jumpByKey || !jumpable || isTalking) return;

        jumpable = false;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        StartCoroutine(ResetJumpDelay());

    }

    IEnumerator ResetJumpDelay()
    {
        yield return new WaitForSeconds(0.2f);
        jumpable = true;
        jumpByKey = false;
    }

    public void EnterRipCurrent(Vector3 vec)
    {
        if (!Clock.activeSelf)
        {
            rigid.velocity = Vector3.zero;
            rigid.transform.Translate(vec * Time.deltaTime);
            jumpable = false;
            movable = false;
            rigid.useGravity = false;
        }
    }

    public void ExitRipCurrent()
    {
        jumpable = true;
        movable = true;
        rigid.useGravity = true;
    }

}
