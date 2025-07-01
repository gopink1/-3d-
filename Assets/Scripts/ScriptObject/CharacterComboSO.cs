using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "Create/Character/Combo", order = 1)]
public class CharacterComboSO : ScriptableObject
{
    [SerializeField] private List<CharacterComboDataSO> allComboData = new List<CharacterComboDataSO>();

    //��ȡ�����еĶ���
    public string TryGetOneComboAction(int index)
    {
        if (allComboData.Count == 0) return null;
        return allComboData[index].ComboName;
    }
    //��ȡ�˺������ڼ�����ʽ�ĵڼ����˺��Ķ���
    public string TryGetOneHitComboAction(int index, int hitIndex)
    {
        if (allComboData.Count == 0) return null;//���б�Ϊ�ձ�
        if (allComboData[index].GetHitAndParryMaxCount() == 0) return null;//û�����ñ�������
        //Debug.Log(index +"+" + hitIndex);
        return allComboData[index].ComboHitName[hitIndex];

    }
    //��ȡ�񵲶����ڼ�����ʽ�ĵڼ����˺��Ķ���
    public string TryGetOneParryComboAction(int index, int ParryIndex)
    {
        if (allComboData.Count == 0) return null;//���б�Ϊ�ձ�
        if (allComboData[index].GetHitAndParryMaxCount() == 0) return null;//û�����ñ�������
        return allComboData[index].ComboParryName[ParryIndex];
    }
    /// <summary>
    /// �˺�ֵ
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetDmage(int index)
    {
        if (allComboData.Count == 0) return 0f;
        return allComboData[index].Damage;
    }
    /// <summary>
    /// ��ȴʱ��
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetColdTime(int index)
    {
        if (allComboData.Count == 0) return 0f;
        return allComboData[index].ColdTime;
    }
    public Vector2 TryGetComboMatchTime(int index)
    {
        if (allComboData.Count == 0) return Vector2.zero;
        return allComboData[index].MatchTime;
    }

    /// <summary>
    /// ��Ѿ���
    /// �����ڴ����ȶ������Ž���ƥ��
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetComboPositionOffset(int index)
    {
        if (allComboData.Count == 0) return 0f;
        return allComboData[index].ComboPositonOffset;
    }
    /// <summary>
    /// ��ȡ������Χ
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetComboAtkRange(int index)
    {
        if (allComboData.Count == 0) return 0f;
        return allComboData[index].ComboAtkRange;
    }
    /// <summary>
    /// ��ȡ�����Ƕ�
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetComboAngleRange(int index)
    {
        if(allComboData.Count == 0)return 0f;
        return allComboData[index].ComboAngleRange;
    }
    public int TryGetHitAndParryMaxCount(int index) => allComboData[index].GetHitAndParryMaxCount();//������ĳһ�εĴ���������˺���������

    public int TryGetComboMaxCount() => allComboData.Count;//���д���

}

