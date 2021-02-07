using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CubeGen : MonoBehaviour
{
    [SerializeField]
    private Vector3 size = Vector3.one;

    [SerializeField]
    private int Size = 6;

    public List<Material> allMaterials;

    
    void Update()
    {
        
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        
        MeshGen meshBuilder = new MeshGen(Size);


        //Building the cube

        //top vertices
        Vector3 t0 = new Vector3(size.x, size.y, -size.z); // top left point
        Vector3 t1 = new Vector3(-size.x, size.y, -size.z); // top right point
        Vector3 t2 = new Vector3(-size.x, size.y, size.z); //bottom right point of top square
        Vector3 t3 = new Vector3(size.x, size.y, size.z); // bottom left point of top square

        //bottom vertices
        Vector3 b0 = new Vector3(size.x, -size.y, -size.z); // bottom left point
        Vector3 b1 = new Vector3(-size.x, -size.y, -size.z); // bottom right point
        Vector3 b2 = new Vector3(-size.x, -size.y, size.z); //bottom right point of bottom square
        Vector3 b3 = new Vector3(size.x, -size.y, size.z); // bottom left point of bottom square


        //top square
        meshBuilder.BuildTriangle(t0, t1, t2, 0);
        meshBuilder.BuildTriangle(t0, t2, t3, 0);

        //bottom square
        meshBuilder.BuildTriangle(b2, b1, b0, 1);
        meshBuilder.BuildTriangle(b3, b2, b0, 1);


        //back square
        meshBuilder.BuildTriangle(b0, t1, t0, 2);
        meshBuilder.BuildTriangle(b0, b1, t1, 2);


        //left-side square
        meshBuilder.BuildTriangle(b1, t2, t1, 3);
        meshBuilder.BuildTriangle(b1, b2, t2, 3);


        //right-side square
        meshBuilder.BuildTriangle(b2, t3, t2, 4);
        meshBuilder.BuildTriangle(b2, b3, t3, 4);

        //front square
        meshBuilder.BuildTriangle(b3, t0, t3, 5);
        meshBuilder.BuildTriangle(b3, b0, t0, 5);

        meshFilter.mesh = meshBuilder.CreateMesh();


        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        AddMaterials();
        meshRenderer.materials = allMaterials.ToArray();
    }

    private void AddMaterials()
    {
        Material yellow = new Material(Shader.Find("Specular"));
        yellow.color = Color.yellow;
        Material white = new Material(Shader.Find("Specular"));
        white.color = Color.white;
        Material magenta = new Material(Shader.Find("Specular"));
        magenta.color = Color.magenta;
        Material red = new Material(Shader.Find("Specular"));
        red.color = Color.red;
        Material blue = new Material(Shader.Find("Specular"));
        blue.color = Color.blue;
        Material green = new Material(Shader.Find("Specular"));
        green.color = Color.green;
      

        
        allMaterials = new List<Material>();
        allMaterials.Add(white);
        allMaterials.Add(blue);
        allMaterials.Add(green);
        allMaterials.Add(magenta);
        allMaterials.Add(yellow);
        allMaterials.Add(red);
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, size*2);
    }
}
