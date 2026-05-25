using UnityEngine;

public class ShopView : MonoBehaviour
{
    public GameObject equip;
    public GameObject lobbyView;

    public void OnClickLobbyButton()
    {
        gameObject.SetActive(false);
        lobbyView.SetActive(true);
    }
}
