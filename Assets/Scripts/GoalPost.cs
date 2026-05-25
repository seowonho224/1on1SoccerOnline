using UnityEngine;

public class GoalPost : MonoBehaviour
{
    public GameManager gameManager;
    public int playerNumber; // 골대 번호 (1 또는 2)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Soccer Ball")
        {
            gameManager.OnGoal(playerNumber);
        }
    }
}