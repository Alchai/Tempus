//Written by Daniel Stover and Steven Belowsky

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Client : MonoBehaviour
{
    #region Variables

    public int mySID = 0, inputDelay = 4, myChar, hisChar, playerNum;
    private float fullPacketLength;
    private List<float> timesSent = new List<float>(), timesRecd = new List<float>();

    private GameObject char1, char2, char3, char4;
    public GameObject me, them;

    public string CharUserName;


    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameOver();
    }

    void Start()
    {
        CharUserName = SystemInfo.deviceName;
        Network.Connect("72.238.29.102", 843);
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;

        char1 = Resources.Load("Character1") as GameObject;
        char2 = Resources.Load("Character2") as GameObject;
        char3 = Resources.Load("Character3") as GameObject;
        char4 = Resources.Load("Character4") as GameObject;
    }

    void OnConnectedToServer()
    {
        StartCoroutine(speedTick());
    }

    private IEnumerator speedTick()
    {
        networkView.RPC("CalculateInputDelay", RPCMode.Server, Network.player);
        timesSent.Add(Time.time);

        for (int i = 0; i < 60; i++)
            yield return new WaitForEndOfFrame();

        StartCoroutine(speedTick());
    }

    [RPC]
    public void SendInput(int index, bool down_or_up, int seshID, bool p1_p2)
    {
        Player theirPlayer = them.GetComponent<Player>();
        print(them.name + " just received an input!");

        if (!down_or_up)
            switch (index)
            {
                case 0:
                    theirPlayer.LeftPressed = true;
                    break;
                case 3:
                    theirPlayer.RightPressed = true;
                    break;
                case 4:
                    theirPlayer.Jump();
                    break;
                case 5:
                    //Attack here
                    break;
                case 6:
                    theirPlayer.Dash();
                    break;
                case 7:
                    //ranged here
                    break;
                default:
                    break;
            }

        if (down_or_up)
            switch (index)
            {
                case 0:
                    them.GetComponent<Player>().LeftPressed = false;
                    break;
                case 3:
                    them.GetComponent<Player>().RightPressed = false;
                    break;
                default:
                    break;
            }

    }

    public void GameOver()
    {
        networkView.RPC("EndGame", RPCMode.Server, mySID);
    }

    [RPC]
    public void EndGame(int mysID)
    {

    }

    [RPC]
    public void GetSessionID(NetworkPlayer player, int sID, int opponentCharChoice, int myCharChoice, int whichPlayerAmI)
    {
        mySID = sID;
        myChar = myCharChoice;
        hisChar = opponentCharChoice;
        playerNum = whichPlayerAmI;

        Application.LoadLevel("CharacterSelect");
    }

    [RPC]
    public void CalculateInputDelay(NetworkPlayer player)
    {
        timesRecd.Add(Time.time);
        fullPacketLength = Mathf.Abs(timesSent[timesSent.Count - 1] - timesRecd[timesRecd.Count - 1]);
        inputDelay = Mathf.CeilToInt(fullPacketLength / (2f * .0167f));
    }

    [RPC]
    public void CreateCharacter(int whichChar, int whichPlayer, Vector3 pos, Vector3 rot, int SID)
    {
        GameObject newobj = char1;
        switch (whichChar)
        {
            case 1:
                newobj = GameObject.Instantiate(char1) as GameObject;
                newobj.transform.position = pos;
                newobj.transform.eulerAngles = rot;
                break;
            case 2:
                newobj = GameObject.Instantiate(char2) as GameObject;
                newobj.transform.position = pos;
                newobj.transform.eulerAngles = rot;
                break;
            case 3:
                newobj = GameObject.Instantiate(char3) as GameObject;
                newobj.transform.position = pos;
                newobj.transform.eulerAngles = rot;
                break;
            case 4:
                newobj = GameObject.Instantiate(char4) as GameObject;
                newobj.transform.position = pos;
                newobj.transform.eulerAngles = rot;
                break;
            default:
                Destroy(newobj);
                break;

        }

        if (whichPlayer == playerNum)
        {
            newobj.AddComponent<InputManager>();
            //newobj.AddComponent<Player>();
            newobj.name = "me";
            me = newobj;
        }
        else
        {
            newobj.name = "them";
           // newobj.AddComponent<Player>();
            them = newobj;
        }
    }

    [RPC]
    public void JoinLobby(NetworkPlayer player)
    {

    }
    private int numcharselects = 0;

    [RPC]
    public void SelectCharacter(string who, int seshID, bool p1_p2)
    {
        numcharselects++;
        GameObject mybutton, theirbutton;
        if (!p1_p2)
        {
            mybutton = GameObject.Find("charone");
            theirbutton = GameObject.Find("chartwo");
        }
        else
        {
            mybutton = GameObject.Find("chartwo");
            theirbutton = GameObject.Find("charone");
        }
        if (who.Contains("1") && theirbutton.GetComponent<ButtonSelect>().currentSelection != 1)
        {
            mybutton.transform.position = GameObject.Find("topleftplayer").transform.position;
            mybutton.GetComponent<ButtonSelect>().currentSelection = 1;

        }
        else if (who.Contains("2") && theirbutton.GetComponent<ButtonSelect>().currentSelection != 2)
        {
            mybutton.transform.position = GameObject.Find("toprightplayer").transform.position;
            mybutton.GetComponent<ButtonSelect>().currentSelection = 2;
        }
        else if (who.Contains("3") && theirbutton.GetComponent<ButtonSelect>().currentSelection != 3)
        {
            mybutton.transform.position = GameObject.Find("bottomleftplayer").transform.position;
            mybutton.GetComponent<ButtonSelect>().currentSelection = 3;
        }
        else if (who.Contains("4") && theirbutton.GetComponent<ButtonSelect>().currentSelection != 4)
        {
            mybutton.transform.position = GameObject.Find("bottomrightplayer").transform.position;
            mybutton.GetComponent<ButtonSelect>().currentSelection = 4;
        }
    }

    void OnGUI()
    {
        if (Network.isClient)
        {
            GUILayout.Label("Connected To Server!");
            if (mySID == 0)
            {
                GUILayout.Label("My session ID is: " + mySID);
                GUILayout.Label("I am player: " + playerNum);
                GUILayout.Label("Input delay: " + inputDelay);
                GUILayout.Label("Last Round Trip: " + fullPacketLength);
                GUILayout.Label("num sent char selects: " + numcharselects);
            }
            else
                GUILayout.Label("Awaiting opponent...");
        }
        else
            GUILayout.Label("Not connected to server");
    }
}
