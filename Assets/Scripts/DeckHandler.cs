using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CardType
{
    NOODLES,
    TOMATOES,
    POTATOES,
    CREAM,
    ONIONS,
    GARLIC,
    CHEESE,
    PORK,
    BEEF,
    CHICKEN,
    BACON,
    DOUGH,
    ZUCCHINI,
    BROCCOLI, 
    VEGANPORK,  // ab hier: nur durch transform() erhaltbare Zutaten
    VEGANHACK,
    TOFU,
    VEGANBACON
}

/*
public struct Card
{
    public Card(CardType ct) //, bool goe)
    {
        type = ct;
        //good_or_evil = goe;
        currently_in_deck = true;
        can_be_drawn = true;  //TODO: redundant?? -> soll nur gespeichert werden, ob man sie ziehen kann oder nicht?
    }

    public CardType type;
    //public bool good_or_evil;
    public bool currently_in_deck;
    public bool can_be_drawn;
}
*/

public static class DeckHandler
{
    private static List<CardType> draw_pile;
    private static List<CardType> active_pile;
    private static List<CardType> discard_pile;

    public static int number_of_different_cards = 14;

    public static void generate_deck()
    {
        draw_pile = new List<CardType>();
        active_pile = new List<CardType>();
        discard_pile = new List<CardType>();

        // Karten-Typ-Begrenzungen (all ingredients of at least 1 dish have to be in the deck)
        int dish_select = UnityEngine.Random.Range(0, 5);
        switch(dish_select)
        {
            case 0:  // noodles with tomato-sauce
                draw_pile.Add(CardType.NOODLES);
                draw_pile.Add(CardType.TOMATOES);
                if (UnityEngine.Random.Range(0, 6) < 4)  // 2/3 chance to get onions
                {
                    draw_pile.Add(CardType.ONIONS);
                }
                if (UnityEngine.Random.Range(0, 6) < 2)  // 1/3 chance to get garlic
                {
                    draw_pile.Add(CardType.GARLIC);
                }
                break;
            case 1:  // potato gratin
                draw_pile.Add(CardType.POTATOES);
                draw_pile.Add(CardType.CHEESE);
                if (UnityEngine.Random.Range(0, 6) < 4)  // 2/3 chance to get cream
                {
                    draw_pile.Add(CardType.CREAM);
                }
                if (UnityEngine.Random.Range(0, 6) < 2)  // 1/3 chance to get broccoli
                {
                    draw_pile.Add(CardType.BROCCOLI);
                }
                break;
            case 2:  // tarte flambee
                draw_pile.Add(CardType.DOUGH);
                draw_pile.Add(CardType.ONIONS);
                draw_pile.Add(CardType.CREAM);
                if(UnityEngine.Random.Range(0, 6) < 2)  // 1/3 chance to get zucchini
                {
                    draw_pile.Add(CardType.ZUCCHINI);
                }
                break;
            case 3:  // potato gratin with beef
                draw_pile.Add(CardType.POTATOES);
                draw_pile.Add(CardType.CHEESE);
                draw_pile.Add(CardType.BEEF);
                break;
            case 4:  // spaghetti carbonara
                draw_pile.Add(CardType.NOODLES);
                draw_pile.Add(CardType.CREAM);
                draw_pile.Add(CardType.BACON);
                break;
        }

        // Generate 20 cards for drawing
        while (draw_pile.Count < 15)
        {
            CardType type = (CardType)UnityEngine.Random.Range(0, number_of_different_cards); // random number between 0 and number_of_different_cards-1
            draw_pile.Add(type);
        }

        Debug.Log("Deck: " + draw_pile.Count);
        Debug.Log(String.Join(", ", draw_pile));

        // Draw 5 hand cards
        for (int i = 0; i < 5; i++)
        {
            draw_card();
        }

        Debug.Log("Deck: " + draw_pile.Count);
        Debug.Log(String.Join(", ", active_pile));
        Debug.Log(String.Join(", ", draw_pile));
    }

    public static CardType get_card_from_actives(int idx)
    {
        if(active_pile == null)
        {
            throw new Exception("Deck wasn't generated yet!");
        }
        return active_pile[idx];
    }

    public static CardType draw_card()
    {
        if(draw_pile == null)
        {
            throw new Exception("Deck wasn't generated yet!");
        }

        int card_idx = UnityEngine.Random.Range(0, draw_pile.Count);

        CardType new_card = draw_pile[card_idx];
        active_pile.Add(new_card);
        draw_pile.Remove(new_card);

        return new_card;
    }

    public static void throw_card_in_trash(CardType old_card)
    {
        // Removing card from active cards
        active_pile.Remove(old_card);

        // 50% that old card will be removed from deck
        int rand = UnityEngine.Random.Range(0, 2);  // 0 or 1
        if(rand == 0)
        {
            draw_pile.Add(old_card);
        } else
        {
            discard_pile.Add(old_card);
        }
    }

    public static void throw_card_in_pot(CardType card)
    {
        // Removing card from active cards
        active_pile.Remove(card);
    }

    public static void put_ingredients_back_to_deck(List<CardType> ingredients)
    {
        // Removing card from active cards
        foreach (CardType card in ingredients)
        {
            draw_pile.Add(card);
        }
    }

    public static void expand_deck_with_cards()
    {
        Debug.Log("expand deck");
        int added_cards = 0;
        while (added_cards < 7)
        {
            CardType type = (CardType)UnityEngine.Random.Range(0, number_of_different_cards); // random number between 0 and number_of_different_cards
            draw_pile.Add(type);
            added_cards++;
        }
    }

    public static int get_amount_of_deck_cards()
    {
        if (draw_pile == null) return 0;
        return draw_pile.Count;
    }

    public static double get_efficiency_of_card(CardType card)
    {
        switch (card)
        {
            case CardType.NOODLES:
            case CardType.TOMATOES:
            case CardType.POTATOES:
            case CardType.CREAM:
            case CardType.ONIONS:
            case CardType.GARLIC:
            case CardType.CHEESE:
            case CardType.DOUGH:
            case CardType.ZUCCHINI:
            case CardType.BROCCOLI:
                return 1.0;
            case CardType.PORK:
            case CardType.BEEF:
            case CardType.CHICKEN:
            case CardType.BACON:
                return -1.0;
            case CardType.VEGANBACON:
            case CardType.VEGANHACK:
            case CardType.VEGANPORK:
            case CardType.TOFU:
                return 0.8;
            default:
                return 0.0;
        }
    }

    public static double detect_dish_quality(List<CardType> ingredients)
    {
        if(ingredients == null)
        {
            return 0;  // no dish
        } 

        if (ingredients.Count == 2)
        {
            bool pasta_with_tomatoe_sauce_easy = ingredients.Contains(CardType.NOODLES) && ingredients.Contains(CardType.TOMATOES);
            bool potato_gratin_easy = ingredients.Contains(CardType.POTATOES) && ingredients.Contains(CardType.CHEESE);

            if (pasta_with_tomatoe_sauce_easy || potato_gratin_easy)
            {
                return 1.1;  // 10% more effective
            }
        }
        else if(ingredients.Count == 3)
        {
            bool pasta_with_tomatoe_sauce_normal = ingredients.Contains(CardType.NOODLES) && ingredients.Contains(CardType.TOMATOES) && ingredients.Contains(CardType.ONIONS);
            bool potato_gratin = ingredients.Contains(CardType.POTATOES) && ingredients.Contains(CardType.CHEESE) && ingredients.Contains(CardType.CREAM);
            bool tart_flambee_easy = ingredients.Contains(CardType.DOUGH) && ingredients.Contains(CardType.ONIONS) && ingredients.Contains(CardType.CREAM);

            bool potato_gratin_easy_with_beef = ingredients.Contains(CardType.POTATOES) && ingredients.Contains(CardType.CHEESE) && ingredients.Contains(CardType.BEEF);
            bool potato_gratin_easy_with_veg_hack = ingredients.Contains(CardType.POTATOES) && ingredients.Contains(CardType.CHEESE) && ingredients.Contains(CardType.VEGANHACK);

            bool pasta_carbonara = ingredients.Contains(CardType.NOODLES) && ingredients.Contains(CardType.CREAM) && ingredients.Contains(CardType.BACON);
            bool pasta_carbonara_veg = ingredients.Contains(CardType.NOODLES) && ingredients.Contains(CardType.CREAM) && ingredients.Contains(CardType.VEGANBACON);

            if(pasta_with_tomatoe_sauce_normal || potato_gratin || tart_flambee_easy || pasta_carbonara_veg || potato_gratin_easy_with_veg_hack)
            {
                return 1.2;  // 20% more effective
            }
            if(pasta_carbonara || potato_gratin_easy_with_beef)
            {
                return -0.4;
            }
        } else if(ingredients.Count == 4)
        {
            bool pasta_with_tomatoe_sauce = ingredients.Contains(CardType.NOODLES) && ingredients.Contains(CardType.TOMATOES) && ingredients.Contains(CardType.ONIONS) && ingredients.Contains(CardType.GARLIC);
            bool potato_gratin_with_broccoli = ingredients.Contains(CardType.POTATOES) && ingredients.Contains(CardType.CHEESE) && ingredients.Contains(CardType.CREAM) && ingredients.Contains(CardType.BROCCOLI);
            bool tart_flambee = ingredients.Contains(CardType.DOUGH) && ingredients.Contains(CardType.ONIONS) && ingredients.Contains(CardType.CREAM) && ingredients.Contains(CardType.ZUCCHINI);

            if (pasta_with_tomatoe_sauce || potato_gratin_with_broccoli || tart_flambee)
            {
                return 1.3;  // 30% more effective
            }
        }

        return 0.8;  // 20% less effective
    }

    public static void reset_deck()
    {
        draw_pile = null;
        active_pile = null;
        discard_pile = null;
    }
}
