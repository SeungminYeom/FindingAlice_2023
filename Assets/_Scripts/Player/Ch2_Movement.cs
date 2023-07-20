using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2_Movement : Movement
{
    //private bool canJump = true;
    public Vector3 speedOffset = new Vector3(1, 0, 0);
    public GameObject Clock;


    private void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !jumpByKey && jumpable)
        {
            jumpByKey = true;
        }
    }

    private void FixedUpdate()
    {
        base.Move();
        Jump();
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

    void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "RipCurrent":
                if (!Clock.activeSelf)
                {
                    rigid.velocity = Vector3.zero;
                    rigid.transform.Translate(speedOffset * Time.deltaTime);
                    //rigid.velocity = speedOffset;
                    //rigid.AddForce(speedOffset, ForceMode.Impulse);
                    StateBeginShoot();
                    rigid.useGravity = false;
                }
                    break;
            default:
                return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "RipCurrent":
                jumpable = true;
                movable = true;
                rigid.useGravity = true;
                break;
            default:
                return;
        }
    }

}
