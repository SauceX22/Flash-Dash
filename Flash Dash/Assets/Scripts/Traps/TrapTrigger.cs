using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TrapTrigger : MonoBehaviour
{
    public enum TrapType
    {
        crossingTrap,
        spaceshipsTrap
    }
    public TrapType trapType;

    public CrossingTrap crossingTrap;
    public SpaceShipTrap spaceShipTrap;

    public float speed = 20;

    private void OnTriggerEnter(Collider other)
    {
        //car col
        if (other.gameObject.layer == 12)
        {
            if (trapType == TrapType.crossingTrap)
            {
                crossingTrap.ActivateTrap(speed, this);
            }
            else if (trapType == TrapType.spaceshipsTrap)
            {
                //call script
                //spaceShipTrap.DoTrap();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
