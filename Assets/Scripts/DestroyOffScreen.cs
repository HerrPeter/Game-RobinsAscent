using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
    public float destroyOffset = 8f;

    void Update()
    {
        if (Camera.main == null) return;

        if (transform.position.y < Camera.main.transform.position.y - destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}