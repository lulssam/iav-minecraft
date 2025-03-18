using UnityEngine;
using System.Collections;

public class World0 : MonoBehaviour
{
    public GameObject block;
    public int size;

    IEnumerator BuildWorld()
    {
        for (int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject cubo = GameObject.Instantiate(block, pos, Quaternion.identity);
                    cubo.transform.parent = this.transform;
                    cubo.name = x + " " + y + " " + z;
                    if (Random.Range(0, 100) < 50)
                    {
                        cubo.GetComponent<MeshRenderer>().material.color = Color.red;
                    }

                }
            }
            yield return null;
        }
    }

    void Start()
    {
        StartCoroutine(BuildWorld());
    }

    void Update()
    {

    }
}
