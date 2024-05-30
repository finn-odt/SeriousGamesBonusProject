using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingHandler : MonoBehaviour
{
    public static double enemyHealth = 100;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        if (enemy == null)
        {
            enemy = GameObject.FindWithTag("enemy");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0.0)
        {
            Destroy(enemy); // Entfernt den Gegner aus der Szene
            Debug.Log("Gegner besiegt!");
        }
    }

    public static void hit_with_card(CardType card) {
        double damage = CalculateDamage(card);
        enemyHealth -= damage;
        Debug.Log("Gegner getroffen! Verbleibende Gesundheit: " + enemyHealth);
    }

    private static double CalculateDamage(CardType card)
    {
        return 30 * DeckHandler.get_efficiency_of_card(card);
    }
}
