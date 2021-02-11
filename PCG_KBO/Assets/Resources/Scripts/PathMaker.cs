using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PathMaker : MonoBehaviour
{

    [SerializeField]
    private int startingPathCount;
    private int nextPathDirection;
    [SerializeField]
    private Transform currentPath;
    [SerializeField]

    private GameObject pathPrefab;

    void Start()
    {
        GeneratestartingPath();
    }

    void GeneratestartingPath()
    {
        for (int i = 0; i < startingPathCount; i++)
        {
            nextPathDirection = Random.Range(0, 2);
            if(nextPathDirection == 0)
            {
                currentPath = Instantiate(pathPrefab, currentPath.position + Vector3.right * 15, Quaternion.identity).transform;
            }
            else
            {
                currentPath = Instantiate(pathPrefab, currentPath.position - Vector3.forward * 15, Quaternion.identity).transform;
            }
        }
    }
}
