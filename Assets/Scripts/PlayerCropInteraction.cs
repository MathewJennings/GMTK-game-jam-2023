using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void interactWithCrop()
    {
        Debug.Log("button pressed");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Plot collidedCrop = collision.gameObject.GetComponent<Plot>();
        if (collidedCrop != null)
        {
            plot = collidedCrop;
            Debug.Log(plot);
        }
    }
}
