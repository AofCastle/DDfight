using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private static MainMenu instance;
    private Scene mainScene;
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

    // Corrutina para cargar peleas
    IEnumerator LoadMatch(string[] args)
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
        for (int i = 0; i < args.Length; i++)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(args[i], LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        ContinueLoading();
    }

    private void ContinueLoading()
    {
        //Pause.SetActive(false); //Desactivar la pantalla de carga, pseudocode
        //Time.timeScale = 1;      
    }
}
