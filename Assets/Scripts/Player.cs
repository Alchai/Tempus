using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    #region Variables

    public const float AttacksToKill = 15.0f, MaxLevelState = 10.0f * AttacksToKill,
        RangeBlockAmount = 0.35f, LightMBlockAmount = 0.45f, HeavyMBlockAmount = 0.65f;

    private float sWinBackground, sJumpForce, sJumpHeight,
       sMoveSpeed, runSpeed = .2f, fallspeed = .2f, gravityBoost = 0f;

    public float RangeAttackScale = 1.0f, MeleeLightAttackScale = 1.0f, MeleeStrongAttackScale = 1.0f;
    private int BaseAttack, BaseDefense, jumpFrames = 25;

    public bool IsBlocking, IsMovingLeft, IsMovingRight, IsAirborne, canRight = true, canLeft = true,
        applyGravity = true, canJump = false, isJumping = false, facingLeft = false, isHit = false;

    public int FramesForKnockback = 10;

    //CHANGE THESE VARIABLES WHEN THE BUTTONS ARE\ARENT PRESSED 
    public bool LeftPressed = false, RightPressed = false, JumpPressed = false, isDashing = false, isAttacking = false;

    private GameObject head, feet;
    private RaycastHit[] ray;

    public Client client;

    public float dashSpeed = .5f;
    public int dashFrames = 10, FramsForKnockback = 10;

    private StatesInherit SInherit;

    #endregion

    void Start()
    {
        feet = transform.FindChild("feet").gameObject;
        head = transform.FindChild("head").gameObject;

        if (transform.eulerAngles.y == 270 || transform.eulerAngles.y == -90)
            facingLeft = true;
        else
            facingLeft = false;
    }

    void Update()
    {
        RaycastHit[] ray = Physics.RaycastAll(feet.transform.position, Vector3.down, (fallspeed + gravityBoost) * 1.5f);
        applyGravity = true;

        for (int i = 0; i < ray.Length; i++)
        {
            if (ray[i].transform.tag == "Wall")
            {
                applyGravity = false;
                canJump = true;
            }
        }
        if (applyGravity)
        {
            transform.Translate(0f, -fallspeed - gravityBoost, 0f);
            gravityBoost += .005f;
            canJump = false;
        }
        else
            gravityBoost = -.1f;

        canLeft = true;
        canRight = true;

        ray = Physics.RaycastAll(head.transform.position, Vector3.right, runSpeed * 3f);

        for (int i = 0; i < ray.Length; i++)
            if (ray[i].transform.tag == "Wall")
                canRight = false;

        ray = Physics.RaycastAll(head.transform.position, Vector3.left, runSpeed * 3f);

        for (int i = 0; i < ray.Length; i++)
            if (ray[i].transform.tag == "Wall")
                canLeft = false;

        ray = Physics.RaycastAll(feet.transform.position, Vector3.right, runSpeed * 3f);

        for (int i = 0; i < ray.Length; i++)
            if (ray[i].transform.tag == "Wall")
                canRight = false;

        ray = Physics.RaycastAll(feet.transform.position, Vector3.left, runSpeed * 3f);

        for (int i = 0; i < ray.Length; i++)
            if (ray[i].transform.tag == "Wall")
                canLeft = false;

        if (!isDashing && !isAttacking)
        {
            if (canLeft && LeftPressed)
            {
                transform.Translate(new Vector3(runSpeed, 0f, 0f), Space.World);
                transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0f, 90f, 0f), 45f);
            }
            if (canRight && RightPressed)
            {
                transform.Translate(new Vector3(-runSpeed, 0f, 0f), Space.World);
                transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0f, 270f, 0f), 45f);
            }
        }
    }

    public void Dash()
    {
        if (!isDashing)
        {
            StopCoroutine("dash");
            StartCoroutine("dash");
        }
    }

    private IEnumerator dash()
    {
        isDashing = true;
        if (Vector3.Distance(transform.eulerAngles, new Vector3(0f, 90f, 0f)) < 150f)
            facingLeft = false;
        else
            facingLeft = true;

        if ((facingLeft && dashSpeed > 0f) || (!facingLeft && dashSpeed < 0f))
            dashSpeed *= -1f;

        for (int i = 0; i < dashFrames; i++)
        {
            if ((float)dashFrames / 2f <= i)
            {
                isDashing = false;
                print("isdashing is false now");
            }
            transform.Translate(new Vector3(dashSpeed, 0f, 0f), Space.World);

            yield return new WaitForEndOfFrame();
        }
    }

    public void KnockBack()
    {
        StartCoroutine("knockBack");
    }

    private IEnumerator knockBack()
    {
        for (int i = 0; i < FramesForKnockback; i++)
        {
            if (facingLeft)
                gameObject.transform.Translate(new Vector3(-0.1f, 0f, 0f), Space.World);
            else
                gameObject.transform.Translate(new Vector3(0.1f, 0f, 0f), Space.World);

            yield return new WaitForEndOfFrame();
        }
    }

    void Attack_LightMelee(Player Them)
    {
        //Play Aminmation
        //Calc Attack amount
        float AttackAmount = this.BaseAttack * this.MeleeLightAttackScale;

        if (Them.IsBlocking)
            AttackAmount *= LightMBlockAmount;

        Them.sWinBackground -= AttackAmount;
        if (this.sWinBackground + AttackAmount > MaxLevelState)
            this.sWinBackground = MaxLevelState;
        else if (this.sWinBackground + AttackAmount < MaxLevelState)
            this.sWinBackground += AttackAmount;

    }

    void Attack_HeaveyMelee(Player Them)
    {
        //Play Aminmation
        //Calc Attack amount
        float AttackAmount = this.BaseAttack * this.MeleeStrongAttackScale;

        if (Them.IsBlocking)
            AttackAmount *= HeavyMBlockAmount;

        Them.sWinBackground -= AttackAmount;
        if (this.sWinBackground + AttackAmount > MaxLevelState)
            this.sWinBackground = MaxLevelState;
        else if (this.sWinBackground + AttackAmount < MaxLevelState)
            this.sWinBackground += AttackAmount;
    }

    void Attack_Ranged(Player Them)
    {
        //Play Aminmation
        //Calc Attack amount
        float AttackAmount = this.BaseAttack * this.RangeAttackScale;

        if (Them.IsBlocking)
            AttackAmount *= RangeBlockAmount;

        Them.sWinBackground -= AttackAmount;
        if (this.sWinBackground + AttackAmount > MaxLevelState)
            this.sWinBackground = MaxLevelState;
        else if (this.sWinBackground + AttackAmount < MaxLevelState)
            this.sWinBackground += AttackAmount;
    }

    public void SetBlocking(bool IsActive)
    {
        //Set Blocking Anim if true
        // else set idle
        this.IsBlocking = IsActive;
    }

    bool GetIsBlocking()
    {
        return this.IsBlocking;
    }

    public void Jump()
    {
        if (canJump)
            StartCoroutine("jump");
    }

    private IEnumerator jump()
    {
        if (!isAttacking)
        {
            isJumping = true;
            int counter = 0;
            float boost = .15f;

            while (counter < jumpFrames)
            {
                transform.Translate(0f, fallspeed * 1.4f + boost, 0f);
                boost -= .01f;
                yield return new WaitForEndOfFrame();
                counter++;
            }
            isJumping = false;
        }
    }
}
