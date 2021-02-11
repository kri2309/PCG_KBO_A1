using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public GameObject[] startPoints, endPoints;
    GameObject player, startObject, endObject, playerRef, endRef;
    public GameObject winText, maze;

    private void Start()
    {
        player = Resources.Load("Prefabs/Player") as GameObject;
        startObject = Resources.Load("Prefabs/Start") as GameObject;
        endObject = Resources.Load("Prefabs/End") as GameObject;

        //Activate maze, (each maze piece uses the cube generator)
        maze.SetActive(true);

        int start = Random.Range(0, startPoints.Length);//creating random startpoint from array
        Instantiate(startObject, startPoints[start].transform.position, Quaternion.identity);
        playerRef = Instantiate(player, startPoints[start].transform.position + Vector3.up, Quaternion.identity); //putting player on start point

        int end = Random.Range(0, endPoints.Length); //getting random end point from array
        endRef = Instantiate(endObject, endPoints[end].transform.position, Quaternion.identity); //setting the finish point to the end ref
    }

    private void Update()
    {
        //checking if the player is at end point to activate the win text
        if (Vector3.Distance(playerRef.transform.position, endRef.transform.position) < 2)
        {
            winText.SetActive(true);
        }
    }
}
