using System.CodeDom;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class SetUpScene : MonoBehaviour
{
    //Mains
    [MenuItem("Set-Up Scene/Main Stuff", false, 10)]
    static void AddMains(MenuCommand menuCommand)
    {
        //Game Manager 
        GameObject gameManagerObj = new GameObject("GameManager", typeof(GameManager));
        GameObjectUtility.SetParentAndAlign(gameManagerObj, menuCommand.context as GameObject);
        GameManager gameManager = gameManagerObj.GetComponent<GameManager>();
        gameManagerObj.tag = "GameManager";



        //Dierctional Light 
        GameObject directionalLightObj = new GameObject("Directional Light");
        Undo.RegisterCreatedObjectUndo(directionalLightObj, "Create " + gameManagerObj.name);
        GameObjectUtility.SetParentAndAlign(gameManagerObj, menuCommand.context as GameObject);
        directionalLightObj.AddComponent<Light>().type = UnityEngine.LightType.Directional;
        gameManager.directionalLight = directionalLightObj.transform;
        StopWatch stopWatch = gameManager.gameObject.AddComponent<StopWatch>();

        //UI With Health Bar 
        GameObject pfUI = Resources.Load("Main_Prefabs/Canvas_RaceManager", typeof(GameObject)) as GameObject;
        GameObject allUI = Instantiate(pfUI, Vector3.zero, Quaternion.identity);
        Undo.RegisterCreatedObjectUndo(allUI, "Create " + allUI.name);
        allUI.transform.name = "Canvas_RaceManager";
        HealthBar healthBar = allUI.transform.Find("HealthBar").GetComponent<HealthBar>();
        gameManager.healthBar = healthBar;

        //EventSystem 
        GameObject eventSystemObj = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(StandaloneInputModule));

        //Stopwatch 
        stopWatch.gamePausedPanel = allUI.transform.Find("Panel_GamePaused").transform;
        stopWatch.gameOnPanel = allUI.transform.Find("Panel_GameOn").transform;
        stopWatch.gameOnColorPanel = allUI.transform.Find("Panel_Color").transform;
        stopWatch.panelChild_HasRewindPopUp = allUI.transform.Find("Panel_PowerUps").transform.Find("Rewind").transform;
        stopWatch.Text_CountDownTimer = allUI.transform.Find("Panel_GameOn").transform.Find("Text_CountDownTimer").GetComponent<TextMeshProUGUI>();
        stopWatch.Text_ThreeTwoOneGoCountDownTimer = allUI.transform.Find("Panel_Color").transform.Find("Text_ThreeTwoOneGoCountDownTimer").GetComponent<TextMeshProUGUI>();
        stopWatch.panel_LossScreen = allUI.transform.Find("Panel_LossScreen").transform;
        stopWatch.panel_WinScreen = allUI.transform.Find("Panel_WinScreen").transform;
        stopWatch.gameManager = gameManager;
        gameManager.stopWatch = stopWatch;

        //Player Car 
        GameObject pfCar = Resources.Load("Main_Prefabs/Car", typeof(GameObject)) as GameObject;
        GameObject playerCar = Instantiate(pfCar, Vector3.zero, Quaternion.identity);
        Undo.RegisterCreatedObjectUndo(playerCar, "Create " + gameManagerObj.name);
        playerCar.transform.name = "Car";
        playerCar.GetComponent<CarCollisionSensors>().gameManager = gameManager;
        playerCar.GetComponent<CarLights>().gameManager = gameManager;
        gameManager.playerCar = playerCar.GetComponent<Car>();
        Effects effects = playerCar.GetComponent<Effects>();
        effects.gameManager = gameManager;
        gameManager.carTimeBody = playerCar.GetComponent<CarTimeBody>();

        //Camera Follow 
        Camera2DFollow cameraFollow = Camera.main.gameObject.AddComponent<Camera2DFollow>();
        TimeBodyNoRigidBody cameraTimeBoy = Camera.main.gameObject.AddComponent<TimeBodyNoRigidBody>();
        Camera.main.orthographic = false;
        Camera.main.fieldOfView = 110f;
        cameraFollow.target = playerCar.transform;
        Debug.LogError("Enable Post-Processing on the camera", Camera.main);
        cameraFollow.target = playerCar.transform;
        cameraFollow.smoothSpeed = 11f;
        cameraFollow.offset.z = -8;
        gameManager.cameraFollow = cameraFollow;
        gameManager.cameraTimeBody = cameraTimeBoy;
        Camera.main.backgroundColor = new Color(0x7E, 0x67, 0xC5);

        //SceneManging stuff 
        GameObject pfSceneFader = Resources.Load("Main_Prefabs/SceneFader", typeof(GameObject)) as GameObject;
        GameObject sceneFaderObj = Instantiate(pfSceneFader, Vector3.zero, Quaternion.identity);
        sceneFaderObj.name = "Scene Fader";
        SceneFader sceneFader = sceneFaderObj.GetComponent<SceneFader>();
        Undo.RegisterCreatedObjectUndo(sceneFaderObj, "Create " + sceneFaderObj.name);

        //More SceneManaging Stuff 
        GameObject sceneLoaderObj = new GameObject("Scene Loader (Level Manager)", typeof(LevelManager));
        Undo.RegisterCreatedObjectUndo(sceneLoaderObj, "Create " + sceneLoaderObj.name);
        LevelManager sceneLoader = sceneLoaderObj.GetComponent<LevelManager>();
        sceneLoader.fader = sceneFaderObj.GetComponent<SceneFader>();
        gameManager.sceneLoader = sceneLoader;
        allUI.GetComponent<LossScreen>().sceneLoader = sceneLoader;

        //Getting References to VFX Profiles 
        VolumeProfile volumeProfile_Normal = Resources.Load("Profiles/VolumeProfile_Normal", typeof(VolumeProfile)) as VolumeProfile;
        VolumeProfile volumeProfile_Loss = Resources.Load("Profiles/VolumeProfile_Loss", typeof(VolumeProfile)) as VolumeProfile;
        VolumeProfile volumeProfile_Win = Resources.Load("Profiles/VolumeProfile_Win", typeof(VolumeProfile)) as VolumeProfile;
        gameManager.normalProfile = volumeProfile_Normal;
        gameManager.lossProfile = volumeProfile_Loss;
        gameManager.winAndOutProfile = volumeProfile_Win;

        //Global Volume 
        GameObject globalVolumeObj = new GameObject("Global Volume", typeof(Volume));
        Volume globalVolume = globalVolumeObj.GetComponent<Volume>();
        globalVolume.profile = volumeProfile_Normal;

        gameManager.FXVolume = globalVolume;

        // Register the creation in the undo system 
        Undo.RegisterCreatedObjectUndo(gameManagerObj, "Create " + gameManagerObj.name);
        Selection.activeObject = gameManagerObj;
    }

    //FX Volume
    [MenuItem("Set-Up Scene/Create Local FXVolume", false, 10)]
    static void CreateFXVolume(MenuCommand menuCommand)
    {
        GameObject localVolumeObj = new GameObject("FX Volume", typeof(Volume), typeof(ApplyColorPreset));
        Volume localVolume = localVolumeObj.GetComponent<Volume>();
        ApplyColorPreset colorPresetApplier = localVolumeObj.GetComponent<ApplyColorPreset>();
        VolumeProfile volumeProfile_Normal = Resources.Load("Profiles/VolumeProfile_Normal", typeof(VolumeProfile)) as VolumeProfile;
        ColorsPreset colorPreset_Normal = Resources.Load("ColorPresets/ColorPreset_Normal", typeof(ColorsPreset)) as ColorsPreset;
        localVolume.isGlobal = false;
        localVolume.blendDistance = 10f;
        localVolumeObj.transform.localScale = new Vector3(100f, 100f, 3);
        localVolume.profile = volumeProfile_Normal;
        colorPresetApplier.normalColorsPreset = colorPreset_Normal;
        colorPresetApplier.gameManager = FindObjectOfType<GameManager>();
        Debug.LogError("Assign Car graphics to apply preset script", localVolumeObj);
        Debug.LogError("Trigger This Damn Collider", localVolumeObj);
        Debug.LogError("Change The HDR Intensity to -5", localVolumeObj);
    }

    //Road
    [MenuItem("Set-Up Scene/Create Road", false, 10)]
    static void CreateRoad(MenuCommand menuCommand)
    {
        Material roadMatt = Resources.Load("Materials/PathMaterials/Modern_Track", typeof(Material)) as Material;
        GameObject road = new GameObject("Road", typeof(RoadCreator), typeof(Rigidbody));
        GameObjectUtility.SetParentAndAlign(road, menuCommand.context as GameObject);
        road.GetComponent<MeshRenderer>().material = roadMatt;
        road.GetComponent<Rigidbody>().useGravity = false;
        road.GetComponent<Rigidbody>().isKinematic = true;
        road.layer = 9;
        road.transform.position = new Vector3(0f, 0f, 1f);
        Debug.LogError("Add The MeshCollider!", road.transform);

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(road, "Create " + road.name);
        Selection.activeObject = road;
    }

    //Trap Trigger
    [MenuItem("Set-Up Scene/Trap/Trigger", false, 10)]
    static void CreateTrapTrigger(MenuCommand menuCommand)
    {
        GameObject trigger = new GameObject("TrapTrigger", typeof(TrapTrigger), typeof(Rigidbody));
        GameObjectUtility.SetParentAndAlign(trigger, menuCommand.context as GameObject);
        trigger.GetComponent<Rigidbody>().useGravity = false;
        trigger.layer = 11;
        trigger.transform.localScale = new Vector3(20f, 1f, 3f);
        BoxCollider boxCollider = trigger.GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        Vector3 camPos = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;
        Vector3 objPos = new Vector3(camPos.x, camPos.y, 0f);

        trigger.transform.position = objPos;

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(trigger, "Create " + trigger.name);
        Selection.activeObject = trigger;
    }

    //Crossing Trap
    [MenuItem("Set-Up Scene/Trap/Crossing Trap", false, 10)]
    static void CreateCrossingTrap(MenuCommand menuCommand)
    {
        GameObject crossingTrapObj = new GameObject("Crossing Trap");
        GameObjectUtility.SetParentAndAlign(crossingTrapObj, menuCommand.context as GameObject);

        //Debug.LogError("Reference this trap in GameManager list", crossingTrapObj.transform);
        //TimeBodyRigidBody3D crossingTrapTimeBody = crossingTrapObj.AddComponent<TimeBodyRigidBody3D>();
        //FindObjectOfType<GameManager>().crossingTrapsTimeBodies.Add(crossingTrapTimeBody);

        Vector3 camPos = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;
        Vector3 objPos = new Vector3(camPos.x, camPos.y, 0.98f);
        crossingTrapObj.transform.position = objPos;

        GameObject graphics = new GameObject("Graphics", typeof(SpriteRenderer), typeof(TimeBodyRigidBody3D), typeof(BoxCollider));
        CrossingTrap crossingTrap = graphics.AddComponent<CrossingTrap>();
        Rigidbody rb = graphics.AddComponent<Rigidbody>();
        graphics.tag = "CrossingTrap";
        rb.useGravity = false;
        var boxCollider = graphics.GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        GameObjectUtility.SetParentAndAlign(graphics, crossingTrapObj);

        GameObject startPoint = new GameObject("Start Point");
        GameObjectUtility.SetParentAndAlign(startPoint, crossingTrapObj);

        GameObject endPoint = new GameObject("End Point");
        GameObjectUtility.SetParentAndAlign(endPoint, crossingTrapObj);

        crossingTrap.startPoint = startPoint.transform;
        crossingTrap.endPoint = endPoint.transform;

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(crossingTrapObj, "Create " + crossingTrapObj.name);
        Selection.activeObject = crossingTrapObj;
    }

    //Sewer Trap
    [MenuItem("Set-Up Scene/Sewer", false, 10)]
    static void CreateSewer(MenuCommand menuCommand)
    {
        GameObject pfSewer = Resources.Load("Main_Prefabs/Sewer", typeof(GameObject)) as GameObject;
        GameObjectUtility.SetParentAndAlign(pfSewer, menuCommand.context as GameObject);

        Vector3 camPos = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;
        Vector3 objPos = new Vector3(camPos.x, camPos.y, 0.98f);

        GameObject sewer = Instantiate(pfSewer, objPos, Quaternion.identity);
        sewer.name = "Sewer Trap";

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(sewer, "Create " + pfSewer.name);
        Selection.activeObject = sewer;
    }

    //Raod Blocker
    [MenuItem("Set-Up Scene/Road Blocker", false, 10)]
    static void AddRoadBlocker(MenuCommand menuCommand)
    {
        GameObject pfRoadBlocker = Resources.Load("Main_Prefabs/Road_Blocker", typeof(GameObject)) as GameObject;
        GameObjectUtility.SetParentAndAlign(pfRoadBlocker, menuCommand.context as GameObject);

        Vector3 camPos = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;
        Vector3 objPos = new Vector3(camPos.x, camPos.y, 0.98f);

        GameObject roadBlocker = Instantiate(pfRoadBlocker, objPos, Quaternion.identity);
        roadBlocker.transform.name = "Road Blocker";


        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(roadBlocker, "Create " + pfRoadBlocker.name);
        Selection.activeObject = roadBlocker;
    }

    //PowerUP
    [MenuItem("Set-Up Scene/PowerUp", false, 10)]
    static void AddPowerUp(MenuCommand menuCommand)
    {
        GameObject powerUp = new GameObject("PowerUp", typeof(PowerUp), typeof(Rigidbody), typeof(SpriteRenderer));
        GameObjectUtility.SetParentAndAlign(powerUp, menuCommand.context as GameObject);
        powerUp.GetComponent<Rigidbody>().useGravity = false;
        powerUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        powerUp.layer = 14;
        powerUp.tag = "PowerUp";
        BoxCollider boxCollider = powerUp.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = Vector3.one * 2f;

        Vector3 camPos = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;
        Vector3 objPos = new Vector3(camPos.x, camPos.y, 0.98f);

        powerUp.transform.position = objPos;

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(powerUp, "Create " + powerUp.name);
        Selection.activeObject = powerUp;
    }

    //Reference All powerUps
    [MenuItem("Set-Up Scene/Reference PowerUp FX", false, 20)]
    static void ReferencePowerUpFX(MenuCommand menuCommand)
    {
        var powerUps = GameObject.FindGameObjectsWithTag("PowerUp");

        foreach (var powerUp in powerUps)
        {
            var powerUpCom = powerUp.GetComponent<PowerUp>();
            if (powerUpCom.powerUpType == PowerUp.PowerUps.Rewind)
            {
                powerUpCom.pickUpVisualEffect = Resources.Load("Main_Prefabs/RewindFX", typeof(GameObject)) as GameObject;
                var spriteRenderer = powerUp.transform.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load("Materials/Sprites/Rewind", typeof(Sprite)) as Sprite;
            }
            else if (powerUpCom.powerUpType == PowerUp.PowerUps.Turbo)
            {
                powerUpCom.pickUpVisualEffect = Resources.Load("Main_Prefabs/TurboFX", typeof(GameObject)) as GameObject;
                var spriteRenderer = powerUp.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load("Materials/Sprites/Turbo", typeof(Sprite)) as Sprite;
            }
        }
    }

    //Reference All Crossing Traps to gameManager
    [MenuItem("Set-Up Scene/Reference Crossing Traps", false, 20)]
    static void ReferenceCrossingTrapsToGameManager(MenuCommand menuCommand)
    {
        var crossingTraps = GameObject.FindGameObjectsWithTag("CrossingTrap");
        var gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.crossingTrapsTimeBodies.Clear();

        foreach (var crossingTrap in crossingTraps)
        {
            var timeBody = crossingTrap.GetComponent<TimeBodyRigidBody3D>();

            gameManager.crossingTrapsTimeBodies.Add(timeBody);
        }
        Debug.Log(gameManager.crossingTrapsTimeBodies.Count);
    }

    //Turn 180 deg
    [MenuItem("Set-Up Scene/reset blockers", false, 20)]
    static void resetBlockers(MenuCommand menuCommand)
    {
        var roadBlockers = GameObject.FindGameObjectsWithTag("RoadBlocker");

        foreach (var roadBlocker in roadBlockers)
        {
            Vector3 temp = roadBlocker.transform.rotation.eulerAngles;
            temp.z += 180f;
            roadBlocker.transform.rotation = Quaternion.Euler(temp);
        }
    }

    //Prefab of crossing trap
    [MenuItem("Set-Up Scene/CrossingTrapVTwo", false, 10)]
    static void AddCrossingTrapVTwo(MenuCommand menuCommand)
    {
        GameObject pfCrossingTrap = Resources.Load("Main_Prefabs/CrossingTrap", typeof(GameObject)) as GameObject;
        GameObjectUtility.SetParentAndAlign(pfCrossingTrap, menuCommand.context as GameObject);

        Vector3 camPos = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;
        Vector3 objPos = new Vector3(camPos.x, camPos.y, 0f);

        GameObject crossingTrapObj = Instantiate(pfCrossingTrap, objPos, Quaternion.identity);
        crossingTrapObj.transform.name = "Crossing Trap";

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(crossingTrapObj, "Create " + pfCrossingTrap.name);
        Selection.activeObject = crossingTrapObj;
    }

    //Wining Trigger
    [MenuItem("Set-Up Scene/Wining Trigger", false, 10)]
    static void AddWiningTrigger(MenuCommand menuCommand)
    {
        GameObject winTrigger = new GameObject("Wining Trigger", typeof(WinAndRate), typeof(Rigidbody));
        GameObjectUtility.SetParentAndAlign(winTrigger, menuCommand.context as GameObject);
        winTrigger.GetComponent<Rigidbody>().useGravity = false;
        winTrigger.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        winTrigger.layer = 13;
        winTrigger.transform.localScale = new Vector3(20f, 1f, 3f);
        BoxCollider boxCollider = winTrigger.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        Vector3 camPos = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;
        Vector3 objPos = new Vector3(camPos.x, camPos.y, 0.98f);

        winTrigger.transform.position = objPos;

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(winTrigger, "Create " + winTrigger.name);
        Selection.activeObject = winTrigger;
    }


    //Fix Ui Things
    [MenuItem("Fix Scene/Fix UI", false, 10)]
    static void FixUI(MenuCommand menuCommand)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        StopWatch stopWatch = gameManager.gameObject.GetComponent<StopWatch>();

        GameObject pfUI = Resources.Load("Main_Prefabs/Canvas_RaceManager", typeof(GameObject)) as GameObject;
        GameObject allUI = Instantiate(pfUI, Vector3.zero, Quaternion.identity);
        Undo.RegisterCreatedObjectUndo(allUI, "Create " + allUI.name);
        allUI.transform.name = "Canvas_RaceManager";
        HealthBar healthBar = allUI.transform.Find("HealthBar").GetComponent<HealthBar>();
        gameManager.healthBar = healthBar;


        stopWatch.gamePausedPanel = allUI.transform.Find("Panel_GamePaused").transform;
        stopWatch.gameOnPanel = allUI.transform.Find("Panel_GameOn").transform;
        stopWatch.gameOnColorPanel = allUI.transform.Find("Panel_Color").transform;
        stopWatch.panelChild_HasRewindPopUp = allUI.transform.Find("Panel_PowerUps").transform.Find("Rewind").transform;
        stopWatch.Text_CountDownTimer = allUI.transform.Find("Panel_GameOn").transform.Find("Text_CountDownTimer").GetComponent<TextMeshProUGUI>();
        stopWatch.Text_ThreeTwoOneGoCountDownTimer = allUI.transform.Find("Panel_Color").transform.Find("Text_ThreeTwoOneGoCountDownTimer").GetComponent<TextMeshProUGUI>();
        stopWatch.panel_LossScreen = allUI.transform.Find("Panel_LossScreen").transform;
        stopWatch.panel_WinScreen = allUI.transform.Find("Panel_WinScreen").transform;
        stopWatch.gameManager = gameManager;
        gameManager.stopWatch = stopWatch;
        allUI.GetComponent<LossScreen>().sceneLoader = FindObjectOfType<LevelManager>();

        Debug.LogError("Add Audio Manager!!!!!!!!!!!!");

        Selection.activeObject = allUI;
    }


    //Fix Crossing Things
    [MenuItem("Fix Scene/Fix Crossing", false, 10)]
    static void FixCrossing(MenuCommand menuCommand)
    {
        var triggers = GameObject.FindGameObjectsWithTag("G");
        var traps = GameObject.FindGameObjectsWithTag("H");

        for (int i = 0; i < traps.Length; i++)
        {
            DestroyImmediate(traps[i].gameObject);
        }
        for (int i = 0; i < triggers.Length; i++)
        {
            var crossingTrapObj = Resources.Load("Main_Prefabs/CrossingTrap", typeof(GameObject)) as GameObject;
            GameObject crossingTrap = Instantiate(crossingTrapObj, triggers[i].transform.position, triggers[i].transform.rotation);
            DestroyImmediate(triggers[i].gameObject);
        }
    }

    //Fix Crossing Things
    [MenuItem("Fix Scene/Fix Crossing AGAIN", false, 10)]
    static void FixCrossingAgain(MenuCommand menuCommand)
    {
        var traps = GameObject.FindGameObjectsWithTag("CrossingTrap");
        var asteroidSprite = Resources.Load("Materials/Sprites/Asteroid.jpg", typeof(Sprite)) as Sprite;

        for (int i = 0; i < traps.Length; i++)
        {
            traps[i].transform.localScale = new Vector3(.3f, .3f, 1f);
            traps[i].GetComponent<SpriteRenderer>().sprite = asteroidSprite;
        }
    }
}
