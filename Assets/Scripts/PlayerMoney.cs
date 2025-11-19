using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [Tooltip("Starting money for the player")]
    public int money = 0;

    public void AddMoney(int amount)
    {
        if (amount <= 0) return;
        money += amount;
        Debug.Log("Added money: " + amount + " | Total: " + money);
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0) return true;

        if (money >= amount)
        {
            money -= amount;
            Debug.Log("Spent money: " + amount + " | Total: " + money);
            return true;
        }

        Debug.Log("Not enough money to spend: " + amount);
        return false;
    }
}
