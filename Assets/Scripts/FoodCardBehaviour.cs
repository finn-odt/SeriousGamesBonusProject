using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class FoodCardBehaviour : MonoBehaviour
{
    [SerializeField] private bool dragging;

    public GameObject card;
    public GameObject trash;
    public GameObject pot;

    public float big_scale, small_scale;

    private float initial_x, initial_y;

    private Vector3 offset;
    private bool new_card_animation;
    private float animation_offset;

    private int click_counter;
    private float last_click;

    public CardType card_type;

    // Start is called before the first frame update
    void Start()
    {
        dragging = false;
        click_counter = 0;
        new_card_animation = false;

        // Look up card type
        int id = Int32.Parse(card.name.Remove(0, 8)) - 1;  // Remove "FoodCard" from name to just have the id (1-5) -> 0 - 4
        try
        {
            card_type = DeckHandler.get_card_from_actives(id);
        }
        catch
        {
            DeckHandler.generate_deck();
            card_type = DeckHandler.get_card_from_actives(id);
        }

        // Change sprites according to card
        load_sprites();
    }

    // Update is called once per frame
    void Update()
    {
        if(new_card_animation)
        {
            Vector3 origin = new Vector3(initial_x, initial_y - animation_offset, 0);
            card.transform.position = origin;

            animation_offset -= 0.005f;
            if(animation_offset <= 0f)
            {
                new_card_animation = false;
            }

            return;
        }

        // Check for double click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Setze die Z-Koordinate auf 0
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == card)
            {
                if (click_counter == 0)
                {
                    click_counter++;
                    last_click = Time.time;
                }
                else
                {
                    if (click_counter == 1 && Time.time - last_click < 0.5f)  // double click, when clicking for the second time in 500ms
                    {
                        transform_card();
                    }
                    else
                    {
                        click_counter = 0;
                    }
                }
            } else
            {
                click_counter = 0;
            }
        }

        // Change position, when dragging
        if (dragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            card.transform.position = mousePosition;
        }

        // Check for dragging-constraint
        if (Input.GetMouseButtonDown(0) && !dragging)
        {
            // save initial coordinates
            initial_x = card.transform.position.x;
            initial_y = card.transform.position.y;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Setze die Z-Koordinate auf 0

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == card)
            {
                dragging = true;

                // Change Scale (smaller)
                Vector3 scaleChange = new Vector3(small_scale, small_scale, small_scale);
                card.transform.localScale = scaleChange;
            }
        }
        else if (Input.GetMouseButtonUp(0) && dragging)
        {

            dragging = false;

            // Check for Collision with trash/cooking-pot
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Setze die Z-Koordinate auf 0

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

            bool reached_target = false;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject == pot)
                {
                    reached_target = true;
                    // Throw in cooking pot
                    CookingHandler.add_ingredient(card_type);
                    card.SetActive(false);
                    // TODO: add ingredient to ingredients_list (maybe in CookingHandler??)
                }
                else if (hits[i].collider.gameObject == trash)
                {
                    reached_target = true;
                    // Throw in trash
                    card_type = DeckHandler.throw_card_in_trash(card_type);
                    load_sprites();
                    scale_big();
                    // Back to below origin
                    Vector3 origin = new Vector3(initial_x, initial_y - 2.7f, 0);
                    card.transform.position = origin;
                    animation_offset = 2.6f;
                    new_card_animation = true;
                }
            }

            if (!reached_target)
            {
                // Back to origin
                Vector3 origin = new Vector3(initial_x, initial_y, 0);
                card.transform.position = origin;
                scale_big();
            }
        }
    }

    private void scale_big()
    {
        // Change Scale (bigger/normal)
        Vector3 scaleChange = new Vector3(big_scale, big_scale, big_scale);
        card.transform.localScale = scaleChange;
    }

    public void transform_card()
    {
        if (card_type == CardType.CHICKEN)
        {
            card_type = CardType.TOFU;
            load_sprites();
        } else if (card_type == CardType.BACON)
        {
            card_type = CardType.VEGANBACON;
            load_sprites();
        }
        else if (card_type == CardType.BEEF)
        {
            card_type = CardType.VEGANHACK;
            load_sprites();
        }
        else if (card_type == CardType.PORK)
        {
            card_type = CardType.VEGANPORK;
            load_sprites();
        }
    }

    private void load_sprites()
    {
        GameObject textChild = transform.Find("FoodText").gameObject;
        GameObject iconChild = transform.Find("FoodIcon").gameObject;

        Sprite text_sprite = Resources.Load<Sprite>("Images/cards/text/" + card_type.ToString().ToLower());
        textChild.GetComponent<SpriteRenderer>().sprite = text_sprite;

        Sprite icon_sprite = Resources.Load<Sprite>("Images/cards/food/" + card_type.ToString().ToLower());
        iconChild.GetComponent<SpriteRenderer>().sprite = icon_sprite;
    }
}