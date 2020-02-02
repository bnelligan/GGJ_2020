using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class SpawnGenerators : MonoBehaviour
{
    public List<Transform> generator1Points;
    public List<Transform> generator2Points;
    public List<Transform> generator3Points;

    public GameObject generatorPrefab;
    private GameObject generator1, generator2, generator3;
        
    // Start is called before the first frame update
    void Start()
    {
        SpawnEachGenerator(); 
    }

    public void SpawnEachGenerator()
    {
        int index1 = Random.Range(0, generator1Points.Count - 1);
        generator1 = Instantiate(generatorPrefab, generator1Points[index1].position, Quaternion.identity);
        generator1.GetComponent<AuthorGenerator>().GenID = 1;
        
        int index2 = Random.Range(0, generator2Points.Count - 1);
        generator2= Instantiate(generatorPrefab, generator2Points[index2].position, Quaternion.identity);
        generator1.GetComponent<AuthorGenerator>().GenID = 2;
        
        int index3 = Random.Range(0, generator3Points.Count - 1);
        generator3 = Instantiate(generatorPrefab, generator3Points[index3].position, Quaternion.identity);
        generator1.GetComponent<AuthorGenerator>().GenID = 3;
        
    }
}
