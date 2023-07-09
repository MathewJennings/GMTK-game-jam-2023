using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] GameObject originalStartPosition;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private bool allowMovement;

    // Start is called before the first frame update
    public void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        allowMovement = true;
        ResetToOriginalStartPosition();
    }

    public void ResetToOriginalStartPosition()
    {
        gameObject.transform.position = originalStartPosition.transform.position;
    }

    public void ResetToOriginalStartPositionAfterDelay()
    {
        StartCoroutine(WaitAndResetToOriginalStartPosition());
    }

    IEnumerator WaitAndResetToOriginalStartPosition()
    {
        yield return new WaitForSeconds(3f);
        ResetToOriginalStartPosition();
    }


    public void setAllowMovement(bool a)
    {
        allowMovement = a;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h;
        float v;
        if (!allowMovement || GetComponentInChildren<useFarmTool>() != null)
        {
            h = 0;
            v = 0;
        } else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        if (h > 0)
        {
            animator.SetBool("walkRight", true);
        }
        else if (h < 0)
        {
            animator.SetBool("walkLeft", true);
        }
        else
        {
            animator.SetBool("walkRight", false);
            animator.SetBool("walkLeft", false);
        }
        if (v > 0)
        {
            animator.SetBool("walkUp", true);
        } else if (v < 0)
        {
            animator.SetBool("walkDown", true);
        }
        else
        {
            animator.SetBool("walkUp", false);
            animator.SetBool("walkDown", false);
        }
        rigidbody.velocity = new Vector2(h * speed, v * speed);
    }

}
