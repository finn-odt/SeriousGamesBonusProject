using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameStatus
{
    START,
    FIGHTING,
    WIN,
    LOSE
}

public class GameStatusHandler : MonoBehaviour
{
    //Singleton-Klasse
    public static GameStatusHandler Instance { get; private set; }

    private GameObject status_object;
    private GameObject start_button;
    private GameObject overlay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        status_object = FindGameObjectWithTag("status_text");
        start_button = FindGameObjectWithTag("start_button");
        overlay = FindGameObjectWithTag("overlay");

        if (status_object == null || start_button == null || overlay == null)
        {
            Debug.LogWarning("Status object or start button or overlay not found in Awake.");
        }
        else
        {
            status_object.SetActive(false);
            start_button.SetActive(false);
            overlay.SetActive(false);
        }
    }

    private GameObject FindGameObjectWithTag(string tag)
    {
        Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        foreach (Transform t in transforms)
        {
            if (t.hideFlags == HideFlags.None && t.CompareTag(tag))
            {
                return t.gameObject;
            }
        }
        return null;
    }


    public void update_game_status(GameStatus status)
    {
        Debug.Log("Game Status Change: " + status.ToString());

        //if (status_object != null && start_button != null)
        //{
            Debug.Log("Game Status Change | UI-elements found");

            TextMeshProUGUI status_text = status_object.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI button_text = start_button.GetComponentInChildren<TextMeshProUGUI>();
            if (status == GameStatus.START)
            {
                // Status Text
                status_text.text = "Let's start the game!";
                status_object.SetActive(true);

                // Start Button
                button_text.text = "Start";
                start_button.SetActive(true);

                // Overlay
                overlay.SetActive(true);
            }
            else if (status == GameStatus.FIGHTING)
            {
                status_object.SetActive(false);
                start_button.SetActive(false);
                overlay.SetActive(false);
            }
            else
            {
                Debug.Log("Game Status Change WIN/LOSE");

                // Status Text
                status_text.text = (status == GameStatus.WIN ? "You win!" : "You lose!");
                status_text.color = (status == GameStatus.WIN ? new Color(0, 255, 0) : new Color(255, 0, 0));
                status_object.SetActive(true);

                // Start Button
                button_text.text = "Restart";
                start_button.SetActive(true);

                // Overlay
                overlay.SetActive(true);

                if (status == GameStatus.LOSE) {
                    DeckHandler.reset_deck();
                }
            }
        //}
    }
}
