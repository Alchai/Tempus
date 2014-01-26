using UnityEngine;
using System.Collections;

public class SciFiStateMachine : StatesInherit
{

    #region Variables
    private Player plr;
    #endregion

    // Use this for initialization
	void Start () {
        plr = GameObject.Find("Character2").GetComponent<Player>();
        ChangeState("Idle");
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if ((plr.IsMovingLeft || plr.IsMovingRight) && (!plr.IsBlocking && !plr.isDashing && !plr.IsAirborne && !plr.isJumping))
            ChangeState("Run");
        if ((plr.canJump && plr.JumpPressed))
            ChangeState("Jump");
        if ((plr.isDashing))
            ChangeState("Dash");
        if ((plr.isHit))
            ChangeState("Hit");
        else
            ChangeState("Idle");
	}
}
