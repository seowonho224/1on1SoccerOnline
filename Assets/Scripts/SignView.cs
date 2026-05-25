using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 유니티 UI 컴포넌트를 사용하기 위해 필수 추가
using GooglePlayGames;

public class SignView : MonoBehaviour
{
    // [선택] 디버깅 로그를 화면에 띄우고 싶다면 Canvas 내의 Text 컴포넌트를 연결합니다.
    [SerializeField] private Text logText;

    // 전환할 다음 화면 (LobbyView 또는 GameView)
    [SerializeField] private GameObject lobbyView;

    private void Start()
    {
        // 게임 시작 시 GPGS 초기화 수행
        GPGSBinder.Inst.Init(); 
        UpdateLog("GPGS Initialized.");
    }

    /// <summary>
    /// 유니티 UI Button의 OnClick()에 연결할 로그인 함수
    /// </summary>
    public void OnClickLogin()
    {
        UpdateLog("접속 중...");

        GPGSBinder.Inst.Login((success, userName, userId) =>
        {
            if (success)
            {
                UpdateLog($"[성공] 유저: {userName}");

                // 로그인에 성공했으므로 현재 SignView는 끄고, 로비(또는 게임) 화면을 켭니다.
                ChangeToLobbyView();
            }
            else
            {
                UpdateLog("[실패] GPGS 로그인 실패");
            }
        });
    }

    /// <summary>
    /// 다음 화면으로 전환하는 함수
    /// </summary>
    private void ChangeToLobbyView()
    {
        if (lobbyView != null)
        {
            lobbyView.SetActive(true);    // 로비 화면 켜기
            this.gameObject.SetActive(false); // 현재 로그인 화면 끄기
        }
        else
        {
            Debug.LogError("LobbyView 오브젝트가 지정되지 않았습니다!");
        }
    }

    /// <summary>
    /// 텍스트 컴포넌트와 콘솔창에 로그를 동시에 기록하는 함수
    /// </summary>
    private void UpdateLog(string message)
    {
        Debug.Log(message);
        if (logText != null)
        {
            logText.text = message;
        }
    }
}