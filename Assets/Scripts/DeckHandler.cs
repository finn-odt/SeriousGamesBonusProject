using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    BROKKOLI
}

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

public static class DeckHandler
{
    public static List<Card> deck_of_cards;

    public static void generate_deck()
    {
        deck_of_cards = new List<Card>();

        int number_of_different_cards = 14;

        for (int i = 0; i < 50; i++)
        {
            CardType type = (CardType)Random.Range(0, number_of_different_cards + 1); // random number between 0 and number_of_different_cards
            Card card = new Card(type);
            deck_of_cards.Add(card);
        }
    }
}
