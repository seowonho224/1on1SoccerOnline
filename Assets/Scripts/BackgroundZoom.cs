using UnityEngine;

public class BackgroundZoom : MonoBehaviour
{
    public float zoomSpeed = 0.05f; // 줌 속도
    public float maxZoom = 1.2f;    // 최대 확대 배율 (1.2배)

    private RectTransform rectTransform;
    private Vector3 initialScale;

    void Start()
    {
        // Panel의 RectTransform을 가져옵니다.
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }

    void Update()
    {
        // 매 프레임마다 조금씩 scale을 키웁니다.
        if (rectTransform.localScale.x < maxZoom)
        {
            rectTransform.localScale += Vector3.one * zoomSpeed * Time.deltaTime;
        }
        else
        {
            // 최대치에 도달하면 다시 처음으로 돌리거나 멈출 수 있습니다.
            // 아래 코드는 다시 처음으로 부드럽게 되돌리는 예시입니다 (반복하고 싶을 때 사용)
             rectTransform.localScale = initialScale;
        }
    }
}