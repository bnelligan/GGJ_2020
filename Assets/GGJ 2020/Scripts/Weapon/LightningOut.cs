using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningOut : MonoBehaviour
{
    public GameObject miniBullet;
    public GameObject FirstEnemy;
    private bool explode;
    private float timeToExplosion;
    private float speed = 3f;
    private Vector3 targetLocation;
    // Start is called before the first frame update
    void Start()
    {
        timeToExplosion = .05f;
        targetLocation = new Vector3(1.909973f, 1.01001f, 2.976f);
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetLocation, step); 
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
                bullet.GetComponent<BulletAttack>().speed = 2.5f;
                bullet.GetComponent<BulletAttack>().target = other.gameObject;
            }
        }
    }

}
