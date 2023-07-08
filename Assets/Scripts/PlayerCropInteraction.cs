using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCropInteraction : MonoBehaviour
{
    public Plot plot;
    public Seed seed;
    public PlayerManager playerManager;

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
            serviceCrop();
        }

        
    }

    private void serviceCrop()
    {
        if (plot == null)
        {
            return;
        }

        if (plot.isEmpty())
        {
            //WE SHOULD GET THE SEED FROM UI!!!!
            if (playerManager.canAffordAction(2)) {
                playerManager.ChangeAp(-2);
                plot.plantSeed(seed);
            } else
            {
                Debug.Log("You are out of energy and can not perform that action!");
            }
        }
        else if (plot.needsWatering())
        {
            if (playerManager.canAffordAction(1))
            {
                playerManager.ChangeAp(-1);
                plot.waterPlot();
            } else
            {
                Debug.Log("You are out of energy and can not perform that action!");
            }

        }
        else if (plot.isMature())
        {
            if (playerManager.canAffordAction(3)) { 
                playerManager.ChangeAp(-3);
                Yield yield = plot.harvest();
                //STICK IT IN THE INVENTORY!!!!!
            } else
            {
                Debug.Log("You are out of energy and can not perform that action!");
            }
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
