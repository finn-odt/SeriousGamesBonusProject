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
    BROCCOLI
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

    public static void generate_deck()
    {
        draw_pile = new List<CardType>();
        active_pile = new List<CardType>();
        discard_pile = new List<CardType>();

        int number_of_different_cards = 14;
        // TODO: Karten-Typ-Begrenzungen??

        // Generate 50 cards for drawing
        while (draw_pile.Count < 50)
        {
            CardType type = (CardType)UnityEngine.Random.Range(0, number_of_different_cards); // random number between 0 and number_of_different_cards
            draw_pile.Add(type);
        }

        // Draw 5 hand cards
        for(int i = 0; i < 5; i++)
        {
            CardType card = draw_card();
            active_pile.Add(card);
            draw_pile.Remove(card);
        }
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

        int card_idx = UnityEngine.Random.Range(0, draw_pile.Count + 1);

        return draw_pile[card_idx];
    }

    public static CardType throw_card_in_trash(CardType old_card)
    {
        // Removing card from active card
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

        // Drawing new card
        CardType new_card = draw_card();
        active_pile.Add(new_card);
        draw_pile.Remove(new_card);

        return new_card;
    }

    public static void cook_with_cards(List<CardType> cards)
    {

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
            default:
                return 0.0;
        }
    }
}
