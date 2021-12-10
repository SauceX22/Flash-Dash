using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject Panel_HowToPlay;

    public void Play()
    {
        SceneManager.LoadScene("Scene_ChooseLevel");
    }

    public void PlayBackButton()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void HowToPlay()
    {
        Panel_HowToPlay.SetActive(true);
    }

    public void Button_HTPNextLastButton(RectTransform transform)
    {
        Panel_HowToPlay.SetActive(false);
        transform.parent.gameObject.SetActive(false);
        transform.gameObject.SetActive(false);
    }

    public void Button_HTPNext(RectTransform transform)
    {
        transform.parent.gameObject.SetActive(false);
        transform.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnPointer()
    {
        FindObjectOfType<AudioManager>().Play("ButtonHover");
    }

    public void OnClick()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }
}
