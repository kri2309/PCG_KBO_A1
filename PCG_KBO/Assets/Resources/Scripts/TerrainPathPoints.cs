using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPathPoints : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
       
    }
}
