using UnityEngine;

public class CarLights : MonoBehaviour
{
    public GameManager gameManager;
    public SpriteRenderer lights;
    public Sprite ONLights;
    public Sprite OFFLights;
    public Transform lightObjL; 
    public Transform lightObjR; 

    void FixedUpdate()
    { 
        if (gameManager.isDark)
        {
            lightObjL.gameObject.SetActive(true);
            lightObjR.gameObject.SetActive(true);
            lights.sprite = ONLights;
        }
        else
        {
            lightObjL.gameObject.SetActive(false);
            lightObjR.gameObject.SetActive(false);
            lights.sprite = OFFLights;
        }
    }
}
