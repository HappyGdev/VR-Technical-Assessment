using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class RandomThings : MonoBehaviour
{
    //do ta kar anjam shod: 1:rang va charkhesh be sorate meghdar avalieh be system dadeh shodan va dar har frame taghir nemikonan
    //2:system dar halate ghabli har sanieh 60 bar check mikard alan har 0.1f sanieh yek bar chek mikoneh va mitooneh nafas bekeshe ba coroutine
    public MainClass main;
    private bool taken = false;
    private MeshRenderer rend;
    private Color originalColor;

    //make rotation Seprate from Update
    [SerializeField] private float rotationSpeed = 30f;

    //instead of update use coroutines
    private static readonly float sphereRadius = 1f;
    private static readonly float maxRayDistance = 5f;
    [SerializeField] private float checkInterval = 0.1f;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material.color = Color.white;

        float c = Random.Range(0f, 1f);
        if (c >= 0.5f)
        {
            originalColor = Color.red;
        }
        else
        {
            originalColor = Color.green;
        }
    }

    //start coroutine
    private void Start()
    {
        StartCoroutine(CheckLightHitsRoutine());
    }

    public void doRotate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }


    private IEnumerator CheckLightHitsRoutine()
    {
        while (true)
        {
            CheckLightHits();
            //make system Breathing
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void CheckLightHits()
    {
        //early returns
        if (main == null)
        {
            return;
        }
        if (main.mainLight == null || main.mainLight.Length == 0)
        {
            return;
        }

        bool hitByLight = false;

        foreach (var light in main.mainLight)
        {
            if (light == null) continue;

            Ray ray = new Ray(light.transform.position, light.transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, sphereRadius, out hit, maxRayDistance))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    RandomThings rt = hit.collider.GetComponent<RandomThings>();
                    if (rt != null)
                    {
                        rt.rend.material.color = originalColor;
                    }
                    hitByLight = true;
                    break;
                }
            }
        }

        if (hitByLight)
        {
            rend.material.color = originalColor;
        }
        else
        {
            rend.material.color = Color.white;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //early returns
        if (taken)
        {
            return;
        }
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        taken = true;

        if (originalColor == Color.red)
        {
            AnotherManager._CollectedCount += -1;
        }
        else
        {
            AnotherManager._CollectedCount += 1;
        }

        MainClass.ALL.Remove(gameObject);
        UtilityStuff._Put("lastcol", Time.time);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        //early returns
        if (main == null)
        {
            return;
        }
        if (main.mainLight == null || main.mainLight.Length == 0)
        {
            return;
        }

        Gizmos.color = originalColor;
        foreach (var light in main.mainLight)
        {
            if (light == null) continue;

            Vector3 start = light.transform.position;
            Vector3 end = start + light.transform.forward * maxRayDistance;

            Gizmos.DrawLine(start, end);
            Gizmos.DrawWireSphere(start, 0.2f);
        }
    }
}
