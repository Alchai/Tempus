using UnityEngine;
using System.Collections;

public class OnMouseDownStuff : MonoBehaviour
{
    #region Variables

    private float Seconds = 15;
    public static int NumCharSelected = 0;
    public static bool ChoiceLock = false;
    public bool HasLocked = false;
    public static int currentSelection = 0;
    private bool joinedlobby = false;

    GameObject Timer;
    GameObject LockButton;
    private int numMenuItems = 4;
    private GameObject[] Characters;
    private Client client;
    private Color SelectedColor = Color.white;
    private Color MouseOverColor = new Color(0.75f, 0, 0);

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

        if (Application.loadedLevelName.Contains("Character"))
        {
            if (Seconds > 0)
            {
                Seconds -= Time.deltaTime;
                Timer.guiText.text = Seconds.ToString("f0");
            }
            if (Mathf.Round(Seconds) < 1)
            {
                Timer.guiText.text = Seconds.ToString("f0");
                Application.LoadLevel("TestBattle");
            }
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


    }

    void OnMouseDown()
    {
        if (!ChoiceLock)
        {
            if (tag == "Character 1")
            {
                if (currentSelection != 1)
                    Select("Character 1");
            }

            if (tag == "Character 2")
            {
                if (currentSelection != 2)
                    Select("Character 2");
            }

            if (tag == "Character 3")
            {
                if (currentSelection != 3)
                    Select("Character 3");
            }

            if (tag == "Character 4")
            {
                if (currentSelection != 4)
                    Select("Character 4");
            }
        }

        if (tag == "Play" && !joinedlobby && GameObject.Find("GUI").GetComponent<InputGUI>().CanPlay)
        {
            client.networkView.RPC("JoinLobby", RPCMode.Server, Network.player);
            joinedlobby = true;
        }

        else if (tag == "Credits")
            print("Credits Switch");

        if (tag == "Exit" || tag == "Exit Cube")
            Application.Quit();

        if (tag == "Lock Choices" && currentSelection > 0)
        {
            print("Locked");
            Select("Lock Choices");

        }
    }

    void OnMouseEnter()
    {
        Color newcol = new Color(.6f, .8f, .6f, 1f);
        Color RedCol = new Color(.8f, 0.1f, 0.1f, 1f);
        if (tag == "Play" && !GameObject.Find("GUI").GetComponent<InputGUI>().CanPlay)
        {
            this.renderer.material.color = RedCol;
        }
        else
        {
            this.renderer.material.color = newcol;
        }   

        if (tag == "Exit" || tag == "Credits" || tag == "Play")
            this.renderer.material.color = MouseOverColor;

    }
    void OnMouseExit()
    {
        if (!(this.tag == "Character " + currentSelection))
            renderer.material.color = Color.white;
    }
}
