using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStageData
{
    public abstract void InitStage();
    public abstract void Update();
    public abstract void Release();
    public abstract bool ApplyStage(bool apply);

}
