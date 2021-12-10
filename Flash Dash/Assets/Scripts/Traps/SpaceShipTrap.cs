using System.Collections;
using TMPro;
using UnityEngine;

public class SpaceShipTrap : MonoBehaviour
{
    public Transform player;
    public Vector2 direction;
    public float speed = 10f;
    //public float supposedDis = 30f;

    private Vector2 random;
    private bool headToPlayer = false;

    void OnEnable()
    {
        StartCoroutine(ChooseRandomDir());
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.Self);
    }

    IEnumerator ChooseRandomDir()
    {
        random = CalculateRnadom();
        yield return new WaitForSeconds(1f);
        StartCoroutine(ChooseRandomDir());
    }

    private void Update()
    {
        if (random == Vector2.zero)
            random = CalculateRnadom();
        else
            direction = random;
    }

    Vector2 CalculateRnadom()
    {
        Vector2 t = Vector2.zero;

        if (headToPlayer)
        {
            Vector2 g = player.position;
            Vector2 r = Camera.main.WorldToScreenPoint(g);
            t = (r - (Vector2)transform.position).normalized;
            speed *= 1.5f;
            headToPlayer = false;
        }
        else
        {
            t = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            Camera.main.WorldToScreenPoint(t);
            speed /= 1.5f;
            headToPlayer = true;
        }
        return t;
    }
}
