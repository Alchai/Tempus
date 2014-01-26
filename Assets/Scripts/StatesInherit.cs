using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//this class is probably not needed
[Serializable]
public class State
{
    public string stateName = "";
    public StateAnimation[] Animations;
}

//every animation gets this class, and has attributes for the blending
[Serializable]
public class StateAnimation
{
    public string animationName = "";
    public int InFrames = 20, OutFrames = 10, Rate = 20;
    public string stateAfterOneShot = "";
}

//this is the class that we will be inheriting from on our animated models
//the basic idea is that you setup your values in the inspector (blendrates, etc), and then 
//whenever you want to change the state you just call "ChangeState". So, if my character was running, and then i 
//inputted an attack, it would change the animation state from running to x attack

public class StatesInherit : MonoBehaviour
{
    #region Variables

    [SerializeField]
    public StateAnimation[] StateAnimations;
    public StateAnimation currentState, previousState;
    public string startStateName = "";
    protected bool smoothBlending = false;
    public bool isFrozen = false;

    protected StateAnimation GetState(string stateName)
    {
        foreach (StateAnimation state in StateAnimations)
            if (state.animationName == stateName)
                return state;
        return null;
    }

    #endregion

    IEnumerator SmoothBlend(string stateName)
    {
        smoothBlending = true;
        int stateIndex = -1;

        for (int i = 0; i < StateAnimations.Length; i++)
            if (StateAnimations[i].animationName == stateName)
                stateIndex = i;

        previousState = currentState;
        currentState = StateAnimations[stateIndex];

        AnimationState previousStateAnim = animation[previousState.animationName];
        AnimationState currentStateAnim = animation[currentState.animationName];

        float blendTime = 1.0f;

        int OutFrames = previousState.OutFrames;
        int InFrames = currentState.InFrames;

        float blendOutRate = blendTime / OutFrames;
        float blendInRate = blendTime / InFrames;

        while (previousStateAnim.weight > 0.0f && currentStateAnim.weight < 1.0f)
        {
            while (isFrozen)
                yield return new WaitForEndOfFrame();

            previousStateAnim.weight -= blendOutRate;
            currentStateAnim.weight += blendInRate;

            yield return new WaitForEndOfFrame();
        }

        currentStateAnim.weight = 1.0f;
        previousStateAnim.weight = 0.0f;
        previousStateAnim.normalizedTime = 0.0f;
        previousStateAnim.enabled = false;
        smoothBlending = false;
    }

    public virtual void ChangeState(string stateName)
    {
        if (stateName != currentState.animationName /*&& currentState.animationName != "Death"*/)
        {
            if (smoothBlending)
            {
                StopCoroutine("SmoothBlend");
                AnimationState current = animation[currentState.animationName];
                AnimationState previous = animation[previousState.animationName];
                current.weight = 1.0f;
                current.enabled = true;
                previous.weight = 0.0f;
                previous.normalizedTime = 0.0f;
                previous.enabled = false;
                smoothBlending = false;
            }
            StartCoroutine("SmoothBlend", stateName);
        }
    }

    void OnEnable()
    {
        for (int i = 0; i < StateAnimations.Length; i++)
        {
            StateAnimation state = StateAnimations[i];
            AnimationState anim = animation[state.animationName];
            anim.weight = 0.0f;
            anim.normalizedSpeed = 0.0f;
        }

        currentState = GetState(startStateName);
        animation[startStateName].weight = 1.0f;
    }

    void Update()
    {
        if (!isFrozen)
        {
            AnimationState currentStateAnimation = animation[currentState.animationName];
            currentStateAnimation.enabled = true;
            currentStateAnimation.normalizedTime += 1.0f / currentState.Rate;
            if (currentState.stateAfterOneShot != "")
            {

                if (currentStateAnimation.normalizedTime >= 0.9f)
                {
                    //if (currentState.animationName == "Death")
                    //{
                    //    this.enabled = false;
                    //    return;
                    //}
                    if (currentState.stateAfterOneShot == "Previous")
                        ChangeState(previousState.animationName);
                    else
                        ChangeState(currentState.stateAfterOneShot);
                }
            }
        }
    }
}
