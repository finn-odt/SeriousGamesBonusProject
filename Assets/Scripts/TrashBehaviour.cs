using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBehaviour : MonoBehaviour
{
    public GameObject trash;

    private Sprite closed, open;

    // Start is called before the first frame update
    void Start()
    {
        open = Resources.Load<Sprite>("Images/trash_can_open");
        closed = Resources.Load<Sprite>("Images/trash_can_closed");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Setze die Z-Koordinate auf 0

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        bool over_trash = false;
        bool has_card = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject == trash)
            {
                over_trash = true;
            }

            if (hits[i].collider.gameObject.tag == "card")
            {
                has_card = true;
            }
        }

        if (over_trash && has_card)
        {
            trash.GetComponent<SpriteRenderer>().sprite = open;
        } else
        {
            trash.GetComponent<SpriteRenderer>().sprite = closed;
        }
    }
}
