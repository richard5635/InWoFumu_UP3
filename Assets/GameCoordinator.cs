using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator : MonoBehaviour
{
    public void StartDebug()
    {
        this.gameObject.SetActive(false);
        // Start Debug Mode
        Debug.Log("Debug Mode Started");
    }

    public void StartPython()
    {
        this.gameObject.SetActive(false);
        // Start Python Communication Mode
        Debug.Log("Python Mode Started");
    }
}
