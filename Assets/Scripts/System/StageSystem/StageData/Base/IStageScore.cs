using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStageScore
{
    public abstract bool CheckScore();

    public abstract void InitScore();

    public abstract void Release();
}
