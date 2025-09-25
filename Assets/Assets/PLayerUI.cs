using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text healthText;
    public Text killsText;

    public PlayerController player; // script que tem as variáveis health e kills

    void Update()
    {
        if (player == null) return;

        healthText.text = "Vida: " + player.health.ToString();
        killsText.text = "Derrotou: " + player.killCount.ToString();
    }
}
