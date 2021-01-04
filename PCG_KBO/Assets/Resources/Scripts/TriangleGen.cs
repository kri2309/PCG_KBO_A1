using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TriangleGen : MonoBehaviour
{
    [SerializeField]
    private Vector3 size = Vector3.one;

    public List<Material> materialList;


    void Update()
    {

        MeshFilter meshFilter = this.GetComponent<MeshFilter>();


        MeshGen meshBuilder = new MeshGen(1);


        Vector3 p0 = new Vector3(size.x,  size.y, -size.z);
        Vector3 p1 = new Vector3(-size.x, size.y, -size.z);
        Vector3 p2 = new Vector3(-size.x, size.y,  size.z);

        meshBuilder.BuildTriangle(p0, p1, p2, 0);

        meshFilter.mesh = meshBuilder.CreateMesh();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialList.ToArray();
    }
}
