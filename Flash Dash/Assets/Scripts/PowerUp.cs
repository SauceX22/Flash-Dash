using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUps
    {
        Turbo,
        Rewind
    }
    public PowerUps powerUpType;

    public GameObject pickUpVisualEffect;

    public int duration = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            var car = other.transform.parent.transform.GetComponent<Car>();
            var carEffects = car.transform.GetComponent<Effects>();
            FindObjectOfType<AudioManager>().Play("PowerUpPickUp");

            StartCoroutine("PickUp", carEffects);
        }
    }

    IEnumerator PickUp(Effects effects)
    {
        GameObject effect = Instantiate(pickUpVisualEffect, transform.position, transform.rotation);

        transform.GetComponent<SpriteRenderer>().enabled = false;

        ApplyPowerUp(effects);

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        Destroy(effect);
    }

    void ApplyPowerUp(Effects effects)
    {
        if (powerUpType == PowerUps.Rewind)
        {
            effects.gameManager.hasRewind = true;
        }
        else if (powerUpType == PowerUps.Turbo)
        {
            effects.StartCoroutine("DoTurbo", duration);
        }
    }
}
