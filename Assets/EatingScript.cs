using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingScript : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter(Collider other)
    {
        HealingItem tempHealingItem;

        if (other.TryGetComponent<HealingItem>(out tempHealingItem) && player.health < 5)
        {
            player.takeHeal(tempHealingItem.healAmount);
            Destroy(other.gameObject);
        }
    }
}
