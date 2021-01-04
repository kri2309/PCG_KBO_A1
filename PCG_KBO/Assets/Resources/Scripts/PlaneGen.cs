using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PlaneGen : MonoBehaviour
{

    [SerializeField]
    private float cellSize = 1f;

    [SerializeField]
    private int width = 24;

    [SerializeField]
    private int height = 24;

    [SerializeField]
    private int Size = 6;

    
    void Update()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshGen meshBuilder = new MeshGen(Size);

        //Creating the points of the plane
        Vector3[,] points = new Vector3[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                points[x, y] = new Vector3(
                        cellSize * x,
                        0,
                        cellSize * y);
            }
        }

        //create the quads

        int submesh = 0;

        for(int x = 0; x < width - 1; x++)
        {
            for(int y = 0; y < height - 1; y++)
            {
                Vector3 br = points[x ,    y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x ,    y + 1];
                Vector3 tl = points[x + 1, y + 1];

                //create 2 triangles that make up a quad
                meshBuilder.BuildTriangle(bl,tr,tl, submesh % Size);
                meshBuilder.BuildTriangle(bl,br,tr, submesh % Size);
            }

            submesh++;
        }

        meshFilter.mesh = meshBuilder.CreateMesh();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = MaterialsList().ToArray();
    }


    private List<Material> MaterialsList()
    {
        List<Material> materialsList = new List<Material>();

        Material blue = new Material(Shader.Find("Specular"));
        blue.color = Color.blue;

        Material red = new Material(Shader.Find("Specular"));
        red.color = Color.red;

        Material green = new Material(Shader.Find("Specular"));
        green.color = Color.green;

        
        Material yellow = new Material(Shader.Find("Specular"));
        yellow.color = Color.yellow;

        Material magenta = new Material(Shader.Find("Specular"));
        magenta.color = Color.magenta;

        Material white = new Material(Shader.Find("Specular"));
        white.color = Color.white;

        materialsList.Add(red);
        materialsList.Add(green);
        materialsList.Add(blue);
        materialsList.Add(yellow);
        materialsList.Add(magenta);
        materialsList.Add(white);

        return materialsList;
    }

}
