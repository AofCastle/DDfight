using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    //Estos 4 int son las posiciones en la build de las escenas de personajes y escenarios. IMPORTANTE QUE ESTÉN ACTUALIZADOS
    //Estos 4 int nos permitirán hacer selecciones aleatorias, entre otras cosas.
    public int indexOfFirstCharacter, indexOfLastCharacter, indexOfFirstStage, indexOfLastStage;
    public GameObject mainButtons, loadingScreen;

    private Scene mainScene;

    //Estructura singleton
    private static MainMenu instance;
    public static MainMenu Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            mainScene = SceneManager.GetActiveScene();
        }
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
    }


    IEnumerator UnloadAllScenes()      // Corrutina para descargar todas las escenas que no sean el menu principal
    {
        Time.timeScale = 0;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i) != mainScene)
            {
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                while (!asyncUnload.isDone)
                {
                    yield return null;
                }
            }
        }
        CameraController.Instance.MoveCameraToThisReference(0);
        mainButtons.SetActive(true);
        LoadingHasFinished();
    }

    public void LoadingHasFinished()
    {
        loadingScreen.SetActive(false);
    }

    public void ToTraining()
    {
        mainButtons.SetActive(false);
        int[] args = { indexOfFirstStage, indexOfFirstCharacter, indexOfLastCharacter, 0, 0 };
        MatchSetup.Instance.LoadMatch(args);
    }
}
