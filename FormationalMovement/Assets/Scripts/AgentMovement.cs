using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
	[SerializeField] private Transform leader;

	//Distance from leader
	[SerializeField] private float zOffset;
	[SerializeField] private float xOffset;
	private float distanceFromFormation;
	//I increased it for better performance with multiple obstacles
	private float satisfactionRadius = 1.5f;
	private Vector3 formationPosition;
	private Vector3 direction;
	private Quaternion playerRot;
	private float rotSpeed = 2f;
	float speed = 5f;

	public GameObject[] obstacles;
	private float detectionRadius = 2f;
	private bool blocked = false;
	private float dodgeSpeed = 0.05f;
	private Vector3 direction2;
	private Vector3 dodgeDestination;

	//Agent components
	[SerializeField] private Transform trans;

	private void Update()
    {
		AvoidObstacle();
		Move();
    }

	//Avoid obstacle
	private void AvoidObstacle()
    {
		//Check if an obstacle is close
		obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach (GameObject obstacle in obstacles)
		{
			direction2 = obstacle.transform.position - trans.position;
			//If agent can see the obstacle ahead
			if(Vector3.Distance(obstacle.transform.position, trans.position) < detectionRadius && Vector3.Dot(leader.forward, direction2) > 0)
            {
				//Stop movement of agent
				blocked = true;
				//Check if obstacle is to the agent's left or right
				float dot = Vector3.Dot(trans.right, direction2);
				//If it is at the right move agent left and vice versa
				if(dot > 0)
                {
					trans.position += trans.right * -1f * dodgeSpeed;
				}
				else
                {
					trans.position += trans.right * dodgeSpeed;
				}
            }
            else
            {
				//resume normal movement
				blocked = false;
            }
		}
	}

	//Make agent follow player
	private void Move()
	{
		//Get target position
		formationPosition = leader.position - leader.forward * xOffset + leader.right * zOffset;

		//Distance between agent and his formation position along z axis
		distanceFromFormation = Vector3.Distance(trans.position, formationPosition);
		direction = formationPosition - trans.position;

		//If not within satisfaction radius
		if(distanceFromFormation > satisfactionRadius && !blocked)
        {
			//Rotate agent to look at leader
			playerRot = Quaternion.LookRotation(direction);
			//Slowly rotate towards target
			trans.rotation = Quaternion.Slerp(trans.rotation, playerRot, rotSpeed * Time.deltaTime);
			//Having a fixed speed works better as speed gets very slow when close to satisfaction radius if we do it similarly to the leader
			direction = direction.normalized * speed;
			//Walk to target position
			GetComponent<Rigidbody>().velocity = direction;
        }
        
	}
}


