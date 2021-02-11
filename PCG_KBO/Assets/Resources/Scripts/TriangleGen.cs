using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TriangleGen : MonoBehaviour
{
    [SerializeField]
    private Vector3 size = Vector3.one;


    void Update()
    {

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();


        MeshGen meshGen = new MeshGen(1);


        Vector3 p0 = new Vector3(size.x,  size.y, -size.z);
        Vector3 p1 = new Vector3(-size.x, size.y, -size.z);
        Vector3 p2 = new Vector3(-size.x, size.y,  size.z);

        meshGen.BuildTriangle(p0, p1, p2, 0); //3 points and submesh index = material

        meshFilter.mesh = meshGen.CreateMesh();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();

        Material yellow = new Material(Shader.Find("Specular"));
        yellow.color = Color.yellow;
        meshRenderer.material = yellow;
    }


}
