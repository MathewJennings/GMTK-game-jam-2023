using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.DiscreteButtonControl;

public class useFarmTool : MonoBehaviour
{
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator =  GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*
private IEnumerator Die()
{
    myAnimator.duration();
    PlayAnimation(myAnimator, WrapeMode.ClampForever);
    yield return new WaitForSeconds(gameObject, GlobalSettings.animDeath1.length);
    Destroy(gameObject);
   
} */
}
