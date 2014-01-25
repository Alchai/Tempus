using UnityEngine;
using System.Collections;

public class AnimationTest : StatesInherit
{
    void Start()
    {
        ChangeState("Idle");
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ChangeState("Idle");

        else if (Input.GetKeyDown(KeyCode.W))
            ChangeState("Walk");

        else if (Input.GetKeyDown(KeyCode.E))
            ChangeState("Run");

        else if (Input.GetKeyDown(KeyCode.R))
            ChangeState("ArmScratch");
    }
}
