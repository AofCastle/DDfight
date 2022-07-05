using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private static MenuController instance;
    public string mainScene;

    public GameObject[] menus;  //0-Main, 1-settings, 2-loading, 3-pause, 4-debug
    public GameObject player;
    public GameObject playerSprite;
    //public CheckPoints playerSpawns;
    private int checkpointNo=0;
    bool isPaused;
    public GameObject pausedPanel;
    //private PlayerHealth health;
    public GameObject[] animators; //0-Menu, 1-Settings, 2-Images
    Animator menuAnim, settingAnim, imagesAnim;
    public static MenuController Instance
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
        menuAnim = animators[0].GetComponent<Animator>();
        settingAnim = animators[1].GetComponent<Animator>();
        imagesAnim = animators[2].GetComponent<Animator>();
    }

    private bool canPause = false;

    void Update()
    {
        if (Input.GetButtonDown("Pause") && canPause)
        {
            if (!isPaused)
            {
                //GetComponent<FastSoundCtrl>().Play("EscapeIn");
                menus[3].SetActive(true);
                isPaused = true;
                Time.timeScale = 0;
            }
            else
            {
                //GetComponent<FastSoundCtrl>().Play("EscapeOut");
                menus[3].SetActive(false);
                isPaused = false;
                Time.timeScale = 1;
            }
            /* menus[3].SetActive(!menus[3].activeInHierarchy);
             if (menus[3].activeInHierarchy)
             {
                 Time.timeScale = 0;
             }
             else
             {
                 Time.timeScale = 1;
             }
             Debug.Log("Pausa" + Time.deltaTime.ToString());*/
        }
    }

    public void StartGame()
    {
        imagesAnim.SetBool("Start", true);
        menuAnim.SetBool("State", true);
        Invoke("LoadLvl0", 1.5f);
    }
    public void EndGame()
    {
        Application.Quit();
    }
    public void Settings()
    {
        if (isPaused) { menus[1].SetActive(true); canPause = false; settingAnim.SetBool("State", true); pausedPanel.SetActive(true); }
        else
        {
            menus[1].SetActive(true);
            menuAnim.SetBool("State", true); imagesAnim.SetBool("Settings", true); settingAnim.SetBool("State", true);
        }
    }
    public void Return()
    {
        if (isPaused) { settingAnim.SetBool("State", false); MenuState(3); canPause = true; pausedPanel.SetActive(false); }
        else
        {
            menuAnim.SetBool("State", false); imagesAnim.SetBool("Settings", false); settingAnim.SetBool("State", false);
        }
    }
    public void NextLevel(string lvlToLoad)
    {
        StartCoroutine(LoadLvl(lvlToLoad));
    }

    IEnumerator LoadLvl(string sceneName)
    {
        MenuState(2);
        animators[2].SetActive(false);
        Time.timeScale = 0;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name != mainScene)
            {
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                while (!asyncUnload.isDone)
                {
                    yield return null;
                }
            }
        }

        if (sceneName != null) //Si queremos cargar un nivel entra en esta parte del if
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            ContinueLoading();
        }
        else //Si queremos volver al menú principal el if entra por aquí. Viene de RestartGame()
        {
            player.SetActive(false);
            MenuState(0);
            menuAnim.SetBool("State", true);
        }
    }

    private void ContinueLoading()
    {
        //canPause = true;
        //menus[2].SetActive(false);
        //player.SetActive(true);
        //playerSpawns = GameObject.FindObjectOfType<CheckPoints>();
        //player.transform.position = playerSpawns.checkpoints[checkpointNo].transform.position;
        //playerSprite.transform.position = player.transform.position;
        //health = GameObject.FindObjectOfType<PlayerHealth>();
        //health.HealPlayer();
        //Time.timeScale = 1;      
    }

    public void MenuState(int x) //x-> menu that we want
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (i == x) { menus[i].SetActive(true); continue; }
            menus[i].SetActive(false);
        }
    }

    //================= Levels =================
    private void LoadLvl0()
    {
        StartCoroutine(LoadLvl("lvl0"));
    }
    public void LoadLvl1()
    {
        StartCoroutine(LoadLvl("lvl1"));
    }
    public void LoadLvl2()
    {
        StartCoroutine(LoadLvl("lvl2"));
    }
    public void LoadLvl3()
    {
        StartCoroutine(LoadLvl("lvl3"));
    }
    public void LoadLvl4()
    {
        StartCoroutine(LoadLvl("lvl4"));
    }
    public void ResetCheckpoints()
    {
        checkpointNo = 0;
    }
    public void RestartGame()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name != mainScene)
            {
                //FindObjectOfType<ItemManagement>().StolenItemIsDestroyed();
                StartCoroutine(LoadLvl(SceneManager.GetSceneAt(i).name));
            }
        }
    }

    public void UpdateCheckpoint(int x)
    {
        checkpointNo = x;
    }
}
