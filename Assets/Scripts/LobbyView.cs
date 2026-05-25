using UnityEngine;

public class LobbyView : MonoBehaviour
{
    public GameObject gameView;
    public GameObject shopView;

    public void OnClickMatchingButton()
    {
        gameObject.SetActive(false);
        gameView.SetActive(true);
    }

    public void OnClickLockerRoomButton()
    {
        gameObject.SetActive(false);
        shopView.SetActive(true);
    }
}
