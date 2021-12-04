using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExplosion : MonoBehaviour
{
    public GameObject explose;
    public void spawnExplosion()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject exp = Instantiate(explose, player.transform);
        Destroy(exp, 2f);
    }
}
