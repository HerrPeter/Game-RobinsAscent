using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    // Moves the camera to follow the player, but only if the player is above the camera's current position. This ensures that the camera will not move downwards, providing a better view of the player's ascent while preventing unnecessary downward movement that could hinder visibility.
    void LateUpdate()
    {
        if (player.position.y > transform.position.y)
        {
            transform.position = new Vector3(
                transform.position.x,
                player.position.y,
                transform.position.z
            );
        }
    }
}