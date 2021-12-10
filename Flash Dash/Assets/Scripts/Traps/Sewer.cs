using UnityEngine;

public class Sewer : MonoBehaviour
{
    public BoxCollider boxCollider;

    private bool trapActive = true;

    private void OnTriggerEnter(Collider other)
    {
        //Check if layer is = "Car Col" layer
        if (other.gameObject.layer == 12)
        {
            if (trapActive)
            {
                //Debug.Log("Collided with a car!");
                var car = other.transform.parent.transform.GetComponent<Car>();
                var carEffects = car.transform.GetComponent<Effects>();

                carEffects.StartCoroutine(carEffects.MessUpCarVariables(car));
                //trapActive = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
    }
}
