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

    public void LoadMatch(int[] args)   //Esta funci�n debe ser llamada desde Main Menu
    {
        Time.timeScale = 0;
        sidesAreP1vsP2 = args[3] != 1; //args[3] siempre debe ser el int que decida el lado. Si ese int es 1 entonces el matchup ser� P2vsP1. Esta es la selecci�n de lado
        PvPmatch = args[4] == 0;        //args[4] resultar� en una pelea PvP en caso de ser 0 y una PvE en caso contrario. IMPORTANTE habr� que revisar este c�digo seguro...
        StartCoroutine(LoadCoroutine(args));
    }

    IEnumerator LoadCoroutine(int[] sceneList)
    {
        //Posible numero magico, de momento uso 3 en este for porque no tengo intenci�n de cargar m�s escenas que escenario y 2 personajes
        //Puede que necesite a�adir, al menos, una 4� escena que se encargue de estad�sticas, pero ya me preocupar� de eso en el futuro
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
        //IMPORTANTE posible n�mero m�gico en GetSceneAt, ahora mismo el orden es Pers, Stage, P1, P2. Por lo que en el array debe buscar Scene2 en P1 y Scene3 en P2
        GameObject[] foundObjects = SceneManager.GetSceneAt(2).GetRootGameObjects();
        //Si el objeto que encuentre tiene un script de jugador... Deber�a ser trivial, ya que las escenas de personaje deben tener al personaje como �nico GameObject ra�z
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
