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

    void Awake()
    {
        Debug.Log("awake");
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
        GameObject nextWaypoint = worldSpaceWaypoints[waypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, dayTimeController.isTimePaused ? 0 : moveSpeed * Time.deltaTime);
        if (transform.position == nextWaypoint.transform.position)
        {
            //arrived at the dialogIndex
            if (dialogIndex == waypointIndex)
            {
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

public delegate void DialogDelegate();