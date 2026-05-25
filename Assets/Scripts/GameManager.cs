using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject GoalPost1; // 왼쪽 골대
    public GameObject GoalPost2; // 오른쪽 골대
    public GameObject SoccerBall;
    public GameObject Player1; // 유저
    public GameObject Player2; // 유저2


    public Text text; // 점수 텍스트 (예: 0 : 0)

    private int score1 = 0; // 플레이어 1 점수
    private int score2 = 0; // 플레이어 2 점수
    private Vector3 startPosition = new Vector3(15.7f, 2.0f, -4.0f); // 공 초기 위치
    private Vector3 startPosition2 = new Vector3(12.3f, -0.518f, -4.0f); // 선수1 초기 위치
    private Vector3 startPosition3 = new Vector3(19.2f, -0.518f, -4.0f); // 선수2 초기 위치

    void Start()
    {
        UpdateScoreText();
    }

    // 공이 골대에 닿았을 때 호출되는 함수
    public void OnGoal(int playerNumber)
    {
        if (playerNumber == 1) score2++; // 골대 1에 들어가면 플레이어 2 점수 상승
        else score1++; // 골대 2에 들어가면 플레이어 1 점수 상승

        // 공 위치 초기화
        SoccerBall.transform.position = startPosition;

        // 선수 위치 초기화
        Player1.transform.position = startPosition2;
        Player2.transform.position = startPosition3;

        // 공의 속도 초기화 (물리 엔진 멈춤)
        Rigidbody rb = SoccerBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        text.text = $"{score1} : {score2}";
    }
}