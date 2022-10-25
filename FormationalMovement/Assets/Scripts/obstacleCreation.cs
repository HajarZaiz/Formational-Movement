using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleCreation : MonoBehaviour
{

    private Vector3 obstaclePosition;
    [SerializeField] private GameObject obstacle;
    private GameObject clone;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Set the target position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                obstaclePosition = hit.point + new Vector3(0, 0.5f, 0);
                clone = Instantiate(obstacle, obstaclePosition, Quaternion.identity);
                clone.tag = "Obstacle";
            }
        }
    }
}
