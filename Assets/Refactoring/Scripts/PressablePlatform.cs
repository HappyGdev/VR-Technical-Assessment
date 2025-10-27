using UnityEngine;

public class PressableButton : MonoBehaviour
{
    [Header("References")]
    public Transform buttonVisual;     // assign child that moves
    public Light directionalLight;     // assign Directional Light here

    [Header("Movement Settings")]
    public float pressDistance = 0.1f;
    public float moveSpeed = 5f;

    [Header("Light Settings")]
    public float normalLightIntensity = 1f;
    public float pressedLightIntensity = 2f;
    public float lightChangeSpeed = 2f;

    private Vector3 originalLocalPos;
    private Vector3 targetLocalPos;
    private bool isPressed = false;
    private float currentLightIntensity;

    [SerializeField] ParticleSystem hitParticle;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        if (buttonVisual == null)
        {
            Debug.LogError("PressableButton: Missing buttonVisual reference!");
            enabled = false;
            return;
        }

        originalLocalPos = buttonVisual.localPosition;
        targetLocalPos = originalLocalPos;

        if (directionalLight != null)
            currentLightIntensity = directionalLight.intensity;
    }

    void Update()
    {
        // Move the button smoothly
        buttonVisual.localPosition = Vector3.Lerp(
            buttonVisual.localPosition,
            targetLocalPos,
            Time.deltaTime * moveSpeed
        );

        // Smoothly update light intensity
        if (directionalLight != null)
        {
            float targetIntensity = isPressed ? pressedLightIntensity : normalLightIntensity;
            currentLightIntensity = Mathf.Lerp(currentLightIntensity, targetIntensity, Time.deltaTime * lightChangeSpeed);
            directionalLight.intensity = currentLightIntensity;
        }
    }

    public void SetPressed(bool pressed)
    {
        isPressed = pressed;
        audioSource.Play();
        targetLocalPos = pressed
            ? originalLocalPos - new Vector3(0, pressDistance, 0)
            : originalLocalPos;

        if (pressed)
            hitParticle.Play(); 
    }
}
