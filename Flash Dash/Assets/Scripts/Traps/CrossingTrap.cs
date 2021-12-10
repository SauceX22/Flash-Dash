using UnityEngine;

public class CrossingTrap : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    private TrapTrigger trapTrigger;
    public Vector2 direction;
    private bool fellIn = false;
    private float speed;

    void Start()
    {
        transform.position = startPoint.position;

        Vector2 heading = (endPoint.position - transform.position);
        direction = heading.normalized;
    }

    private void Update()
    {
        if (fellIn)
        {
            StartHeading();
        }
    }

    private void FixedUpdate()
    {

        float distance = Vector3.Distance(transform.position, endPoint.position);
        if (distance <= 0.1f)
        {
            Destroy(transform.gameObject);
            Destroy(trapTrigger.transform.gameObject);
        }
    }

    private void StartHeading()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void ActivateTrap(float speed, TrapTrigger trapTrigger)
    {
        FindObjectOfType<AudioManager>().Play("Crossing");
        this.speed = speed;
        fellIn = true;
        this.trapTrigger = trapTrigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            var car = other.transform.parent.transform.GetComponent<CarCollisionSensors>();
            car.gameManager.healthSystem.Damage(5);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
}
