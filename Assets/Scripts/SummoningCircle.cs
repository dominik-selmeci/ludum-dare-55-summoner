using UnityEngine;

public class SummoningCircle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player == null) return;

        player.StartSummoningTime();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player == null) return;

        player.StopSummoningTime();
    }
}
