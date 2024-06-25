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
    public static double points = 0;
    public static bool dead = false;

    private static double current_damage = 0;

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
        if (enemyHealth <= 0.0 && !dead)
        {
            // Enemy is defeated
            dead = true;
            DeckHandler.expand_deck_with_cards();  // draw 7 new random cards to the draw pile
            GameStatusHandler.Instance.update_game_status(GameStatus.WIN);

            enemy.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0); // Gegner wird rot
            Debug.Log("Gegner besiegt!");
        }

        if (healthSlider != null)
        {
            healthSlider.value = (float)enemyHealth;
        }

        // Enemy is getting damage (with update for smoother animation)
        double temp_damage = 65.0 * Time.deltaTime;
        if (current_damage > 0.0)  // damaging of enemy
        {
            enemyHealth -= temp_damage;
            if (current_damage - temp_damage > 0)
            {
                current_damage -= temp_damage;
            }
            else
            {
                current_damage = 0;
            }
        }
        else if (current_damage < 0.0)  // healing of enemy
        {
            enemyHealth += temp_damage;
            if (current_damage + temp_damage < 0)
            {
                current_damage += temp_damage;
            }
            else
            {
                current_damage = 0;
            }
        }
        if (healthSlider != null)
        {
            healthSlider.value = (float)enemyHealth;  // display updated health slider
        }

    }

    public static void hit_with_card(CardType card) {
        double efficiency = DeckHandler.get_efficiency_of_card(card);
        double damage = calculate_damage(efficiency);
        add_score(efficiency * 10);
        current_damage += damage;
    }

    public static void hit_with_dish(double efficiency) {
        double damage = calculate_damage(efficiency);
        add_score(efficiency * 20);
        current_damage += damage;
    }

    private static double calculate_damage(double efficiency)
    {
        return 30 * efficiency;
    }

    private static void add_score(double tmp_points) {
        points += tmp_points;
        Debug.Log("Score: " + points);
        print_score();
    }

    public static void print_score()
    {
        if (score != null)
        {
            score.text = points.ToString("F0");
        }
    }
}
