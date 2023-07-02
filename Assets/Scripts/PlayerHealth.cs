using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 5;
    public float invicibilityTime = 1.0f;
    private float vulnerableTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health >= 0)
        {
            Debug.Log("You died.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy" && Time.time > vulnerableTime)
        {
            health--;
            vulnerableTime = Time.time + invicibilityTime;
        }
    }
}
