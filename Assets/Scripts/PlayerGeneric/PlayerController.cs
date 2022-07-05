using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool initializedStatus=false; //Para evitar bugs relacionados con una doble inicialización
    private bool AmIHuman; //Variable utilizada para activar la IA cuando se carga una partida
    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    public void InitializeVariables(Vector3 startingPosition, bool doIStartInLeftSide, bool solvedCaptcha)
    {
        if (!initializedStatus)
        {
            AmIHuman = solvedCaptcha;
            if (doIStartInLeftSide)
            {
                body.rotation = Quaternion.Euler(0, -90, 0);
            }
            else
            {
                body.rotation = Quaternion.Euler(0, 90, 0);
            }
            body.transform.position = startingPosition;
            initializedStatus = true;
        }
        else
        {
            Debug.LogError("Me están inicializando dos veces");
        }
    }

}
