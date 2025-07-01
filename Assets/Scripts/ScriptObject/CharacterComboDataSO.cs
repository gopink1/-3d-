using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboDate", menuName = "Create/Character/ComboData", order = 0)]
public class CharacterComboDataSO : ScriptableObject
{
    //��ʽ����
    [SerializeField] private string comboName;//��ʽ������
    [SerializeField] private string[] comboParryName;//��Ӧ�ĸ񵲶���s
    [SerializeField] private string[] comboHitName;//��ǰ��ʽ����Ĵ�������������ɶ���˺���
    [SerializeField] private float damage;//��ɵ��˺�
    [SerializeField] private float coldTime;//���е���ȴ�¼��ν���һ��������ʱ��
    [SerializeField] private Vector2 matchTime;//����ƥ���ʱ���% 
    [SerializeField] private float comboPositionOffset;//���ߵ���Ѿ���
    [SerializeField] private float comboAtkRange;//������Χ
    [SerializeField] private float comboAngleRange;//�����Ƕȷ�Χ
    public string ComboName => comboName;
    public string[] ComboParryName => comboParryName;
    public string[] ComboHitName => comboHitName;
    public float ColdTime => coldTime;
    public Vector2 MatchTime => matchTime;
    public float Damage => damage;
    public float ComboPositonOffset => comboPositionOffset;
    public float ComboAtkRange => comboAtkRange;
    public float ComboAngleRange => comboAngleRange;


    //��ȡ��ǰ�����������������
    public int GetHitAndParryMaxCount() => comboHitName.Length;

}
