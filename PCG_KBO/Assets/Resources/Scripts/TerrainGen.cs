using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TerrainTextureData
{
    public Texture2D terrainTexture;
    public float minHeight;
    public float maxHeight;
    public Vector2 tileSize;
}

[System.Serializable]
class TreeData
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}


[ExecuteInEditMode]
public class TerrainGen : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    //options in editor

    [SerializeField]
    private bool generatePerlinNoiseTerrain = true;

    [SerializeField]
    private bool addTexture = false;

    [SerializeField]
    private bool removeTexture = false;

    [SerializeField]
    private bool addTree = false;

    //variables for generating terrain using random values
    [SerializeField]
    private float minRandomHeightRange = 0f;

    [SerializeField]
    private float maxRandomHeightRange = 0.01f;

    //variables for generating terrain using perlin noise
    [SerializeField]
    private float perlinNoiseWidthScale = 0.01f;

    [SerializeField]
    private float perlinNoiseHeightScale = 0.01f;

    [SerializeField]
    private float perlinNoiseOffsetWidth = 1, perlinNosieOffsetHeight = 1;

    //variables for adding textures to our terrain
    [SerializeField]
    private List<TerrainTextureData> terrainTextureDataList;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

    [SerializeField]
    private List<TreeData> treeDataList;

    [SerializeField]
    private int maxTrees = 2000;

    [SerializeField]
    private int treeSpacing = 10;

    [SerializeField]
    private List<TreeData> detailsDataList;

    [SerializeField]
    private int maxDetails = 10000;

    [SerializeField]
    private int detailSpacing = 3;

    [SerializeField]
    private float randomXRange = 5.0f;

    [SerializeField]
    private float randomZRange = 5.0f;

    [SerializeField]
    private int terrainLayerIndex = 8;

    [SerializeField]
    private GameObject water;

    [SerializeField]
    private float waterHeight = 0.2f;

    [SerializeField]
    private bool addWater;

    [SerializeField]
    private bool addDetails;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;

        CreateTerrain();
    }

    void initialise() //to remove any errors in the scene 
    {
#if UNITY_EDITOR

        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
        }

        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }

#endif
    }
   

    //Updates when not in playmode
    private void OnValidate()
    {
        initialise();

        //Using all the settings set in the inspector, generate certain elements of the terrain

        if ( generatePerlinNoiseTerrain)
        {
            CreateTerrain();
        }

        if (removeTexture)
        {
            addTexture = false;
        }

        if (addTexture || removeTexture)
        {
            TerrainTexture();
        }

        if (addTree)
        {
            AddTree();
        }

        if (addWater)
        {
            AddWater();
        }

        if (addDetails)
        {
            AddDetails();
        }
    }

    void CreateTerrain()
    {
        ////gets the height map data that already exists in the terrain and loads it into a 2D array
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
               
                if (generatePerlinNoiseTerrain)
                {
                    heightMap[width, height] = Mathf.PerlinNoise(width * perlinNoiseWidthScale, height * perlinNoiseHeightScale);//Generates smooth terrain based on perlin noise functions (looks good, smoother)
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);//Applying heightmap to the terrain
    }

    //add textures to the terrain
    void TerrainTexture()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureDataList.Count];

        for (int i = 0; i < terrainTextureDataList.Count; i++)//Goes through all textures applied in inspector
        {
            if (addTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = terrainTextureDataList[i].terrainTexture;
                terrainLayers[i].tileSize = terrainTextureDataList[i].tileSize;
            }
            else if (removeTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;
            }
        }

        terrainData.terrainLayers = terrainLayers;//Adds layers chosen to terrain editor


        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);//Gets heightmap of terrain

        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] splatmap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureDataList.Count; i++)
                {
                    float minHeight = terrainTextureDataList[i].minHeight - terrainTextureBlendOffset;
                    float maxHeight = terrainTextureDataList[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= minHeight && heightMap[width, height] <= maxHeight)
                    {
                        splatmap[i] = 1;
                    }
                }

                NormaliseSplatMap(splatmap);

                for (int j = 0; j < terrainTextureDataList.Count; j++)
                {
                    alphaMapList[width, height, j] = splatmap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMapList);//Applies textures
    }


    void NormaliseSplatMap(float[] splatmap) //when you overlap 2 textures on top on eachother 
    {
        float total = 0;

        for (int i = 0; i < splatmap.Length; i++)
        {
            total += splatmap[i];
        }

        for (int i = 0; i < splatmap.Length; i++)
        {
            splatmap[i] = splatmap[i] / total;
        }
    }

    void AddTree()
    {
        TreePrototype[] trees = new TreePrototype[treeDataList.Count]; //getting trees from inspector 

        for (int i = 0; i < treeDataList.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeDataList[i].treeMesh;
        }

     
        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        if (addTree)
        {
            for (int z = 0; z < terrainData.size.z; z += treeSpacing)
            {
                for (int x = 0; x < terrainData.size.x; x += treeSpacing)
                {
                    for (int treePrototypeIndex = 0; treePrototypeIndex < trees.Length; treePrototypeIndex++)
                    {
                        if (treeInstanceList.Count < maxTrees)
                        {
                            float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                            if (currentHeight >= treeDataList[treePrototypeIndex].minHeight &&
                               currentHeight <= treeDataList[treePrototypeIndex].maxHeight)
                            {
                                float randomX = (x + UnityEngine.Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                                float randomZ = (z + UnityEngine.Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                                TreeInstance treeInstance = new TreeInstance();

                                treeInstance.position = new Vector3(randomX, currentHeight, randomZ);

                                Vector3 treePosition = new Vector3(treeInstance.position.x * terrainData.size.x,
                                                                    treeInstance.position.y * terrainData.size.y,
                                                                    treeInstance.position.z * terrainData.size.z) + this.transform.position;

                                RaycastHit raycastHit; 
                                int layerMask = 1 << terrainLayerIndex;
                                //using raycast to put trees on terrain 
                                if (Physics.Raycast(treePosition, Vector3.down, out raycastHit, 100, layerMask) ||
                                    Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                                {
                                    float treeHeight = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                    treeInstance.position = new Vector3(treeInstance.position.x, treeHeight, treeInstance.position.z);

                                    treeInstance.rotation = UnityEngine.Random.Range(0, 360);
                                    treeInstance.prototypeIndex = treePrototypeIndex;
                                    treeInstance.color = Color.white;
                                    treeInstance.lightmapColor = Color.white;
                                    treeInstance.heightScale = 0.95f;
                                    treeInstance.widthScale = 0.95f;

                                    treeInstanceList.Add(treeInstance); //adding the tree
                                }
                            }
                        }
                    }
                }
            }
        }

        terrainData.treeInstances = treeInstanceList.ToArray();
    }

    void AddWater()
    {
        GameObject waterGameObject = GameObject.Find("Water"); 

        if (!waterGameObject) //create game object if not created
        {
            waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
            waterGameObject.name = "Water";
        }

        waterGameObject.transform.position = this.transform.position + new Vector3(terrainData.size.x / 2, waterHeight * terrainData.size.y, terrainData.size.z / 2);

        waterGameObject.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }

    void AddDetails()
    {
        
    }
}
