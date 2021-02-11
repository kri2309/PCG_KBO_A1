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
    private int subMeshSize = 6;

    // Update is called once per frame
    void Update()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshGen meshBuilder = new MeshGen(subMeshSize);

        //create points of our plane
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

        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                Vector3 br = points[x, y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x, y + 1];
                Vector3 tl = points[x + 1, y + 1];

                //create 2 triangles that make up a quad
                meshBuilder.BuildTriangle(bl, tr, tl, submesh % subMeshSize);
                meshBuilder.BuildTriangle(bl, br, tr, submesh % subMeshSize);
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

        Material redMat = new Material(Shader.Find("Specular"));
        redMat.color = Color.red;

        Material greenMat = new Material(Shader.Find("Specular"));
        greenMat.color = Color.green;

        Material blueMat = new Material(Shader.Find("Specular"));
        blueMat.color = Color.blue;

        Material yellowMat = new Material(Shader.Find("Specular"));
        yellowMat.color = Color.yellow;

        Material magentaMat = new Material(Shader.Find("Specular"));
        magentaMat.color = Color.magenta;

        Material whiteMat = new Material(Shader.Find("Specular"));
        whiteMat.color = Color.white;

        materialsList.Add(redMat);
        materialsList.Add(greenMat);
        materialsList.Add(blueMat);
        materialsList.Add(yellowMat);
        materialsList.Add(magentaMat);
        materialsList.Add(whiteMat);

        return materialsList;
    }

}
