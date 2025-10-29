using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class AnotherManager : MonoBehaviour
{
    public static int _CollectedCount = 0;
    public List<GameObject> spawned = new List<GameObject>();

    private void OnEnable()
    {
        MainClass.onReset += ResetAll;
    }
    private void OnDisable()
    {
        MainClass.onReset -= ResetAll;
    }

    void Start()
    {
        //StartCoroutine(WeirdLoop());
       StartCoroutine(SpawnItem());
    }


    //IEnumerator WeirdLoop()
    IEnumerator SpawnItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (MainClass.instance != null && MainClass.ALL.Count < 100)
            {
                var p = MainClass.instance.itemPrefab;
                var go = Instantiate(p);
                go.transform.position = new Vector3(Random.Range(-10,10),Random.Range(2, 7),Random.Range(-10,10));
                spawned.Add(go);
                MainClass.ALL.Add(go);
                var rt = go.GetComponent<RandomThings>(); if (rt != null) rt.main = MainClass.instance;
            }
        }
    }
    
    public void ResetAll()
    {
        foreach (var g in spawned) if (g != null) Destroy(g);
        spawned.Clear();
        UtilityStuff._KillAll();
        _CollectedCount = 0;
        MainClass.instance.StartCoroutine("doStart");
    }
}