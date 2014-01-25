using UnityEngine;
using System.Collections;

public class BattleLogic : MonoBehaviour
{
    private Client client;

    void Start()
    {
        client = GameObject.Find("Client").GetComponent<Client>();
        StartCoroutine("SendNewChars");
    }


    private IEnumerator SendNewChars()
    {
        //wait 5 seconds before sending the create character RPC (so that both players are definitely connected)
        yield return new WaitForSeconds(5f);
        client.networkView.RPC("CreateCharacter", RPCMode.Server, client.myChar, client.playerNum, Vector3.zero, Vector3.zero, client.mySID);
    }
}
