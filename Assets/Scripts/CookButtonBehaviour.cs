using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookButtonBehaviour : MonoBehaviour
{
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!CookingHandler.are_there_ingredients())
        {
            button.interactable = false;
        } else
        {
            button.interactable = true;
        }
    }

    public void onButtonPressed()
    {
        CookingHandler.cook();
    }

    public void pointerEnteredButton()
    {
        if (button.interactable)
        {            
            Image buttonImage = button.GetComponent<Image>();

            if (buttonImage != null)
            {
                buttonImage.color = new Color(0.8f, 0.8f, 0.8f); //new Color(226, 255, 223);
            }
        }
    }

    public void pointerLeftButton()
    {
        button.GetComponent<Image>().color = new Color(255, 255, 255);
    }
}
