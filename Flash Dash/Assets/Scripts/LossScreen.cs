using UnityEngine;
using UnityEngine.SceneManagement;

public class LossScreen : MonoBehaviour
{
    public LevelManager sceneLoader;

    public void Retry()
    {
        sceneLoader.SelectOrLoad(SceneManager.GetActiveScene().name);
    }

    public void Back()
    {
        sceneLoader.SelectOrLoad("Scene_ChooseLevel");
    }
}
