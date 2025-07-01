using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillEquipedSelectPanel : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] private SkillBar Btn;
    [SerializeField] private SkillInventoryPanel InventoryPanel;
    [SerializeField] private Transform Select;
    [SerializeField] private Transform SelectBtn;
    [SerializeField] private Transform Icon;
    private void Awake()
    {
        //Select = transform.Find("Select");
        //SelectBtn = transform.Find("ButtonImageSelect");
        //Icon = transform.Find("Icon");
    }


    public void SetIcon(Sprite sp)
    {
        if (sp == null) 
        {
            return;
        }

        Icon.GetComponent<Image>().sprite = sp;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select.gameObject.SetActive(true);
        SelectBtn.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Select.gameObject.SetActive(false);
        SelectBtn.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        //点击按钮通知装备技能信息
        InventoryPanel.SelectDown();
        GameEventManager.MainInstance.CallEvent<SkillBar,SkillCardData>(EventHash.EquipSkill,Btn, InventoryPanel.Data);

    }
}
