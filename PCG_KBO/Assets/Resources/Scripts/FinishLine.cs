using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{    
    public int waypointsCrossed = 0;

    public string nextLevel;

    private void Update()
    {
        if (waypointsCrossed >= 3) {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
