using System.Collections;
using TMPro;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
    public float timeStart;
    public bool timerActive = false;
    //References 
    public Transform gamePausedPanel;
    public Transform gameOnPanel;
    public Transform gameOnColorPanel;
    public Transform panelChild_HasRewindPopUp;
    public Transform panel_LossScreen;
    public Transform panel_WinScreen;
    public TextMeshProUGUI Text_CountDownTimer;
    public TextMeshProUGUI Text_ThreeTwoOneGoCountDownTimer;
    public GameManager gameManager;

    private Car playerCar;


    private void Awake()
    {
        playerCar = transform.GetComponent<GameManager>().playerCar;
    }
    void Start()
    {
        Resume();

        Text_CountDownTimer.text = timeStart.ToString("F2");
    }
    void Update()
    {
        if (timerActive)
        {
            timeStart += Time.unscaledDeltaTime;
            Text_CountDownTimer.text = timeStart.ToString("F2");
        }

        //Rewind PowerUp Screen
        if (gameManager.hasRewind)
        {
            panelChild_HasRewindPopUp.gameObject.SetActive(true);
        }
        else if (!gameManager.hasRewind)
        {
            panelChild_HasRewindPopUp.gameObject.SetActive(false);
        }
    }
    public void Pause()
    {
        StartCoroutine(PauseCR());
    }
    public void Resume()
    {
        StartCoroutine(ResumeCR());
    }

    IEnumerator PauseCR()
    {
        //Pause Game
        timerActive = false;
        playerCar.enabled = false;
        gamePausedPanel.gameObject.SetActive(true);
        gameOnPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(0f);
    }

    IEnumerator ResumeCR()
    {
        //Resume Game
        gamePausedPanel.gameObject.SetActive(false);

        //Count 3 2 1 Go!
        StartCoroutine(CountTTO());

        gameOnPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0f);
    }

    public IEnumerator CountTTO()
    {
        gameOnColorPanel.gameObject.SetActive(true);
        playerCar.enabled = false;
        Text_ThreeTwoOneGoCountDownTimer.text = "3";
        yield return new WaitForSeconds(1f);
        Text_ThreeTwoOneGoCountDownTimer.text = "2";
        yield return new WaitForSeconds(1f);
        Text_ThreeTwoOneGoCountDownTimer.text = "1";
        yield return new WaitForSeconds(1f);
        Text_ThreeTwoOneGoCountDownTimer.text = "Go!";
        timerActive = true;
        gameOnColorPanel.gameObject.SetActive(false);
        playerCar.enabled = true;
    }

    public float GetTimerTime()
    {
        timerActive = false;
        float time = timeStart;
        return time;
    }
}
