using UnityEngine;
public static class ResourcesPath
{
    public const string SwordPath = "Weapon";
    public const string EnemyPath = "Character/Enemy/";
}
/// <summary>
/// 资源加载工厂
/// </summary>
public class ResourcesAssetFactory : IAssetFactory
{

    //加载资源的方法
    public override GameObject LoadModel(string path)
    {
        GameObject obj =  Resources.Load<GameObject>(path);
        return obj;
    }
    public override Texture2D LoadSprite(string path)
    {
        Texture2D obj = Resources.Load<Texture2D>(path) as Texture2D;
        return obj;
    }
    public override ScriptableObject LoadSO(string path)
    {
        ScriptableObject obj = Resources.Load<ScriptableObject>(path);
        return obj;
    }
    //public override AnimatorController LoadAnimator(string path)
    //{
    //    AnimatorController obj = Resources.Load<AnimatorController>(path);
    //    return obj;
    //}
}
