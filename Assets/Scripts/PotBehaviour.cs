using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotBehaviour : MonoBehaviour
{
    public GameObject pot;

    private Sprite normal, burning;

    // Start is called before the first frame update
    void Start()
    {
        normal = Resources.Load<Sprite>("Images/pot_normal");
        burning = Resources.Load<Sprite>("Images/pot_with_more_fire");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Setze die Z-Koordinate auf 0

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        bool over_pot = false;
        bool has_card = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject == pot)
            {
                over_pot = true;
            }

            if (hits[i].collider.gameObject.tag == "card")
            {
                has_card = true;
            }
        }

        if (over_pot && has_card)
        {
            pot.GetComponent<SpriteRenderer>().sprite = burning;
        }
        else
        {
            pot.GetComponent<SpriteRenderer>().sprite = normal;
        }
    }
}