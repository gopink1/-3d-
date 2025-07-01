using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人管理系统
/// 获取到关卡信息，生成敌人。
/// 可以通过对象池获取
/// 对象池会根据关卡信息预加载敌人，
/// 该系统则会根据关卡信息通过对象池加载出来敌人
/// </summary>
public class EnemyManagerSystem : IGameSystem
{
    private GameObject mainPlayer = null;
    public GameObject MainPlayer { get { return mainPlayer; } }


    private Dictionary<string,List<GameObject>> curEnemy = new Dictionary<string, List<GameObject>>();//当前存货敌人

    public EnemyManagerSystem(GameBase gameBase) : base(gameBase)
    {
        Init();
    }

    public override void Init()
    {
        if (mainPlayer == null)
        {
            mainPlayer = GameObject.FindWithTag("Player");
            Debug.Log(mainPlayer.gameObject.name);
        }
    }

    public override void Release()
    {
        curEnemy.Clear();
    }

    public override void Update()
    {

        
    }
    /// <summary>
    /// 添加当前激活的敌人
    /// </summary>
    /// <param name="enemyName">敌人的名字</param>
    /// <param name="activeEnemy">激活敌人本身</param>
    public void AddCurActiveEnemy(string enemyName,GameObject activeEnemy,int count) 
    { 
        //先判断当前的激活敌人的类型
        if(!curEnemy.ContainsKey(enemyName))
        {
            List<GameObject> list = new List<GameObject>();
            for(int i = 0; i<count; i++)
            {
                list.Add(activeEnemy);
            }
            curEnemy.Add(enemyName, new List<GameObject>());
        }
        else
        {
            curEnemy[enemyName].Add(activeEnemy);
        }
    }

}
