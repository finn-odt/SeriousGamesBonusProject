using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonBehaviour : MonoBehaviour
{
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onButtonPressed()
    {
        Debug.Log("button pressed");
        SceneManager.LoadScene("MainScene");
        GameStatusHandler.Instance.update_game_status(GameStatus.FIGHTING);
        FightingHandler.enemyHealth = 100;
        FightingHandler.dead = false;
        FightingHandler.points = 0;
        FightingHandler.print_score();

        GameObject enemy = GameObject.FindWithTag("enemy"); 
        if (enemy != null)
        {
            enemy.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }

    public void pointerEnteredButton()
    {
        if (button.interactable)
        {
            Image buttonImage = button.GetComponent<Image>();

            if (buttonImage != null)
            {
                buttonImage.color = new Color(0.8f, 0.8f, 0.8f); //new Color(226, 255, 223);
            }
        }
    }

    public void pointerLeftButton()
    {
        button.GetComponent<Image>().color = new Color(255, 255, 255);
    }
}
