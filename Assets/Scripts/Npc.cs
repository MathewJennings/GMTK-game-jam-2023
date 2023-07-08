using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public List<GameObject> waypoints;
    List<GameObject> worldSpaceWaypoints;
    public float moveSpeed;
    public int dialogIndex;
    public DialogDelegate dialogDelegate { set; get; }
    int waypointIndex;
    DayTimeController dayTimeController;
    private Animator animator;
    private GameObject player;
    private BarterManager barterManager;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        barterManager = GameObject.FindGameObjectWithTag("BarterManager").GetComponent<BarterManager>();
        worldSpaceWaypoints = new List<GameObject>();
        foreach(GameObject waypoint in waypoints)
        {
            var newWaypoint = new GameObject();
            //TransformPoint transforms from local space to world space
            newWaypoint.transform.position = newWaypoint.transform.TransformPoint(waypoint.transform.position);
            worldSpaceWaypoints.Add(newWaypoint);
        }
        waypointIndex = 0;
        dayTimeController = FindAnyObjectByType<DayTimeController>();
        transform.position = worldSpaceWaypoints[0].transform.position;
        animator = gameObject.GetComponent<Animator>();
    }

    public void SetFields(DialogDelegate dialogDelegate)
    {
        Debug.Log("setfields");
        /*
        this.waypoints = waypoints;
        this.moveSpeed = moveSpeed;
        this.dialogIndex = dialogIndex;
        */
        this.dialogDelegate = dialogDelegate;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            stopTrading();
        }
        GameObject nextWaypoint = worldSpaceWaypoints[waypointIndex];
        bool doNotMove = barterManager.IsTrading() || dayTimeController.isTimePaused;
        float maxDistanceDelta = doNotMove ? 0 : moveSpeed * Time.deltaTime;
        if(!doNotMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, maxDistanceDelta);
            if (transform.position == nextWaypoint.transform.position)
            {
                // walking left
                if (waypointIndex < dialogIndex)
                {
                    animator.SetBool("walkLeft", true);

                }
                //arrived at the dialogIndex
                else if (waypointIndex == dialogIndex)
                {
                    animator.SetBool("walkLeft", false);
                    animator.SetBool("walkRight", false);
                    dialogDelegate.Invoke();

                }
                waypointIndex++;
                if (waypointIndex >= worldSpaceWaypoints.Count)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void stopTrading()
    {
        barterManager.stopTrading();
        animator.SetBool("walkRight", true);
    }

}

public delegate void DialogDelegate();