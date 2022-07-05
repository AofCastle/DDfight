using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSide : MonoBehaviour
{
    public GameObject leftSide,rightSide;
    //Utilizo aquí esta estructura porque sólo debería haber un escenario cargado
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
