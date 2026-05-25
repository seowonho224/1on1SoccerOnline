using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform ball;

    void LateUpdate()
    {
        if (-6.7f < ball.position.x && ball.position.x < 38.1f)
        {
            transform.position = new Vector3(ball.position.x, transform.position.y, transform.position.z);
        }
    }
}