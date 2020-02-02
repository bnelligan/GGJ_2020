using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4Controller : MonoBehaviour
{
    public GameObject c4;
    public GameObject lazer;
    public GameObject shotgunBullet;
    public int c4Limit;
    private List<GameObject> c4List = new List<GameObject>();
    public GameObject spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && c4List.Count < c4Limit)
        {
            
            GameObject OBJc4 = Instantiate(c4, spawnPoint.transform.position, Quaternion.identity);
            c4List.Add(OBJc4);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {

            GameObject laz = Instantiate(lazer, spawnPoint.transform.position, Quaternion.identity);
            
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Vector3 pos = new Vector3(10, 0, 10);
            GameObject centerlaz = Instantiate(shotgunBullet, spawnPoint.transform.position, Quaternion.identity);
            centerlaz.GetComponent<ShotgunShell>().BulletTime = .5f;
            centerlaz.GetComponent<ShotgunShell>().speed = 3;
            centerlaz.GetComponent<ShotgunShell>().hit = pos;

            for (int i = 1; i <= 5; i++)
            {
                GameObject laz = Instantiate(shotgunBullet, spawnPoint.transform.position, Quaternion.identity);
                laz.GetComponent<ShotgunShell>().BulletTime = .5f;
                laz.GetComponent<ShotgunShell>().speed = 3;
                laz.GetComponent<ShotgunShell>().hit = new Vector3(pos.x + (2 * i), 0, pos.z - (2 * i));

                GameObject altlaz = Instantiate(shotgunBullet, spawnPoint.transform.position, Quaternion.identity);
                altlaz.GetComponent<ShotgunShell>().BulletTime = .5f;
                altlaz.GetComponent<ShotgunShell>().speed = 3;
                altlaz.GetComponent<ShotgunShell>().hit = new Vector3(pos.x - (2 * i), 0, pos.z + (2 * i));
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (var item in c4List)
            {
                item.GetComponent<C4>().TimeToExplode = true;
            }
            for (int i = c4List.Count-1; i >= 0; i--)
            {
                c4List.RemoveAt(i);
            }
        }

    }
}
