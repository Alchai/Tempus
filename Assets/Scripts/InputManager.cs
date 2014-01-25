using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    #region Variables

    private GameObject client, player;
    private bool leftDown, rightDown, downDown, upDown, jumpDown, attackDown, defendDown;
    private bool controllersConnected;
    private bool leftWasDown, rightWasDown, downWasDown, upWasDown;
    public enum COMMAND { LEFT, DOWN, UP, RIGHT, JUMP, ATTACK, DEFEND, INTERACT };
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

        leftWasDown = rightWasDown = downWasDown = upWasDown = false;
    }

    private IEnumerator delayInput(COMMAND cmdToSend, bool down_up)
    {
        //this waits for 4 frames or so
        for (int i = 0; i < client.GetComponent<Client>().inputDelay; i++)
            yield return new WaitForEndOfFrame();

        ProcessInput(cmdToSend, down_up);
    }

    void SendEvents(COMMAND cmdToSend, bool down_up)
    {
        bool p1_p2;
      
        if (client.GetComponent<Client>().playerNum == 1)
            p1_p2 = true;
        else
            p1_p2 = false;

        client.networkView.RPC("SendInput", RPCMode.Server, (int)cmdToSend, down_up,
            client.GetComponent<Client>().mySID, p1_p2);

        StartCoroutine(delayInput(cmdToSend, down_up));

    }

    public void ProcessInput(COMMAND cmdToSend, bool down_up)
    {
        print("processed input: " + (int)cmdToSend);
        Player playerScript = player.GetComponent<Player>();

        switch (cmdToSend)
        {
            case COMMAND.ATTACK:

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
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SendEvents(COMMAND.LEFT, false);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            SendEvents(COMMAND.DOWN, false);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            SendEvents(COMMAND.UP, false);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            SendEvents(COMMAND.RIGHT, false);
        if (Input.GetKeyDown(KeyCode.Space))
            SendEvents(COMMAND.JUMP, false);
        if (Input.GetKeyDown(KeyCode.Z))
            SendEvents(COMMAND.ATTACK, false);
        if (Input.GetKeyDown(KeyCode.X))
            SendEvents(COMMAND.DEFEND, false);

        if (Input.GetKeyUp(KeyCode.LeftArrow))
            SendEvents(COMMAND.LEFT, true);
        if (Input.GetKeyUp(KeyCode.DownArrow))
            SendEvents(COMMAND.DOWN, true);
        if (Input.GetKeyUp(KeyCode.UpArrow))
            SendEvents(COMMAND.UP, true);
        if (Input.GetKeyUp(KeyCode.RightArrow))
            SendEvents(COMMAND.RIGHT, true);
        if (Input.GetKeyUp(KeyCode.Space))
            SendEvents(COMMAND.JUMP, true);
        if (Input.GetKeyUp(KeyCode.Z))
            SendEvents(COMMAND.ATTACK, true);
        if (Input.GetKeyUp(KeyCode.X))
            SendEvents(COMMAND.DEFEND, true);

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

            if (Input.GetButtonUp("Jump"))
                SendEvents(COMMAND.JUMP, true);
            if (Input.GetButtonUp("Fire1"))
                SendEvents(COMMAND.ATTACK, true);

        }
    }

}
