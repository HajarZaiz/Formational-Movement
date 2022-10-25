using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicArrive : MonoBehaviour
{
    Vector3 targetPosition;
    Vector3 lookAtTarget;
    Quaternion playerRot;
    float rotSpeed = 5f;
    float speed;
    float maxSpeed = 10f;
    bool moving = false;
    float satisfactionRadius = 0.5f;
    float timeToTarget = 0.5f;

    //Dodging obstacles variables
    private GameObject[] obstacles;
    private float detectionRadius = 2f;
    private bool blocked = false;
    private float dodgeSpeed = 0.05f;
    private Vector3 direction2;

    // Update is called once per frame
    void Update()
    {
        AvoidObstacle();
        Move();
    }
    
    void Move()
    {
        //Upon mouse left click
        if (Input.GetMouseButton(0))
        {
            //Set the target position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                targetPosition = hit.point;
                lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y, targetPosition.z - transform.position.z);
                playerRot = Quaternion.LookRotation(lookAtTarget);
                moving = true;
            }

        }
        //If not within satisfaction radius get to it
        if (moving && !blocked)
        {
            //Slowly rotate towards target
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, rotSpeed * Time.deltaTime);
            //Clamp speed
            float distance = Vector3.Distance(transform.position, targetPosition);
            speed = distance / timeToTarget;
            if(speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            //Walk to target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            //Set degree of satisfaction and continuously check if it is verified to stop movement
            if (distance < satisfactionRadius)
                moving = false;
        }


    }

    //Avoid obstacle
    private void AvoidObstacle()
    {
        //Check if an obstacle is close
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            direction2 = obstacle.transform.position - transform.position;
            //If agent can see the obstacle ahead
            if (Vector3.Distance(obstacle.transform.position, transform.position) < detectionRadius && Vector3.Dot(transform.forward, direction2) > 0)
            {
                //Stop movement of agent
                blocked = true;
                //Check if obstacle is to the agent's left or right
                float dot = Vector3.Dot(transform.right, direction2);
                //If it is at the right move agent left and vice versa
                if (dot > 0)
                {
                    transform.position += transform.right * -1f * dodgeSpeed;
                }
                else
                {
                    transform.position += transform.right * dodgeSpeed;
                }
            }
            else
            {
                //resume normal movement
                blocked = false;
            }
        }
    }
}


