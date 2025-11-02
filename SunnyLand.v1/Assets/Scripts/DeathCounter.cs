using UnityEngine;
using TMPro; // <<< IMPORTANTE

public class DeathCounter : MonoBehaviour
{
    public static int deathCount = 0;
    public TextMeshProUGUI deathText; // <<< TMP EM VEZ DE TEXT

    void Start()
    {
        UpdateDeathText();
    }

    public void AddDeath()
    {
        deathCount++;
        UpdateDeathText();
    }

    void UpdateDeathText()
    {
        deathText.text = "Mortes: " + deathCount.ToString();
    }
}