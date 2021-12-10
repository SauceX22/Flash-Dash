using UnityEngine;

public class RoadBlocker : MonoBehaviour
{
    private bool trapActive = true;
    private void OnCollisionEnter(Collision collision)
    {
        //Check if layer is = "Car Col" layer
        if (collision.gameObject.layer == 12)
        {
            if (trapActive)
            {
                //Debug.Log("Collided with a car!");
                FindObjectOfType<AudioManager>().Play("RoadBlocker");
                var car = collision.transform.parent.transform.GetComponent<Car>();
                var carEffects = car.transform.GetComponent<Effects>();

                carEffects.StartCoroutine("GiveSlowness");
                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                trapActive = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
