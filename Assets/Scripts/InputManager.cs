using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    #region Variables

    private GameObject client, player;
    private bool leftDown, rightDown, downDown, upDown, jumpDown, attackDown, defendDown;
    private bool controllersConnected;
    private bool leftWasDown, rightWasDown, downWasDown, upWasDown, lTriggerWasDown, rTriggerWasDown;
    public enum COMMAND { LEFT, DOWN, UP, RIGHT, JUMP, ATTACK, DASH, RANGED, INTERACT };
    public bool isMyPlayer = false;

    #endregion

    void Start()
    {
        player = GameObject.Find("me");
        client = GameObject.Find("Client");

        //Check for controllers once at startup.
        if (Input.GetJoystickNames().GetLength(0) > 0)
            controllersConnected = true;
        else
            controllersConnected = false;

        leftWasDown = false;
        rightWasDown = false;
        downWasDown = false;
        upWasDown = false;
    }

    private IEnumerator delayInput(COMMAND cmdToSend, bool down_up)
    {
        //this waits for 4 frames or so
        for (int i = 0; i < client.GetComponent<Client>().inputDelay; i++)
            yield return new WaitForEndOfFrame();

        ProcessInput(cmdToSend, down_up);
    }
    // [RPC]
    //public void SendInput(int index, bool down_or_up, int seshID, bool p1_p2)
    //{
    void SendEvents(COMMAND cmdToSend, bool down_up)
    {
        bool p1_p2 = false;
        if (client.GetComponent<Client>().playerNum == 2)
            p1_p2 = true;

        client.networkView.RPC("SendInput", RPCMode.Server, (int)cmdToSend, down_up, client.GetComponent<Client>().mySID, !p1_p2);

        StartCoroutine(delayInput(cmdToSend, down_up));
    }

    public void ProcessInput(COMMAND cmdToSend, bool down_up)
    {
        Player playerScript = player.GetComponent<Player>();

        switch (cmdToSend)
        {
            case COMMAND.LEFT:
                if (!down_up)
                    playerScript.LeftPressed = true;
                else
                    playerScript.LeftPressed = false;
                break;
            case COMMAND.RIGHT:
                if (!down_up)
                    playerScript.RightPressed = true;
                else
                    playerScript.RightPressed = false;
                break;

            case COMMAND.JUMP:
                playerScript.Jump();
                break;
            case COMMAND.ATTACK:
                {

                }
                break;
            case COMMAND.DASH:
                {

                }
                break;
            case COMMAND.RANGED:
                {

                }
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            SendEvents(COMMAND.LEFT, false);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            SendEvents(COMMAND.DOWN, false);
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            SendEvents(COMMAND.UP, false);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            SendEvents(COMMAND.RIGHT, false);
        if (Input.GetKeyDown(KeyCode.Space))
            SendEvents(COMMAND.JUMP, false);
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J))
            SendEvents(COMMAND.ATTACK, false);
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.K))
            SendEvents(COMMAND.RANGED, false);
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            SendEvents(COMMAND.DASH, false);

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            SendEvents(COMMAND.LEFT, true);
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            SendEvents(COMMAND.DOWN, true);
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            SendEvents(COMMAND.UP, true);
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            SendEvents(COMMAND.RIGHT, true);
        if (Input.GetKeyUp(KeyCode.Space))
            SendEvents(COMMAND.JUMP, true);
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.J))
            SendEvents(COMMAND.ATTACK, true);
        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.K))
            SendEvents(COMMAND.RANGED, true);
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            SendEvents(COMMAND.DASH, true);

        //Check controller input
        if (controllersConnected)
        {
            if (Input.GetAxisRaw("Horizontal") <= -0.1f)
            {
                SendEvents(COMMAND.LEFT, false);
                leftWasDown = true;
            }
            else if (leftWasDown)
            {
                SendEvents(COMMAND.LEFT, true);
                leftWasDown = false;
            }
            if (Input.GetAxisRaw("Vertical") <= -0.1f)
            {
                SendEvents(COMMAND.DOWN, false);
                downWasDown = true;
            }
            else if (downWasDown)
            {
                SendEvents(COMMAND.DOWN, true);
                downWasDown = false;
            }
            if (Input.GetAxisRaw("Vertical") >= 0.1f)
            {
                SendEvents(COMMAND.UP, false);
                upWasDown = true;
            }
            else if (upWasDown)
            {
                SendEvents(COMMAND.UP, true);
                upWasDown = true;
            }

            if (Input.GetAxisRaw("Horizontal") >= 0.1f)
            {
                SendEvents(COMMAND.RIGHT, false);
                rightWasDown = true;
            }
            else if (rightWasDown)
            {
                SendEvents(COMMAND.RIGHT, true);
                rightWasDown = false;
            }

            if (Input.GetButtonDown("Jump"))
                SendEvents(COMMAND.JUMP, false);
            if (Input.GetButtonDown("Fire1"))
                SendEvents(COMMAND.ATTACK, false);
            if (Input.GetButtonDown("Fire2"))
                SendEvents(COMMAND.ATTACK, false);
            if (Input.GetButtonDown("Fire3"))
                SendEvents(COMMAND.RANGED, false);
            //LTrigger
            if (Input.GetAxisRaw("Axis9") > 0.0f)
            {
                SendEvents(COMMAND.DASH, false);
                lTriggerWasDown = true;
            }
            else if (lTriggerWasDown)
            {
                SendEvents(COMMAND.DASH, true);
                lTriggerWasDown = false;
            }
            //RTrigger
            if (Input.GetAxisRaw("Axis10") > 0.0f)
            {
                SendEvents(COMMAND.DASH, false);
            }
            else if (rTriggerWasDown)
            {
                SendEvents(COMMAND.DASH, true);
                rTriggerWasDown = false;
            }

            if (Input.GetButtonUp("Jump"))
                SendEvents(COMMAND.JUMP, true);
            if (Input.GetButtonUp("Fire1"))
                SendEvents(COMMAND.ATTACK, true);
            if (Input.GetButtonUp("Fire2"))
                SendEvents(COMMAND.ATTACK, true);
            if (Input.GetButtonUp("Fire3"))
                SendEvents(COMMAND.RANGED, true);
        }
    }

}
