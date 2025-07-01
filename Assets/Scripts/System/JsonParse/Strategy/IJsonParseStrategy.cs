using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IJsonParseStrategy<T>
{
    protected string rootPath;

    public string GetRootPath()
    {
        return rootPath;
    }
    public IJsonParseStrategy()
    {
        InitPath();
    }
    protected abstract void InitPath();
    public abstract T Parse(string fullPath);
    public virtual void Save(T data, string fullPath)
    {
        throw new NotImplementedException("默认不支持保存操作");
    }
}
