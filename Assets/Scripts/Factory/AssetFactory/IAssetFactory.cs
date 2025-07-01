using UnityEngine;

public enum ENUM_LoadType
{
    Resources,
    ABAsset,
}
public abstract class IAssetFactory
{
    public abstract GameObject LoadModel( string path);
    public abstract ScriptableObject LoadSO(string path);
    //public abstract AnimatorController LoadAnimator(string path);
    public abstract Texture2D LoadSprite(string path);
}
