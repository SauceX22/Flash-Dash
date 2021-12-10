using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public SceneFader fader;

    public void SelectOrLoad(string levelName)
    {
        fader.FadeTo(levelName);
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
