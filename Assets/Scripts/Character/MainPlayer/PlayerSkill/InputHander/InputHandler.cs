using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputHandler
{
    protected GameObject owner;
    public InputHandler(GameObject obj)
    {
        owner = obj;
    }
    protected InputHandler _nextHandler;

    public InputHandler SetNextHander(InputHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler;
    }

    public abstract InputHanderResult HandleInput(SkillData data,int skillInventoryIndex);
}