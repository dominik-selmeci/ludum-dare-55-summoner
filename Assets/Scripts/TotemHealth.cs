using UnityEngine;

public class TotemHealth : MonoBehaviour
{
    public void SetHealth(float percentage)
    {
        Vector3 newScale = transform.localScale;
        newScale.x = percentage;
        transform.localScale = newScale;
    }
}
