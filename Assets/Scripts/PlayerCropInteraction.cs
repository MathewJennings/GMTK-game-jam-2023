using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCropInteraction : MonoBehaviour
{
    public Plot plot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("cropStuff")]
    public void interactWithCrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(plot);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Plot collidedPlot = collision.gameObject.GetComponent<Plot>();
        if (collidedPlot != null)
        {
            plot = collidedPlot;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Plot collidedPlot = collision.gameObject.GetComponent<Plot>();
        if (collidedPlot == plot)
        {
            plot = null;
        }
    }
}
