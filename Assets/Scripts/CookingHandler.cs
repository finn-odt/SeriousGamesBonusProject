using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CookingHandler
{

    private static List<CardType> ingredients;

    public static bool are_there_ingredients()
    {
        if(ingredients == null || ingredients.Count == 0)
        {
            return false;
        }
        return true;
    }

    public static void add_ingredient(CardType card)
    {
        if (ingredients == null)
        {
            ingredients = new List<CardType>();
        }
        ingredients.Add(card);
    }

    public static void cook()
    {
        if (ingredients == null)
        {
            return;
        }

        double effectivity = DeckHandler.detect_dish_quality(ingredients);

        double efficiency = 0;
        foreach(CardType card in ingredients)
        {
            efficiency += DeckHandler.get_efficiency_of_card(card);
        }
        efficiency = efficiency * effectivity;

        Debug.Log("Cooking with efficiency: " + efficiency);

        ingredients.Clear();

        List<GameObject> deaktivated_cards = get_deaktivated_cards();
        foreach(GameObject card in deaktivated_cards)
        {
            card.SetActive(true);
        }
    }

    private static List<GameObject> get_deaktivated_cards()
    {
        List<GameObject> deaktivated_cards = new List<GameObject>();
        foreach (GameObject card in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (card.CompareTag("card") && !card.activeInHierarchy)
            {
                deaktivated_cards.Add(card);
            }
        }
        return deaktivated_cards;
    }

}
