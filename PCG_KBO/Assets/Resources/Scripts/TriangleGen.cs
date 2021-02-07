﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TriangleGen : MonoBehaviour
{
    [SerializeField]
    private Vector3 size = Vector3.one;

    public List<Material> allMaterials;


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
}
