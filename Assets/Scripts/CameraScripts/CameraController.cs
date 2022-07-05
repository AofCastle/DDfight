using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Estructura singleton
    private static CameraController instance;
    public static CameraController Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    //---------------------------------

    public void MoveCameraToThisReference(int x)
    {
        CamReference[] references = FindObjectsOfType<CamReference>();
        for (int i = 0; i < references.Length; i++)
        {
            if (references[i].ReferenceType == x)
            {
                transform.position = references[i].transform.position;
            }
        }
    }
}
