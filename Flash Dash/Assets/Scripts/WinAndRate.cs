using System;
using UnityEngine;

public class WinAndRate : MonoBehaviour
{
    private GameManager gameManager;
    private Effects effects;

    public enum Stars
    {
        OneStar = 1,
        TwoStars = 2,
        ThreeStars = 3,
        FourStars = 4,
        FiveStars = 5
    }
    [HideInInspector]
    public Stars overAllRating;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            var car = other.transform.parent.transform.GetComponent<Car>();
            effects = car.transform.GetComponent<Effects>();
            gameManager = car.transform.GetComponent<CarCollisionSensors>().gameManager;
            FindObjectOfType<AudioManager>().Play("Win");
            gameManager.gameHasEnded = true;
            Rate();
        }
    }

    void Rate()
    {
        int timeScore = RateTime();
        int healthScore = RateDamage();

        GiveFinalScore(timeScore, healthScore);
    }

    //Rate From 3 Stars
    int RateTime()
    {
        float time = gameManager.StopTimerAndGetIt();
        int spTime = gameManager.supposedTime;

        int finalScore = 0;

        if (time <= spTime)
        {
            // 3 stars
            finalScore = 3;
        }
        else if (time > spTime && time <= spTime * 1.5f)
        {
            //2 stars
            finalScore = 2;
        }
        else if (time > spTime * 1.5f && time <= spTime * 2f)
        {
            //1 star
            finalScore = 1;
        }
        else if (time > spTime * 2f)
        {
            //0 stars
            finalScore = 0;
        }
        return finalScore;
    }

    //Rate From 4 Stars
    int RateDamage()
    {
        int health = gameManager.healthSystem.GetHealth();
        int maxHealth = gameManager.healthSystem.GetMaxHealth();

        int finalScore = 0;

        if (health > 80)
        {
            //4 stars
            finalScore = 4;
        }
        else if (health < 80 && health >= 50)
        {
            //3 stars
            finalScore = 3;
        }
        else if (health < 50 && health > 10)
        {
            //2 stars
            finalScore = 2;
        }
        else if (health < 10)
        {
            //1 star
            finalScore = 1;
        }
        return finalScore;
    }


    private void GiveFinalScore(int timeScore, int healthScore)
    {
        Stars finalScore = (Stars)((timeScore + healthScore) - 1);

        if (finalScore >= (Stars)5) finalScore = (Stars)5;
        if (finalScore <= (Stars)1) finalScore = (Stars)1;

        Debug.Log(finalScore);
        overAllRating = finalScore;
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
