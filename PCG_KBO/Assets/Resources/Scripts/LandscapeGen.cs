using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGen : MonoBehaviour
{

    [SerializeField]
    private float cellsize = 1f;

    [SerializeField]
    private int width = 50, height = 50;

    [SerializeField]
    private int submeshSize = 1;

    [SerializeField]
    private float bumpyness = 5f;

    [SerializeField]
    private float bumpheight = 5f;

    // Update is called once per frame
    void Update()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshGen meshBuilder = new MeshGen(submeshSize);

        CreateLandscape(meshBuilder);

        meshFilter.mesh = meshBuilder.CreateMesh();
    }

    private void CreateLandscape(MeshGen meshBuilder)
    {
        Vector3[,] points = new Vector3[width, height];

        //1. Generate the points in our plane
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                points[x, y] = new Vector3(cellsize * x,
                                          Mathf.PerlinNoise(
                                             x * bumpyness * 0.1f, y * bumpyness * 0.1f) * bumpheight,
                                         cellsize * y);

                //points[x, y] = new Vector3(cellsize * x,
                //                          Mathf.PerlinNoise(
                //                            (x + Time.time) * bumpyness * 0.1f, (y + Time.time) * bumpyness * 0.1f) * bumpheight,
                 //                        cellsize * y);
            }
        }

        int submesh = 0;

        //2. Generate the quads
        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                //2.1 Generate the points of each quad
                Vector3 br = points[x, y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x, y + 1];
                Vector3 tl = points[x + 1, y + 1];

                //2.2 Generate the triangles to create the quad
                meshBuilder.BuildTriangle(bl, tr, tl, submesh % submeshSize);
                meshBuilder.BuildTriangle(bl, br, tr, submesh % submeshSize);
            }

            submesh++;
        }

    }
}
