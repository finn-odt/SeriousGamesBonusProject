using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CookingHandler
{

    private static List<CardType> ingredients;
    private static int ingredient_index = 1;

    public static bool are_there_ingredients()
    {
        if(ingredients == null || ingredients.Count == 0)
        {
            return false;
        }
        return true;
    }

    public static void clear_ingredients()
    {
        ingredients.Clear();

        ingredient_index = 1;

        GameObject ingredients_list = GameObject.FindWithTag("ingredients_list");

        if (ingredients_list != null)
        {
            // Alle child-Objekte durchgehen
            foreach (Transform child in ingredients_list.transform)
            {
                // Sicherstellen, dass das child ein SpriteRenderer hat
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                // Das Sprite auf None setzen
                spriteRenderer.sprite = null;
            }
        }
    }

    public static void add_ingredient(CardType card)
    {
        if (ingredients == null)
        {
            ingredients = new List<CardType>();
        }
        ingredients.Add(card);

        Sprite icon_sprite = Resources.Load<Sprite>("Images/cards/food/" + card.ToString().ToLower());
        GameObject ingredient_icon = GameObject.FindWithTag("ingredient_" + ingredient_index);
        if (ingredient_icon != null)
        {
            ingredient_icon.GetComponent<SpriteRenderer>().sprite = icon_sprite;
        }
        ingredient_index++;
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
        clear_ingredients();

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
