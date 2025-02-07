using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameManager).ToString());
                    _instance = singleton.AddComponent<GameManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    [Header("References in Menu Scene")]
    public GameObject startButton;
    public GameObject exitButton;

    [Header("References in Select Menu Scene")]
    public GameObject RLGLButton;
    public GameObject GLBridgeButton;
    public GameObject exitButtonSelect;


    [Header("References in Game Scene")]
    public uiControl uiController;
    public DollController dollController;
    public GameObject fallDetector;
    public backsoundingame backSound;


    [Header("Scene Settinge")]
    public bool isRLGL = false;
    public bool isGLBridge = false; 

    [Header("Gameplay")]
    public bool canPlayerMove = false;
  


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
     

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if(backSound != null){
            backSound.OnAudioFinish -= EnablePlayerMovement;
        }
        if(dollController != null){
            backSound.OnAudioFinish -= EnableDollMove;
        }
    }

    private void Update() {
        if((SceneManager.GetActiveScene().name == "SceneRLGL" || SceneManager.GetActiveScene().name == "SceneBridge" ) && Input.GetKeyDown(KeyCode.Escape)){
            PauseGame();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            InitializeMenuReferences();
        }
        else if (scene.name == "SceneRLGL")
        {
            InitializeGameRLGLReferences();
        }
        else if (scene.name == "SelectMenu")
        {
            InitializeChoseMenuReferences();
        }else if(scene.name == "SceneBridge"){
            InitializeGLBridgeReferences();
        }
    }

    private void InitializeMenuReferences()
    {
        
        startButton = GameObject.Find("Play");
        exitButton = GameObject.Find("Exit");

        if (startButton != null)
        {
            startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        }

        if (exitButton != null)
        {
            exitButton.GetComponent<Button>().onClick.AddListener(ExitGame);
        }
    }

    private void InitializeGameRLGLReferences()
    {
        isRLGL = true;
        uiController = GameObject.Find("Canvas").GetComponent<uiControl>();
        dollController = FindObjectOfType<DollController>();
        backSound = FindObjectOfType<backsoundingame>();

        if (uiController.menuPanel != null)
            uiController.menuPanel.SetActive(false);

        if (uiController.restartButton != null)
            uiController.restartButton.onClick.AddListener(RestartGame);

        if (uiController.exitButton != null)
            uiController.exitButton.onClick.AddListener(ExitGame);

        if(backSound != null) {
            backSound.PlayStart();
            backSound.OnAudioFinish += EnablePlayerMovement;
        }

        if(dollController != null){
            backSound.OnAudioFinish += EnableDollMove;
        }

    }

    private void InitializeChoseMenuReferences()
    {
        isRLGL = false;
        isGLBridge = false;
        
        RLGLButton = GameObject.Find("RLGL");
        GLBridgeButton = GameObject.Find("GLBRIDGE");
        exitButtonSelect = GameObject.Find("Exit");

        HandleSelectMenu();
    }

    private void InitializeGLBridgeReferences()
    {
        isGLBridge = true;
        uiController = GameObject.Find("Canvas").GetComponent<uiControl>();
        backSound = FindObjectOfType<backsoundingame>();
        fallDetector = GameObject.Find("FallDetector");

        if (uiController.menuPanel != null)
            uiController.menuPanel.SetActive(false);

        if (uiController.restartButton != null)
            uiController.restartButton.onClick.AddListener(RestartGame);

        if (uiController.exitButton != null)
            uiController.exitButton.onClick.AddListener(ExitGame);
        
        // if(fallDetector != null){
        //     fallDetector.SetActive(false);
        // }

        if(backSound != null) {
            backSound.PlayStart();
            backSound.OnAudioFinish += EnablePlayerMovement;
        }
    }


    private void HandleSelectMenu(){
        if(RLGLButton != null){
            RLGLButton.GetComponent<Button>().onClick.AddListener(StartRLGL);
            print("RLGL");
        }

        if(GLBridgeButton != null){
            GLBridgeButton.GetComponent<Button>().onClick.AddListener(StartGLBridge);
            print("GLBridge");  
        }

        if(exitButtonSelect != null){
            exitButtonSelect.GetComponent<Button>().onClick.AddListener(ExitGame);
        }
    }

    private void StartRLGL(){
        SceneManager.LoadScene("SceneRLGL");
        Time.timeScale = 1;
    }

    private void StartGLBridge(){
        SceneManager.LoadScene("SceneBridge");
        Time.timeScale = 1;
    }

    public void EnableDollMove(){
  
        dollController.StartCoroutine(dollController.changeLightCorroutine());
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SelectMenu");
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        canPlayerMove = false;
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;


        if (uiController.menuPanel != null)
            uiController.menuPanel.SetActive(!uiController.menuPanel.activeSelf);
    }

    public void ExitGame()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Application.Quit();
        }
        else if(SceneManager.GetActiveScene().name == "SelectMenu")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("SelectMenu");
            canPlayerMove = false;
        }
    }

    public void GameOver()
    {
        if (dollController != null)
            dollController.DisableDoll();

        if (uiController.menuPanel != null)
            uiController.menuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canPlayerMove = false;
    }

    private void EnablePlayerMovement(){
        uiController.StartCoroutine(uiController.showGoText());
        canPlayerMove = true;
    }
}
