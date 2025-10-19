using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainClass : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform player;
    public TextMeshProUGUI info;
    public int MAX = 50;
    public float range = 25f;
    public bool started = false;
    public Light[] mainLight;
    
    public static List<GameObject> ALL = new List<GameObject>();
    public static MainClass me;


    void Awake()
    {
        me = this; 
    }


    void Start()
    {
        StartCoroutine(doStart());
    }


    IEnumerator doStart()
    {
        yield return new WaitForSeconds(0.5f);
        started = true;
        for (int i = 0; i < MAX; i++)
        {
            GameObject g = Instantiate(itemPrefab);
            g.transform.position = new Vector3(Random.Range(-range, range), Random.Range(2, 5), Random.Range(-range, range));
            g.transform.localScale = new Vector3(1,1,1)*Random.Range(0.2f,1.7f);
            g.name = "junk_" + i;
            var b = g.GetComponent<RandomThings>();
            if (b != null) b.main = this;
            ALL.Add(g);
        }

        gameObject.AddComponent<Manager1>();
        gameObject.AddComponent<Manager2>();
        gameObject.AddComponent<AnotherManager>();
        yield return null;
    }


    void Update()
    {
        if (!started) return;
        float avg = 0f;
        int cnt = 0;
        foreach (var o in ALL)
        {
            if (o != null)
            {
                avg += Vector3.Distance(player.position, o.transform.position);
                cnt++;
            }
        }

        if (cnt > 0) avg /= cnt;
        
        // TODO: Display the average value with one decimal place
        info.text = "Collected:" + AnotherManager._CollectedCount + "/" + MAX + " Avg:" + avg;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            var am = FindObjectOfType<AnotherManager>();
            if (am != null)
                am.ResetAll();
        }
        
        var temp = new List<GameObject>();
        foreach (var o in ALL)
            if (o != null && o.activeSelf)
                temp.Add(o);
    }
}