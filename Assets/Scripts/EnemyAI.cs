using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform target; //taget we are walking to
    public float speed = 200f;
    public float nextwaypointDistance = 3f;// waypoint distance before we get new wappoint
    public Transform enemyGFX; // to turn around the image

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        seeker.StartPath(rb.position, target.position, onPathComplete);
        
    }
    void UpdatePath()
    {
        if(seeker.IsDone())
        seeker.StartPath(rb.position, target.position, onPathComplete); //seeker creates paths (current position,enemy position,function to do once comlete
    }

    void onPathComplete(Path p)// if no error our path = new path and we reset path generation
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()// if path is not there we return. trying fixed update
    {
        if (path == null)
            return;
        if(currentWaypoint >= path.vectorPath.Count)// if our current waypoint is greater than the total amount of waypoints
        {
            reachedEndOfPath = true;// we reached the and and wack the player.
            return;
        }
        else
        {
            reachedEndOfPath = false; //else we haven't reached the end.
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;//direction = current waypoint - current position. this points from our current position to the next waypoint.
        Vector2 force = direction * speed * Time.deltaTime;// force
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]); //distance between current position and next waypoint
        if ( distance < nextwaypointDistance)
        {
            currentWaypoint++;
        }
        
        // for changing the graphics
        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }

}
