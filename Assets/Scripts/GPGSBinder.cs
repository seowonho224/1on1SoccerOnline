using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi.Events;

public class GPGSBinder
{
    private static readonly GPGSBinder inst = new GPGSBinder();
    public static GPGSBinder Inst => inst;

    // v2.x 버전에서도 여전히 Instance를 통해 클라이언트 접근 가능
    private ISavedGameClient SavedGame => PlayGamesPlatform.Instance.SavedGame;
    private IEventsClient Events => PlayGamesPlatform.Instance.Events;

    /// <summary>
    /// GPGS 초기화
    /// </summary>
    public void Init()
    {
        // [중요] v2.x 버전부터는 PlayGamesClientConfiguration 클래스가 완전히 삭제되었습니다.
        // 클라우드 저장(SavedGame) 활성화는 유니티 에디터의 [Android Setup] 창에서 체크박스를 통해 설정해야 합니다.
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    /// <summary>
    /// 로그인 (인증)
    /// </summary>
    /// <param name="onLoginSuccess">성공 여부(bool), 사용자 이름(string), 사용자 고유 ID(string)</param>
    public void Login(Action<bool, string, string> onLoginSuccess = null)
    {
        Init();

        PlayGamesPlatform.Instance.Authenticate((SignInStatus status) =>
        {
            if (status == SignInStatus.Success)
            {
                // 최신 버전에서 안전하게 유저 정보를 가져오는 방법
                string userName = PlayGamesPlatform.Instance.GetUserDisplayName();
                string userId = PlayGamesPlatform.Instance.GetUserId();

                onLoginSuccess?.Invoke(true, userName, userId);
            }
            else
            {
                onLoginSuccess?.Invoke(false, null, null);
            }
        });
    }

    /// <summary>
    ///// 로그아웃
    ///// </summary>
    public void Logout()
    {
        //PlayGamesPlatform.Instance.SignOut();
    }

    #region 클라우드 저장 (Saved Game)
    public void SaveCloud(string fileName, string saveData, Action<bool> onCloudSaved = null)
    {
        if (SavedGame == null)
        {
            Debug.LogError("GPGS: 클라우드 저장이 활성화되지 않았습니다. Android Setup 창을 확인하세요.");
            onCloudSaved?.Invoke(false);
            return;
        }

        SavedGame.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood, (SavedGameRequestStatus status, ISavedGameMetadata game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    var update = new SavedGameMetadataUpdate.Builder().Build();
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(saveData);
                    SavedGame.CommitUpdate(game, update, bytes, (SavedGameRequestStatus status2, ISavedGameMetadata game2) =>
                    {
                        onCloudSaved?.Invoke(status2 == SavedGameRequestStatus.Success);
                    });
                }
                else
                {
                    onCloudSaved?.Invoke(false);
                }
            });
    }

    public void LoadCloud(string fileName, Action<bool, string> onCloudLoaded = null)
    {
        if (SavedGame == null)
        {
            onCloudLoaded?.Invoke(false, null);
            return;
        }

        SavedGame.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood, (SavedGameRequestStatus status, ISavedGameMetadata game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    SavedGame.ReadBinaryData(game, (SavedGameRequestStatus status2, byte[] loadedData) =>
                    {
                        if (status2 == SavedGameRequestStatus.Success)
                        {
                            string data = System.Text.Encoding.UTF8.GetString(loadedData);
                            onCloudLoaded?.Invoke(true, data);
                        }
                        else
                        {
                            onCloudLoaded?.Invoke(false, null);
                        }
                    });
                }
                else
                {
                    onCloudLoaded?.Invoke(false, null);
                }
            });
    }

    public void DeleteCloud(string fileName, Action<bool> onCloudDeleted = null)
    {
        if (SavedGame == null)
        {
            onCloudDeleted?.Invoke(false);
            return;
        }

        SavedGame.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, (SavedGameRequestStatus status, ISavedGameMetadata game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    SavedGame.Delete(game);
                    onCloudDeleted?.Invoke(true);
                }
                else
                {
                    onCloudDeleted?.Invoke(false);
                }
            });
    }
    #endregion

    #region 업적 (Achievements)
    public void ShowAchievementUI() =>
        PlayGamesPlatform.Instance.ShowAchievementsUI();

    public void UnlockAchievement(string gpgsId, Action<bool> onUnlocked = null) =>
        PlayGamesPlatform.Instance.UnlockAchievement(gpgsId, success => onUnlocked?.Invoke(success));

    public void IncrementAchievement(string gpgsId, int steps, Action<bool> onUnlocked = null) =>
        PlayGamesPlatform.Instance.IncrementAchievement(gpgsId, steps, success => onUnlocked?.Invoke(success));
    #endregion

    #region 리더보드 (Leaderboards)
    public void ShowAllLeaderboardUI() =>
        PlayGamesPlatform.Instance.ShowLeaderboardUI();

    public void ShowTargetLeaderboardUI(string gpgsId) =>
        PlayGamesPlatform.Instance.ShowLeaderboardUI(gpgsId);

    public void ReportLeaderboard(string gpgsId, long score, Action<bool> onReported = null) =>
        PlayGamesPlatform.Instance.ReportScore(score, gpgsId, success => onReported?.Invoke(success));

    public void LoadCustomLeaderboardArray(string gpgsId, int rowCount, LeaderboardStart leaderboardStart,
        LeaderboardTimeSpan leaderboardTimeSpan, Action<bool, LeaderboardScoreData> onloaded = null)
    {
        PlayGamesPlatform.Instance.LoadScores(gpgsId, leaderboardStart, rowCount, LeaderboardCollection.Public, leaderboardTimeSpan, data =>
        {
            onloaded?.Invoke(data.Status == ResponseStatus.Success, data);
        });
    }
    #endregion

    #region 이벤트 (Events)
    public void IncrementEvent(string gpgsId, uint steps)
    {
        Events.IncrementEvent(gpgsId, steps);
    }

    public void LoadEvent(string gpgsId, Action<bool, IEvent> onEventLoaded = null)
    {
        Events.FetchEvent(DataSource.ReadCacheOrNetwork, gpgsId, (ResponseStatus status, IEvent iEvent) =>
        {
            onEventLoaded?.Invoke(status == ResponseStatus.Success, iEvent);
        });
    }

    public void LoadAllEvent(Action<bool, List<IEvent>> onEventsLoaded = null)
    {
        Events.FetchAllEvents(DataSource.ReadCacheOrNetwork, (ResponseStatus status, List<IEvent> events) =>
        {
            onEventsLoaded?.Invoke(status == ResponseStatus.Success, events);
        });
    }
    #endregion
}