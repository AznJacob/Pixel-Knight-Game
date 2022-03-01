using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    //public UnityEngine.Events.UnityEvent endBossFight;

    public GameObject boss;
    public GameObject endLevel;

    public Vector3 placement;

    bool hasBegun = false;

    void Update()
    {
        if(hasBegun == true && this.transform.childCount == 0)
        {
            SpawnEndLevel();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && hasBegun == false)
        {
            //make a starting position
            Vector3 spawnLocation = placement;
            //create new Boss by making a copy of the prefab
            GameObject newCol = Instantiate(boss);
            //postion the Boss
            newCol.transform.position = spawnLocation;

            newCol.transform.parent = this.transform;

            hasBegun = true;
        }
    }


    private void SpawnEndLevel()
    {
        //create new EndLevel by making a copy of the prefab
        GameObject newEnd = Instantiate(endLevel);

        newEnd.transform.position = placement;

        newEnd.transform.parent = this.transform;
    }
}
