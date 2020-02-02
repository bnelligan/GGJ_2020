using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ByeBye : MonoBehaviour
{
    public List<GameObject> EnemiesWithinRange = new List<GameObject>();
    public int health;

    public void Update()
    {
        if (health <= 0)
        {
            foreach (var item in EnemiesWithinRange)
            {
                EnemiesWithinRange.Remove(item);
                Destroy(item);
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
