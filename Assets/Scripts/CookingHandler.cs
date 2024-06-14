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

        // add ingredients back to draw pile
        DeckHandler.put_ingredients_back_to_deck(ingredients);

        // Clear the ingredients (for next dish)
        ingredients.Clear();

        // Gegner Schaden zufügen
        FightingHandler.hit_with_dish(efficiency);

        // for getting new cards after cooking
        List<GameObject> deactivated_cards = get_deactivated_cards();
        foreach(GameObject card in deactivated_cards)
        {
            card.SetActive(true);
            card.GetComponent<FoodCardBehaviour>().load_new_card();
        }
    }

    private static List<GameObject> get_deactivated_cards()
    {
        List<GameObject> deactivated_cards = new List<GameObject>();
        foreach (GameObject card in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (card.CompareTag("card") && !card.activeInHierarchy)
            {
                deactivated_cards.Add(card);
            }
        }
        return deactivated_cards;
    }

}
