using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen //helper class
{
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> indices = new List<int>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();

    private List<int>[] submeshIndices = new List<int>[] { }; // an array of submesh indices

    //procedurally generates meshes
    public MeshGen(int submeshCount)
    {
        submeshIndices = new List<int>[submeshCount];

        for (int i = 0; i < submeshCount; i++)
        {
            submeshIndices[i] = new List<int>();
        }
    }
    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int subMesh)
    {
        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized; //calculating the normal 
        BuildTriangle(p0, p1, p2, normal, subMesh);  //passing on 3 points of triangle and normal


    }



    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal, int subMesh)
    {
        //actually creating the triangle 

        int p0Index = vertices.Count;
        int p1Index = vertices.Count + 1;
        int p2Index = vertices.Count + 2;

        indices.Add(p0Index);
        indices.Add(p1Index);
        indices.Add(p2Index);


        submeshIndices[subMesh].Add(p0Index);
        submeshIndices[subMesh].Add(p1Index);
        submeshIndices[subMesh].Add(p2Index);

        //Adding the points
        vertices.Add(p0); 
        vertices.Add(p1); 
        vertices.Add(p2);

        //Direction each vertex is facing
        normals.Add(normal);
        normals.Add(normal);
        normals.Add(normal);

        //Texture coords
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
    }


    public Mesh CreateMesh() // build our mesh
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();

        mesh.triangles = indices.ToArray();

        mesh.normals = normals.ToArray();

        mesh.uv = uvs.ToArray();

        mesh.subMeshCount = submeshIndices.Length;

        for (int i = 0; i < submeshIndices.Length; i++)
        {
            if (submeshIndices[i].Count < 3)
            {
                mesh.SetTriangles(new int[3] { 0, 0, 0 }, i);
            }
            else
            {
                mesh.SetTriangles(submeshIndices[i].ToArray(), i);
            }
        }

        return mesh;
    }

}
