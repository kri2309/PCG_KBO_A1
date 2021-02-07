using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathShape
{
    public Vector3[] shape = new Vector3[] { -Vector3.up, Vector3.up, -Vector3.up };

}

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class PathMaker : MonoBehaviour
{

    [SerializeField]
    private int subMeshSize = 1;

    [SerializeField]
    private Transform[] path;

    [SerializeField]
    private PathShape pathShape;

    // Update is called once per frame
    void Update()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshGen meshBuilder = new MeshGen(subMeshSize);

        Vector3[] prevShape = TranslateShape(
            path[path.Length - 1].transform.position,
            (path[0].transform.position - path[path.Length - 1].transform.position).normalized,
            pathShape
            );

        for (int i = 0; i < path.Length; i++)
        {
            Vector3[] currentShape = TranslateShape(
            path[i].transform.position,
            (path[(i + 1) % path.Length].transform.position - path[i].transform.position).normalized,
            pathShape
            );

            for (int j = 0; j < currentShape.Length - 1; j++)
            {
                meshBuilder.BuildTriangle(prevShape[j], currentShape[j], currentShape[j + 1], 0);
                meshBuilder.BuildTriangle(prevShape[j + 1], prevShape[j], currentShape[j + 1], 0);
            }

            prevShape = currentShape;
        }


        meshFilter.mesh = meshBuilder.CreateMesh();
    }

    private Vector3[] TranslateShape(Vector3 point, Vector3 forward, PathShape shape)
    {
        Vector3[] translatedShape = new Vector3[shape.shape.Length];

        Quaternion forwardRotation = Quaternion.LookRotation(forward);

        for (int i = 0; i < shape.shape.Length; i++)
        {
            translatedShape[i] = (forwardRotation * shape.shape[i]) + point;
        }

        return translatedShape;
    }
}
