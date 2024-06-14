using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckCounterBehaviour : MonoBehaviour
{
    public TextMeshProUGUI counter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter.text = "Deck: " + DeckHandler.get_amount_of_deck_cards();
    }
}
