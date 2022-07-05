using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchSetup : MonoBehaviour
{
    private bool sidesAreP1vsP2, PvPmatch;
    private GameObject p1, p2, leftStartingPosition, rightStartingPosition;


    //Estructura singleton
    private static MatchSetup instance;
    public static MatchSetup Instance
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

    public void LoadMatch(int[] args)   //Esta función debe ser llamada desde Main Menu
    {
        Time.timeScale = 0;
        sidesAreP1vsP2 = args[3] != 1; //args[3] siempre debe ser el int que decida el lado. Si ese int es 1 entonces el matchup será P2vsP1. Esta es la selección de lado
        PvPmatch = args[4] == 0;        //args[4] resultará en una pelea PvP en caso de ser 0 y una PvE en caso contrario. IMPORTANTE habrá que revisar este código seguro...
        StartCoroutine(LoadCoroutine(args));
    }

    IEnumerator LoadCoroutine(int[] sceneList)
    {
        //Posible numero magico, de momento uso 3 en este for porque no tengo intención de cargar más escenas que escenario y 2 personajes
        //Puede que necesite añadir, al menos, una 4ª escena que se encargue de estadísticas, pero ya me preocuparé de eso en el futuro
        for (int i = 0; i < 3; i++) 
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneList[i], LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        ContinueLoading();
    }
    private void ContinueLoading()
    {
        InitializeStage();
        InitializePlayer1();
        InitializePlayer2();
        //Pause.SetActive(false); //Desactivar la pantalla de carga, pseudocode
        CameraController.Instance.MoveCameraToThisReference(1);
        Time.timeScale = 1;
        MainMenu.Instance.LoadingHasFinished();
    }

    private void InitializeStage()
    {
        leftStartingPosition = SelectedSide.Instance.leftSide;
        rightStartingPosition = SelectedSide.Instance.rightSide;
    }
    private void InitializePlayer1()
    {
        //IMPORTANTE posible número mágico en GetSceneAt, ahora mismo el orden es Pers, Stage, P1, P2. Por lo que en el array debe buscar Scene2 en P1 y Scene3 en P2
        GameObject[] foundObjects = SceneManager.GetSceneAt(2).GetRootGameObjects();
        //Si el objeto que encuentre tiene un script de jugador... Debería ser trivial, ya que las escenas de personaje deben tener al personaje como único GameObject raíz
        for (int i = 0; i < foundObjects.Length; i++)
        {
            if (foundObjects[i].GetComponent<PlayerController>() != null)
            {
                p1 = foundObjects[i];
            }
        }
        //Si los lados son P1 vs P2 entonces 
        if (sidesAreP1vsP2)
        {
            p1.GetComponent<PlayerController>().InitializeVariables(leftStartingPosition.transform.position, sidesAreP1vsP2, true);
            //Siempre es true porque P1 siempre es el jugador, de momento no hay EvE
        }
        else
        {
            p1.GetComponent<PlayerController>().InitializeVariables(rightStartingPosition.transform.position, sidesAreP1vsP2, true);
        }
    }
    private void InitializePlayer2()
    {
        GameObject[] foundObjects = SceneManager.GetSceneAt(3).GetRootGameObjects();
        for (int i = 0; i < foundObjects.Length; i++)
        {
            if (foundObjects[i].GetComponent<PlayerController>() != null)
            {
                p2 = foundObjects[i];
            }
        }
        //IMPORTANTE En este if se niega sidesAreP1vsP2 porque la variable de destino es doIStartInLeftSide, por lo que p2 debe recibir el opuesto a p1
        if (sidesAreP1vsP2)
        {
            p2.GetComponent<PlayerController>().InitializeVariables(rightStartingPosition.transform.position, !sidesAreP1vsP2, PvPmatch);
        }
        else
        {
            p2.GetComponent<PlayerController>().InitializeVariables(leftStartingPosition.transform.position, !sidesAreP1vsP2, PvPmatch);
        }
    }
}
