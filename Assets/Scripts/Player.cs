using UnityEngine;

public class Player : MonoBehaviour
{
    public int power;
    public int uppower;
    public int speed;
    public int jump;

    public bool isGrounded = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SoccerBall"))
        {
            Rigidbody ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (ballRigidbody != null)
            {
                float pushX = collision.transform.position.x - transform.position.x;
                
                float horizontalDirection = pushX > 0 ? 1f : -1f;

                Vector3 finalKickForce = new Vector3(horizontalDirection * power, uppower, 0f);

                // 공의 기존 속도를 깔끔하게 지워줍니다 (기존 물리 가속도 상쇄)
                ballRigidbody.linearVelocity = Vector3.zero;

                // 순간적인 충격량(Impulse)으로 대각선 위로 뻥 차버립니다!
                ballRigidbody.AddForce(finalKickForce, ForceMode.Impulse);
            }
        }

        // 바닥 오브젝트의 태그를 "Ground"로 설정해두면 안전합니다.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
