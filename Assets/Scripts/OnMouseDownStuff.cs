using UnityEngine;
using System.Collections;

public class OnMouseDownStuff : MonoBehaviour
{
    #region Variables

    public float Seconds = 59;
    public static int NumCharSelected = 0;
    public static bool ChoiceLock = false;
    public bool HasLocked = false;
    public static int currentSelection = 0;

    GameObject Timer;
    GameObject LockButton;
    private int numMenuItems = 4;
    private GameObject[] Characters;
    private Client client;
    private Color SelectedColor = new Color(.6f, .8f, .6f, 1f);

    #endregion
    void Start()
    {
        client = GameObject.Find("Client").GetComponent<Client>();
        Characters = new GameObject[numMenuItems];
        for (int i = 0; i < Characters.Length; i++)
        {
            Characters[i] = GameObject.FindGameObjectWithTag("Character " + (i + 1));
        }
        renderer.material.color = Color.gray;
        Timer = GameObject.Find("Timer Text");
        LockButton = GameObject.Find("Lock Choices");
    }
    void Update()
    {
        if (joinedlobby && !Network.isClient)
            joinedlobby = false;
        //Timer Countdown
        if (Application.loadedLevelName.Contains("Character"))
        {
            if (ChoiceLock == true && Seconds > 0)
            {
                if (HasLocked == false && currentSelection > 0 && NumCharSelected > 1)
                {
                    Seconds = 5;
                    HasLocked = true;
                }
                Seconds -= Time.deltaTime;
                Timer.guiText.text = Seconds.ToString("f0");
            }
            else if (Seconds > 0)
            {
                Seconds -= Time.deltaTime;
                Timer.guiText.text = Seconds.ToString("f0"); 
            }
            if (Mathf.Round(Seconds) < 0)
            {
                Timer.guiText.text = Seconds.ToString("f0");
                Application.LoadLevel("TestBattle");
            }
        }
    }



    private void Deselect(string who)
    {
        //de-select all others
        //change colors accordingly
        //change public currentSelection int to 0 you just chose

        if (ChoiceLock == false && currentSelection != 0)
        {
            Characters[currentSelection - 1].renderer.material.color = Color.gray;
            print("deselected");
            currentSelection = 0;
            NumCharSelected--;
        }
    }

    private void Select(string who)
    {
        //de-select all others
        //change colors accordingly
        //change public currentSelection int to whichever you just chose
        bool p1_p2 = false;
        if (client.playerNum == 2)
            p1_p2 = true;
                                                
        client.networkView.RPC("SelectCharacter", RPCMode.Server, who, client.mySID, p1_p2);

        if (who == "Character 1" && ChoiceLock == false)
        {
            currentSelection = 1;
            NumCharSelected++;
        }
        else if (who == "Character 2")
        {
            currentSelection = 2;
            NumCharSelected++;
        }
        else if (who == "Character 3")
        {
            currentSelection = 3;
            NumCharSelected++;
        }
        else if (who == "Character 4")
        {
            currentSelection = 4;
            NumCharSelected++;
        }
        else if (who == "Lock Choices")
        {
            if (ChoiceLock == false)
            {
                ChoiceLock = true;
            }
            else
                ChoiceLock = false;
        }

        //Changes color for selected Character
        for (int i = 0; i < Characters.Length; i++)
        {
            if (i + 1 == currentSelection)
                Characters[i].renderer.material.color = SelectedColor;
            else
                Characters[i].renderer.material.color = Color.gray;
        }


    }
    private bool joinedlobby = false;
    void OnMouseDown()
    {
        if (!ChoiceLock)
        {
            if (tag == "Character 1")
            {
                if (currentSelection == 1)
                    Deselect("Character 1");
                else
                    Select("Character 1");
            }

            if (tag == "Character 2")
            {
                if (currentSelection == 2)
                    Deselect("Character 2");
                else
                    Select("Character 2");
            }

            if (tag == "Character 3")
            {
                if (currentSelection == 3)
                    Deselect("Character 3");
                else
                    Select("Character 3");
            }

            if (tag == "Character 4")
            {
                if (currentSelection == 4)
                    Deselect("Character 4");
                else
                    Select("Character 4");
            }
        }


        print(currentSelection);

        //Main Menu
        if (tag == "Play")
        {
            client.networkView.RPC("JoinLobby", RPCMode.Server, Network.player);
            joinedlobby = true;
        }

        else if (tag == "Credits")
            print("Credits Switch");

        if (tag == "Exit" || tag == "Exit Cube")
            Application.Quit();

        //Lock Choices
        if (tag == "Lock Choices" && currentSelection > 0)
        {
            print("Locked");
            Select("Lock Choices");

        }



        //THIS STUFF WILL BE MOVED TO THE SELECTION FUNCTIONS
        //THIS STUFF WILL BE MOVED TO THE SELECTION FUNCTIONS
        //THIS STUFF WILL BE MOVED TO THE SELECTION FUNCTIONS

        ////HighLight Selected button
        //Color SelectedColor = new Color(.6f, .8f, .6f, 1f);

        //if (CharacterSelected1 && tag == "Character 1")
        //    renderer.material.color = SelectedColor;

        //if (CharacterSelected2 && tag == "Character 2")
        //    renderer.material.color = SelectedColor;

        //if (CharacterSelected3 && tag == "Character 3")
        //    renderer.material.color = SelectedColor;

        //if (CharacterSelected4 && tag == "Character 4")
        //    renderer.material.color = SelectedColor;

        //if (ChoiceLock && tag == "Lock Choices")
        //    renderer.material.color = SelectedColor;
    }

    void OnMouseEnter()
    {
        Color newcol = new Color(.6f, .8f, .6f, 1f);
        this.renderer.material.color = newcol;
    }

    void OnMouseExit()
    {
        if (!(this.tag == "Character " + currentSelection))
            renderer.material.color = Color.gray;

        if (tag == "Lock Choices")
        {
            if (ChoiceLock == true)
                LockButton.renderer.material.color = SelectedColor;
        }
    }
}
