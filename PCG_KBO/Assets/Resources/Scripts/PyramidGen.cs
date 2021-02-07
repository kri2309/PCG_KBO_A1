using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PyramidGen : MonoBehaviour
{
    [SerializeField]
    private float PyramidSize = 5f;

    private int Size = 4;

    // Update is called once per frame
    void Update()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshGen meshBuilder = new MeshGen(Size);

        //Add Points
        Vector3 top = new Vector3(0, PyramidSize, 0);
        Vector3 base0 = Quaternion.AngleAxis(0f, Vector3.up) * Vector3.forward * PyramidSize;
        Vector3 base1 = Quaternion.AngleAxis(240f, Vector3.up) * Vector3.forward * PyramidSize;
        Vector3 base2 = Quaternion.AngleAxis(120f, Vector3.up) * Vector3.forward * PyramidSize;


        //Build the triangles for our pyramid
        meshBuilder.BuildTriangle(base0, base1, base2, 0);
        meshBuilder.BuildTriangle(base1, base0, top, 1);
        meshBuilder.BuildTriangle(base2, top, base0, 2);
        meshBuilder.BuildTriangle(top, base2, base1, 3);
        meshFilter.mesh = meshBuilder.CreateMesh();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = MaterialsList().ToArray();
    }

    private List<Material> MaterialsList()
    {
        List<Material> materialsList = new List<Material>();

        Material red = new Material(Shader.Find("Specular"));
        red.color = Color.red;
       
        materialsList.Add(red);
    

        return materialsList;
    }
}
