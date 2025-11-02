using UnityEngine;

public class BreathDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player p = col.GetComponent<Player>();
            if (p != null)
            {
                p.Morrer();
            }
        }
    }
}