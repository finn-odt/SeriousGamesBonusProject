using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodCardBehaviour : MonoBehaviour //, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private bool dragging;

    public GameObject card;
    public GameObject trash;
    public GameObject pot;

    public float big_scale, small_scale;

    private float initial_x, initial_y;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        dragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Change position, when dragging
        if(dragging)
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
        } else if(Input.GetMouseButtonUp(0) && dragging) {

            dragging = false;

            // Check for Collision with trash/cooking-pot
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Setze die Z-Koordinate auf 0

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

            bool reached_target = false;
            for(int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject == pot)
                {
                    reached_target = true;
                    // Throw in cooking pot
                    Debug.Log("in pot");
                }
                else if (hits[i].collider.gameObject == trash)
                {
                    reached_target = true;
                    // Throw in trash
                    Debug.Log("in trash");
                    generateNewCard();
                }
            }

            if(!reached_target)
            {
                // Back to origin
                Vector3 origin = new Vector3(initial_x, initial_y, 0);
                card.transform.position = origin;
            }

            // Change Scale (bigger/normal)
            Vector3 scaleChange = new Vector3(big_scale, big_scale, big_scale);
            card.transform.localScale = scaleChange;
        }
    }

    public void generateNewCard()
    {

    }
}
