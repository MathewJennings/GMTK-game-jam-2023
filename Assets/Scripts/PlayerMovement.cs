using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float defaultSpeed;
    [SerializeField] float crippleSpeed;
    [SerializeField] float crippleDuration;
    [SerializeField] GameObject originalStartPosition;

    private DayTimeController dayTimeController;

    private float crippleTime;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private bool allowMovement;

    // Start is called before the first frame update
    public void Start()
    {
        dayTimeController = GameObject.FindObjectOfType<DayTimeController>();
        defaultSpeed = 2.8f;
        crippleSpeed = 1.4f;

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

    [ContextMenu("Cripple Movement")]
    public void CrippleMovement()
    {
        crippleTime = dayTimeController.getCurrentTimeSeconds() + crippleDuration;
    }

    public bool IsCrippled()
    {
        return dayTimeController.getCurrentTimeSeconds() < crippleTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = IsCrippled() ? crippleSpeed : defaultSpeed;
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
