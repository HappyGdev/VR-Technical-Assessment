using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class AnotherManager : MonoBehaviour
{
    public static int _CollectedCount = 0;
    public List<GameObject> spawned = new List<GameObject>();


    void Start()
    {
        StartCoroutine(WeirdLoop());
    }


    IEnumerator WeirdLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (MainClass.me != null && MainClass.ALL.Count < 100)
            {
                var p = MainClass.me.itemPrefab;
                var go = Instantiate(p);
                go.transform.position = new Vector3(Random.Range(-10,10),Random.Range(2, 7),Random.Range(-10,10));
                spawned.Add(go);
                MainClass.ALL.Add(go);
                var rt = go.GetComponent<RandomThings>(); if (rt != null) rt.main = MainClass.me;
            }
        }
    }
    
    public void ResetAll()
    {
        foreach (var g in spawned) if (g != null) Destroy(g);
        spawned.Clear();
        UtilityStuff._KillAll();
        _CollectedCount = 0;
        MainClass.me.StartCoroutine("doStart");
    }
}