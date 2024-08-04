using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameButtons : MonoBehaviour
{
    const int NumOfButtons = 1;

    private Color HoveredColour = new Color(0.83921568627f, 0.85882352941f, 0.65490196078f, 1);
    private Color UnHoveredColour = new Color(0.36862745098f, 0.40392156862f, 0.47058823529f, 0.9f);

    public TMP_FontAsset HoveredFont;
    public TMP_FontAsset UnHoveredFont;

    public int CurrentButton;
    public TextMeshProUGUI[] Buttons = { null, null };

    void Awake()
    {
        CurrentButton = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //movement of current button based on keys
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            ButtonPress();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            CurrentButton++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            CurrentButton--;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Continue();
        }

        //makes the current button loop to the top/bottom button
        if(CurrentButton > NumOfButtons) { CurrentButton = 0; }
        if( CurrentButton < 0) { CurrentButton = NumOfButtons; }

        //set button colours
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].color = UnHoveredColour;
            Buttons[i].font = UnHoveredFont;
        }

        Buttons[CurrentButton].color = HoveredColour;
        Buttons[CurrentButton].font = HoveredFont;
    }

    void ButtonPress()
    {
        switch (CurrentButton)
        {
            case 0:
                //continue game
                Continue();
                break;

            case 1:
                //start from last checkpoint
                RestartFromCheckpoint();
                break;

            case 2:
                break;
        }
    }

    void Continue()
    {
        //set menu open to false and remove it
        GameManager.Instance.GameMenuOpen = false;
        Destroy(transform.parent.gameObject);
    }

    void RestartFromCheckpoint()
    {
        //find player object and kill it
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().StartCoroutine("KillPlayer");
        //continue game
        Continue();
    }
}
