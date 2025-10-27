using UnityEngine;
public class ButtonCollisionHandler : MonoBehaviour
{
    private PressableButton parentButton;

    void Start()
    {
        parentButton = GetComponentInParent<PressableButton>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.isTrigger)
            parentButton.SetPressed(true);
    }

    void OnCollisionExit(Collision collision)
    {
        if (!collision.collider.isTrigger)
            parentButton.SetPressed(false);
    }
}
