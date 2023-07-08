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
            plot.plantSeed(seed);
        }
        else if (plot.needsWatering())
        {
            plot.waterPlot();
        }
        else if (plot.isMature())
        {
            Yield yield = plot.harvest();
            //STICK IT IN THE INVENTORY!!!!!
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
