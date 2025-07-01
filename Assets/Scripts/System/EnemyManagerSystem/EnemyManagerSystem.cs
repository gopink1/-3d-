using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���˹���ϵͳ
/// ��ȡ���ؿ���Ϣ�����ɵ��ˡ�
/// ����ͨ������ػ�ȡ
/// ����ػ���ݹؿ���ϢԤ���ص��ˣ�
/// ��ϵͳ�����ݹؿ���Ϣͨ������ؼ��س�������
/// </summary>
public class EnemyManagerSystem : IGameSystem
{
    private GameObject mainPlayer = null;
    public GameObject MainPlayer { get { return mainPlayer; } }


    private Dictionary<string,List<GameObject>> curEnemy = new Dictionary<string, List<GameObject>>();//��ǰ�������

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
    /// ��ӵ�ǰ����ĵ���
    /// </summary>
    /// <param name="enemyName">���˵�����</param>
    /// <param name="activeEnemy">������˱���</param>
    public void AddCurActiveEnemy(string enemyName,GameObject activeEnemy,int count) 
    { 
        //���жϵ�ǰ�ļ�����˵�����
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
