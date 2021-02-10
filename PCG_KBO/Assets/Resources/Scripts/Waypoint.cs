using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Waypoint : MonoBehaviour
{
    public bool crossed = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.tag == "Player")
        {
            if (!crossed)
            {
                crossed = true;
                FindObjectOfType<FinishLine>().waypointsCrossed++;
            }
        }
    }
}
