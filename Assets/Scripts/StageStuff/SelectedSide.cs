using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSide : MonoBehaviour
{
    public GameObject leftSide,rightSide;
    //Utilizo aqu� esta estructura porque s�lo deber�a haber un escenario cargado
    //Estructura singleton
    private static SelectedSide instance;
    public static SelectedSide Instance
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
}
