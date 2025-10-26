using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainClass : MonoBehaviour
{
    //send to Another Manager
    public static Action onReset;

    [Header("Gameplay Settings")]
    public GameObject itemPrefab;
    public Transform player;
    public TextMeshProUGUI info;
    public int MAX = 50;
    public float range = 25f;

    [Header("Scene References")]
    public Light[] mainLight;

    [HideInInspector]
    public bool started = false;

    public static List<GameObject> ALL = new List<GameObject>();
    public static MainClass instance;

    private float avgDistance = 0f;
    private float distanceUpdateTimer = 0f;
    private const float distanceUpdateInterval = 0.2f;

    void Awake()
    {
        instance = this;
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
            g.transform.position = new Vector3(UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(2, 5), UnityEngine.Random.Range(-range, range));
            g.transform.localScale = new Vector3(1, 1, 1) * UnityEngine.Random.Range(0.2f, 1.7f);
            g.name = "junk_" + i;
            var b = g.GetComponent<RandomThings>();
            if (b != null) b.main = this;
            ALL.Add(g);
        }

        //do nothing
        //gameObject.AddComponent<Manager1>();
        //gameObject.AddComponent<Manager2>();

        gameObject.AddComponent<AnotherManager>();
        yield return null;
    }


    void Update()
    {
        if (!started) return;

        distanceUpdateTimer += Time.deltaTime;
        if (distanceUpdateTimer >= distanceUpdateInterval)
        {
            distanceUpdateTimer = 0f;
            UpdateAverageDistance();
        }

        info.text = $"Collected: {AnotherManager._CollectedCount}/{MAX} Avg: {avgDistance:F1}";

        // Event-based Reset
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onReset?.Invoke();
        }
    }
    private void UpdateAverageDistance()
    {
        float sum = 0f;
        int count = 0;

        foreach (var o in ALL)
        {
            if (o != null)
            {
                sum += Vector3.Distance(player.position, o.transform.position);
                count++;
            }
        }

        avgDistance = (count > 0) ? sum / count : 0f;
    }
}