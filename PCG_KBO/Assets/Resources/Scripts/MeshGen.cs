using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen //helper class
{
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> indices = new List<int>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();

    private int submeshCount;


    //procedurally generates meshes
    public MeshGen(int submeshCount)
    {
        this.submeshCount = submeshCount;
    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int subMesh)
    {
        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
        BuildTriangle(p0, p1, p2, normal, subMesh);
    }



    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal, int subMesh)
    {
        int p0Index = vertices.Count;
        int p1Index = vertices.Count + 1;
        int p2Index = vertices.Count + 2;

        indices.Add(p0Index);
        indices.Add(p1Index);
        indices.Add(p2Index);


        vertices.Add(p0); 
        vertices.Add(p1); 
        vertices.Add(p2);


        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);


        uvs.Add(new Vector2(0, 0));
       uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();

        mesh.triangles = indices.ToArray();

        mesh.normals = normals.ToArray();

        mesh.uv = uvs.ToArray();

        return mesh;
    }

}
