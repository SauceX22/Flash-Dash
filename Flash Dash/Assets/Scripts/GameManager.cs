using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int maxHealth = 100;
    public int supposedTime;

    public bool isDark = false;
    public bool hasRewind = false;
    [HideInInspector]
    public bool gameHasEnded = false;
    [HideInInspector]
    public bool hasLost = false;

    private bool raiseWeight = false;

    //References 
    [HideInInspector]
    public HealthSystem healthSystem;
    public HealthBar healthBar;
    public Camera2DFollow cameraFollow;
    public Transform directionalLight;
    public StopWatch stopWatch;
    public LevelManager sceneLoader;
    public Car playerCar;

    public Volume FXVolume;
    public VolumeProfile lossProfile;
    public VolumeProfile winAndOutProfile;
    public VolumeProfile normalProfile;

    //TimeBodies 
    [SerializeField]
    public List<TimeBodyRigidBody3D> crossingTrapsTimeBodies = new List<TimeBodyRigidBody3D>();
    public CarTimeBody carTimeBody;
    public TimeBodyNoRigidBody cameraTimeBody;

    void Awake()
    {
        healthSystem = new HealthSystem(maxHealth);
        healthBar.Setup(this, healthSystem, maxHealth);
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().Stop("Theme");
        hasRewind = true;
    }

    private void FixedUpdate()
    {
        if (isDark)
            directionalLight.gameObject.SetActive(false);
        else
            directionalLight.gameObject.SetActive(true);
    }

    private bool t = true;
    private bool r = true;
    private void Update()
    {
        if (gameHasEnded)
        {
            if (hasLost)
            {
                if (t)
                    LoseGame();
            }
            else if (!hasLost)
            {
                if (r)
                    WinGame();
            }
        }
        if (raiseWeight)
        {
            FXVolume.weight += Time.deltaTime / 4f;
        }

        
    }

    public void SetColorsBackToNormal(SpriteRenderer carSprite, ColorsPreset normalColorsPreset)
    {
        Camera.main.backgroundColor = normalColorsPreset.backgroundColor;
        carSprite.color = normalColorsPreset.carColor;
        isDark = normalColorsPreset.isDark;
    }

    void LoseGame()
    {
        t = false;
        StartCoroutine(LoseGameCO());
    }
    void WinGame()
    {
        r = false;
        StartCoroutine(WinAndEndGame());
    }

    IEnumerator LoseGameCO()
    {
        //loose game effects
        Debug.Log("LoseGameCO");
        stopWatch.gameOnPanel.gameObject.SetActive(false);
        stopWatch.panelChild_HasRewindPopUp.gameObject.SetActive(false);
        FXVolume.weight = 0.25f;
        raiseWeight = true;
        FXVolume.profile = lossProfile;
        yield return new WaitForSeconds(4f);
        stopWatch.panel_LossScreen.gameObject.SetActive(true);
        raiseWeight = false;
    }

    IEnumerator WinAndEndGame()
    {
        Debug.Log("WinGameCO");
        stopWatch.gameOnPanel.gameObject.SetActive(false);
        stopWatch.panelChild_HasRewindPopUp.gameObject.SetActive(false);
        FXVolume.weight = 0.15f;
        raiseWeight = true;
        FXVolume.profile = winAndOutProfile;
        yield return new WaitForSeconds(3f);
        stopWatch.panel_WinScreen.gameObject.SetActive(true);
        GetRightRatingImage().gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        raiseWeight = false;
        sceneLoader.SelectOrLoad("Scene_ChooseLevel");
        FindObjectOfType<AudioManager>().Stop("Damaging");
        FindObjectOfType<AudioManager>().Stop("Ambience");
        yield return 0;
    }

    Transform GetRightRatingImage()
    {
        WinAndRate winAndRate = FindObjectOfType<WinAndRate>();
        int rating = (int)winAndRate.overAllRating;

        return stopWatch.panel_WinScreen.GetChild(rating).transform;
    }

    public float StopTimerAndGetIt()
    {
        float time = stopWatch.GetTimerTime();
        return time;
    }

    public void DoRewindForAll()
    {
        carTimeBody.StartRewind();
        cameraTimeBody.StartRewind();
        foreach (var crossingTrap in crossingTrapsTimeBodies)
        {
            crossingTrap.StartRewind();
        }
    }

    public void StopRewindForAll()
    {
        carTimeBody.StopRewind();
        cameraTimeBody.StopRewind();
        foreach (var crossingTrap in crossingTrapsTimeBodies)
        {
            crossingTrap.StopRewind();
        }
    }
}
