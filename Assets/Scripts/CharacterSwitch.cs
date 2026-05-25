using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GameObject player;
    public ShopView shopView;
    public GameObject equip; // 고르는 캐릭터의 equip

    public Sprite character;
    public int power;
    public int uppower;
    public int speed;
    public int jump;

    public void OnClick()
    {
        shopView.equip.SetActive(false); // 그 전의 eqiup 헤재
        equip.SetActive(true);
        shopView.equip = equip; // eqiup 업데이트


        player.GetComponent<SpriteRenderer>().sprite = character;

        var playerState = player.GetComponent<Player>();
        playerState.power = power;
        playerState.uppower = uppower;
        playerState.speed = speed;
        playerState.jump = jump;

        
    }
}
