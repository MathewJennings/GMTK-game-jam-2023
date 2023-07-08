using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float speed;

    private Rigidbody2D rigidbody;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
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
