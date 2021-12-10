using System.Collections;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public int turboSpeed = 25;
    private bool doTurbo = false;

    public GameManager gameManager;
    public Rigidbody2D rb;
    public Car car;
    private bool slowDown = false;
    private float slownessSpeed = 20f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
            CheckForRewind();
    }

    private void FixedUpdate()
    {
        if (slowDown)
        {
            transform.Translate(Vector3.down * slownessSpeed * Time.deltaTime);
        }

        if (doTurbo)
        {
            transform.Translate(Vector3.up * turboSpeed * Time.deltaTime);
        }
    }

    public IEnumerator GiveSlowness()
    {
        slowDown = true;
        //Slowness Effects
        yield return new WaitForSeconds(2f);
        rb.velocity -= Time.deltaTime * Vector2.one;
        rb.velocity = Vector2.zero;
        slowDown = false;
    }

    public IEnumerator MessUpCarVariables(Car car)
    {
        car.TotalTireGripFront = .3f;
        car.TotalTireGripRear = .5f;
        car.SteerSpeed = 5f;
        car.SteerAdjustSpeed = 0f;
        car.SpeedSteerCorrection = 1000f;
        Debug.LogWarning("Car Affected!");
        yield return new WaitForSeconds(3f);
        ResetCarVariables(car);
    }

    private void ResetCarVariables(Car car)
    {
        Debug.LogWarning("Car reset!");
        car.TotalTireGripFront = 1.5f;
        car.TotalTireGripRear = 4.8f;
        car.SteerSpeed = 4.5f;
        car.SteerAdjustSpeed = 4f;
        car.SpeedSteerCorrection = 850f;
    }

    public IEnumerator DoTurbo(int duration)
    {
        doTurbo = true;
        //Turbo Effects
        gameManager.cameraFollow.smoothSpeed = 16;
        yield return new WaitForSeconds(duration);
        gameManager.cameraFollow.smoothSpeed = 11;
        doTurbo = false;
    }

    public void CheckForRewind()
    {
        if (gameManager.hasRewind)
        {
            StartCoroutine("DoRewindCO");            
        }
    }

    public IEnumerator DoRewindCO()
    {
        gameManager.DoRewindForAll();
        yield return new WaitForSeconds(5f);
        gameManager.StopRewindForAll();
        gameManager.hasRewind = false;
    }
}
