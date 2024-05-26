using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CookingHandler
{

    private static List<CardType> cards_in_pot;
    // Start is called before the first frame update
    public static void put_in_pot(CardType card)
    {
        if(cards_in_pot == null)
        {
            cards_in_pot = new List<CardType>();
        }
        cards_in_pot.Add(card);
    }

    public static void cook()
    {
        double efficiency = 0;
        foreach(CardType card in cards_in_pot)
        {
            efficiency += DeckHandler.get_efficiency_of_card(card);
        }
        efficiency = efficiency * 1.3;  // 30% Gewinn durch Cooking?
    }
}
