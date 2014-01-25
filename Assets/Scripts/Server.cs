//Written by Steven Belowsky and Daniel Stover

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Server : MonoBehaviour
{
    #region Variables

    private const int maxPlayers = 1000;
    private const int port = 843;
    private int numPlayers = 0, numsessions = 0, mychar = 1;

    private string myIP = "";

    private List<NetworkPlayer> playersInLobby = new List<NetworkPlayer>();
    private List<Session> activeSessions = new List<Session>();
    private List<int> activeSessionIDs = new List<int>();

    #endregion

    #region UnityFunctions

    void Start()
    {
        Network.InitializeServer(maxPlayers, port, true);
        StartCoroutine(LobbyLoop());
    }

    void OnServerInitialized()
    {
        // this function is called when you initialize a server (basically right away) 
        // Co-routines are just functions that let you pause for time (like yield return new WaitForEndOfFrame()

        StartCoroutine(LobbyLoop());

        int random = Random.Range(0, Network.connections.Length);
        while (activeSessionIDs.Contains(random))
            random = Random.Range(0, Network.connections.Length);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        //whenever a player connects, add them to lobby, and update numplayers 
        numPlayers = Network.connections.Length;
        // playersInLobby.Add(player);
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        numPlayers = Network.connections.Length;
        if (playersInLobby.Contains(player))
            playersInLobby.Remove(player);

        //this is where important bug fix code will go when dc's fuck everything up
        //this is where important bug fix code will go when dc's fuck everything up
        //this is where important bug fix code will go when dc's fuck everything up
        //this is where important bug fix code will go when dc's fuck everything up
        //this is where important bug fix code will go when dc's fuck everything up
    }

    private IEnumerator LobbyLoop()
    {
        //this list shit will work for game jam for sure but not for n>100
        if (playersInLobby.Count >= 2)
        {
            //create new session class, store our players
            Session newsesh = new Session();
            newsesh.p1 = playersInLobby[0];
            newsesh.p2 = playersInLobby[1];

            //store a random int as the seshID
            int ranID = Random.Range(0, Network.connections.Length);
            while (activeSessionIDs.Contains(ranID))
                ranID = Random.Range(0, Network.connections.Length);
            newsesh.seshID = ranID;

            newsesh.p1char = 1;
            newsesh.p2char = 2;

            //add to activesessions (and session ID list list
            activeSessions.Add(newsesh);
            activeSessionIDs.Add(ranID);

            //remove players from lobby
            playersInLobby.Remove(newsesh.p1);
            playersInLobby.Remove(newsesh.p2);

            //send sesh ID's to the players
            networkView.RPC("GetSessionID", newsesh.p1, newsesh.p1, ranID, newsesh.p2char, newsesh.p1char, 1);
            networkView.RPC("GetSessionID", newsesh.p2, newsesh.p1, ranID, newsesh.p1char, newsesh.p2char, 2);
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        StartCoroutine(LobbyLoop());
    }

    #endregion

    #region RPCs

    [RPC]
    public void SendInput(int index, bool down_or_up, int seshID, bool p1_p2)
    {
        foreach (Session s in activeSessions)
            if (seshID.Equals(s.seshID))
            {
                if (!p1_p2)
                    networkView.RPC("SendInput", s.p1, index, down_or_up, seshID, !p1_p2);
                else
                    networkView.RPC("SendInput", s.p2, index, down_or_up, seshID, p1_p2);
            }
    }

    [RPC]
    public void GetSessionID(NetworkPlayer player, int sID, int opponentCharChoice, int myCharChoice, int whichPlayerAmI)
    {
    }

    [RPC]
    public void SelectCharacter(string who, int seshID, bool p1_p2)
    {
        foreach (Session s in activeSessions)
            if (seshID.Equals(s.seshID))
            {
                NetworkPlayer player;
                if (!p1_p2)
                    player = s.p1;
                else
                    player = s.p2;

                networkView.RPC("SelectCharacter", s.p1, who, seshID, p1_p2);
                networkView.RPC("SelectCharacter", s.p2, who, seshID, p1_p2);
            }

    }

    [RPC]
    public void CreateCharacter(int whichChar, int whichPlayer, Vector3 pos, Vector3 rot, int SID)
    {
        foreach (Session s in activeSessions)
        {
            if (SID.Equals(s.seshID))
            {
                networkView.RPC("CreateCharacter", s.p1, whichChar, whichPlayer, pos, rot, s.seshID);
                networkView.RPC("CreateCharacter", s.p2, whichChar, whichPlayer, pos, rot, s.seshID);
            }
        }
    }

    [RPC]
    public void SessionOver(int sID)
    {
        activeSessionIDs.Remove(sID);

        for (int i = 0; i < activeSessions.Count; i++)
            if (activeSessions[i].Equals(sID))
            {
                playersInLobby.Add(activeSessions[i].p1);
                playersInLobby.Add(activeSessions[i].p2);

                activeSessions.Remove(activeSessions[i]);
            }
    }

    [RPC]
    public void CalculateInputDelay(NetworkPlayer player)
    {
        networkView.RPC("CalculateInputDelay", player, player);
    }

    [RPC]
    public void JoinLobby(NetworkPlayer player)
    {
        playersInLobby.Add(player);
    }

    #endregion

    void OnGUI()
    {
        if (Network.isServer)
        {
            GUILayout.Label("My IP: " + Network.player.externalIP);
            GUILayout.Label("max players: " + maxPlayers);
            GUILayout.Label("players connected: " + Network.connections.Length.ToString());
            GUILayout.Label("players in lobby: " + playersInLobby.Count);
            GUILayout.Label("number of sessions: " + activeSessions.Count);

            if (activeSessions.Count > 0)
                foreach (Session s in activeSessions)
                {
                    GUILayout.Label("Session: " + s.seshID);
                    GUILayout.Label("Player 1: " + s.p1.externalIP);
                    GUILayout.Label("Player 2: " + s.p2.externalIP);
                }
        }
        else
            GUILayout.Label("Initializing server...");
    }
}

public class Session
{
    public NetworkPlayer p1, p2;
    public int p1char, p2char;
    public int seshID = 0;
}