using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Player player;
    private Rigidbody playerRb;

    private bool isLeftPressed = false;
    private bool isRightPressed = false;
    private float baseScaleX; // 원래 X 스케일 저장
    private float speed;
    private float jumpForce;

    public Text text;
    private float currentTime = 100f; // 시작 시간
    private bool isTimerRunning = true;
    void Start()
    {
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody>();
            if (playerRb == null)
            {
                // Rigidbody가 없으면 자동으로 추가 (안전장치)
                playerRb = player.gameObject.AddComponent<Rigidbody>();
                playerRb.constraints = RigidbodyConstraints.FreezeRotation; // 회전 방지
            }
            // 시작할 때 원래의 X 스케일 값을 저장합니다. (예: 0.1f)
            baseScaleX = Mathf.Abs(player.transform.localScale.x);
            speed = 6.0f + player.speed * 0.2f;
            jumpForce = 22.0f + player.jump;
        }

        UpdateTimerText();
    }

    void Update()
    {
        // 타이머 감소 로직
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime; // 매 프레임마다 시간 감소

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isTimerRunning = false;
                // 시간이 다 되면 게임 오브젝트 비활성화
                gameObject.SetActive(false);
            }

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        // 시간을 정수로 변환하여 표시 (예: 100, 99...)
        text.text = Mathf.CeilToInt(currentTime).ToString();
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    // --- 좌우 이동 처리 ---
    private void MoveCharacter()
    {
        if (playerRb == null) return;

        float moveDirection = 0f;
        if (isLeftPressed) moveDirection = -1f;
        else if (isRightPressed) moveDirection = 1f;

        // X축 속도만 변경하여 물리적으로 자연스럽게 이동합니다.
        // Y축 속도(점프/낙하)는 유지합니다.
        Vector3 newVelocity = new Vector3(moveDirection * speed, playerRb.linearVelocity.y, playerRb.linearVelocity.z);
        playerRb.linearVelocity = newVelocity;

        // 방향 전환 (Z축 스케일 고정)
        if (moveDirection != 0f)
        {
            float scaleX = moveDirection * baseScaleX;
            player.transform.localScale = new Vector3(scaleX, player.transform.localScale.y, player.transform.localScale.z);
        }
    }

    // --- 왼쪽 버튼 이벤트 연동 ---
    public void SetLeftPressed(bool isPressed)
    {
        isLeftPressed = isPressed;
    }

    // --- 오른쪽 버튼 이벤트 연동 ---
    public void SetRightPressed(bool isPressed)
    {
        isRightPressed = isPressed;
    }

    // --- 점프 버튼 이벤트 연동 함수 ---
    public void OnTouchJumpButton()
    {
        // 바닥에 닿아있을 때만 점프가 가능하도록 제한 (무한 점프 방지)
        if (playerRb != null && player.isGrounded)
        {
            // 위쪽 방향으로 순간적인 힘(Impulse)을 가합니다.
            playerRb.AddForce(Vector3.up * player.jump, ForceMode.Impulse);
            player.isGrounded = false; // 점프를 했으니 공중 상태로 변경
        }
    }
}