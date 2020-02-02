using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningOut : MonoBehaviour
{
    public GameObject miniBullet;
    public GameObject FirstEnemy;
    private bool explode;
    private float timeToExplosion;
    // Start is called before the first frame update
    void Start()
    {
        timeToExplosion = .25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (explode && timeToExplosion > 0)
        {
            timeToExplosion -= Time.deltaTime;
        }
        else if (explode && timeToExplosion <= 0)
        {
            Destroy(FirstEnemy);
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && FirstEnemy == null)
        {
            GetComponent<CapsuleCollider>().radius = 50;
            FirstEnemy = other.gameObject;
            explode = true;
            
        } else {
            if (other.gameObject.tag == "Enemy")
            {
                GameObject bullet = Instantiate(miniBullet, FirstEnemy.transform.position, Quaternion.identity);
                bullet.GetComponent<BulletAttack>().speed = 2f;
                bullet.GetComponent<BulletAttack>().target = other.gameObject;
                Debug.Log("SPAWNED BULLET!");
            }
        }
    }

}
