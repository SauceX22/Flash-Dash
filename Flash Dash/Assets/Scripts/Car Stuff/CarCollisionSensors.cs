using System.Linq;
using UnityEngine;

public class CarCollisionSensors : MonoBehaviour
{
    public Sensor[] sensors;

    public GameManager gameManager;

    [SerializeField]
    private Animator animator;
    private bool sensorLock = false;

    private void Start()
    {
        InvokeRepeating("DamageIfOut", 1f, 1f);
    }

    private void DamageIfOut()
    {
        sensorLock = true;
        //Debug.Log("Checking....");
        if (sensors.Any(sensor => !sensor.isTouching))
        {
            //Called when NOT on the track
            if (sensorLock)
            {
                //Debug.Log("Damaging!");
                FindObjectOfType<AudioManager>().Play("Damaging");
                DamageCarAnimPlay(true);
                gameManager.healthSystem.Damage(5);
                sensorLock = false;
            }
        }
        else
        {
            //Called when ON track
            DamageCarAnimPlay(false); 
            FindObjectOfType<AudioManager>().Stop("Damaging");
        }
    }

    public void DamageCarAnimPlay(bool playOrStop)
    {
        animator.SetBool("IsOut", playOrStop);
    }
}

