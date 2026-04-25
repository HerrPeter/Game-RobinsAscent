using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
    public float destroyOffset = 8f;

    // Checks if the object has moved below the camera's view by a certain offset and destroys it if it has, preventing off-screen objects from consuming resources unnecessarily.
    void Update()
    {
        if (Camera.main == null) return;

        if (transform.position.y < Camera.main.transform.position.y - destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}