using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator : MonoBehaviour
{
    public DebugModeController DebugController;
    public SerialLight SerialController;
    public void StartDebug()
    {
        // Start Debug Mode
        Debug.Log("Debug Mode Started");
        DebugController.enabled = true;
        this.gameObject.SetActive(false);
    }

    public void StartPython()
    {
        // Start Python Communication Mode
        Debug.Log("Python Mode Started");
        this.gameObject.SetActive(false);
    }

    public void StartArduino()
    {
        Debug.Log("Arduino Mode Started");
        SerialController.enabled = true;
        this.gameObject.SetActive(false);
    }
}
