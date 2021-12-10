using System.Collections;
using UnityEngine;

public class ApplyColorPreset : MonoBehaviour
{
    public ColorsPreset normalColorsPreset;
    public ColorsPreset colorsPreset;
    public SpriteRenderer carSprite;
    public GameManager gameManager;

    public float blendSpeed = 10f;

    private Camera camera;
    private bool setColor = false;
    private bool setColorBack = false;

    private void OnTriggerEnter(Collider other)
    {
        //car col
        if (other.gameObject.layer == 12)
        {
            var car = other.transform.parent.GetComponent<CarCollisionSensors>();
            camera = car.gameManager.cameraFollow.gameObject.GetComponent<Camera>();
            FindObjectOfType<AudioManager>().Play("NewColorZone");

            StartCoroutine(BlendTo());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //car col
        if (other.gameObject.layer == 12)
        {
            var car = other.transform.parent.GetComponent<CarCollisionSensors>();
            camera = car.gameManager.cameraFollow.gameObject.GetComponent<Camera>();
            gameManager = car.gameManager;
            FindObjectOfType<AudioManager>().Play("NewColorZone");

            StartCoroutine(BlendBackTo());
        }
    }

    IEnumerator BlendTo()
    {
        setColorBack = false;
        setColor = true;
        yield return new WaitForSeconds(3f);
        setColor = false;
    }

    IEnumerator BlendBackTo()
    {
        setColor = false;
        setColorBack = true;
        yield return new WaitForSeconds(3f);
        setColorBack = false;
    }

    private void Start()
    {
        gameManager.SetColorsBackToNormal(carSprite, normalColorsPreset); ;
    }

    void Update()
    {
        if (setColor)
        {
            camera.backgroundColor = Color.Lerp(camera.backgroundColor, colorsPreset.backgroundColor, blendSpeed * Time.deltaTime);
            carSprite.color = Color.Lerp(carSprite.color, colorsPreset.carColor, blendSpeed * Time.deltaTime);
            gameManager.isDark = colorsPreset.isDark;
        }
        if (setColorBack)
        {
            gameManager.SetColorsBackToNormal(carSprite, normalColorsPreset);
        }
    }
}
