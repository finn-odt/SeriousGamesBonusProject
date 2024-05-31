using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightingHandler : MonoBehaviour
{
    public static double enemyHealth = 100;
    public GameObject enemy;
    public static double maxHealth = 100;
    public static Slider healthSlider;
    public static TMPro.TextMeshProUGUI score;
    private static double points = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (enemy == null)
        {
            enemy = GameObject.FindWithTag("enemy");
        }

        GameObject healthSliderObject = GameObject.FindWithTag("health_bar");
        if (healthSliderObject != null)
        {
            healthSlider = healthSliderObject.GetComponent<Slider>();
            healthSlider.maxValue = (float)maxHealth;
            healthSlider.value = (float)enemyHealth;
        }

        GameObject scoreObject = GameObject.FindWithTag("score");
        if (scoreObject != null)
        {
            score = scoreObject.GetComponent<TMPro.TextMeshProUGUI>();
        }
        Debug.Log(score);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0.0)
        {
            Destroy(enemy); // Entfernt den Gegner aus der Szene
            Debug.Log("Gegner besiegt!");
        }

        if (healthSlider != null)
        {
            healthSlider.value = (float)enemyHealth;
        }
    }

    public static void hit_with_card(CardType card) {
        double efficiency = DeckHandler.get_efficiency_of_card(card);
        double damage = CalculateDamage(efficiency);
        add_score(efficiency * 10);
        hit(damage);
    }

    public static void hit_with_dish(double efficiency) {
        double damage = CalculateDamage(efficiency);
        add_score(efficiency * 20);
        hit(damage);
    }

    private static void hit(double damage) {
        enemyHealth -= damage;
        if (enemyHealth > maxHealth) enemyHealth = maxHealth;
        Debug.Log("Gegner getroffen! Verbleibende Gesundheit: " + enemyHealth);

        if (healthSlider != null)
        {   
            healthSlider.value = (float)enemyHealth;
        }
    }

    private static double CalculateDamage(double efficiency)
    {
        return 30 * efficiency;
    }

    private static void add_score(double tmp_points) {
        points += tmp_points;
        Debug.Log("Score: " + points);
        if (score != null)
        {
            score.text = points.ToString("F0");
        }
    }
}
