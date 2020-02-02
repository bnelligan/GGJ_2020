using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : MonoBehaviour
{
    public List<GameObject> EnemiesWithinRange = new List<GameObject>();
    private float timeToExplosion;
    public bool TimeToExplode;
    // Start is called before the first frame update
    void Start()
    {
        TimeToExplode = false;
        timeToExplosion = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeToExplode)
        {
            if (timeToExplosion > 0)
            {
                timeToExplosion -= Time.deltaTime;
            }
            else if (timeToExplosion <= 0)
            {
                foreach (var item in EnemiesWithinRange)
                {
                    Destroy(item);
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            EnemiesWithinRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemiesWithinRange.Remove(other.gameObject);
        }
    }
}
