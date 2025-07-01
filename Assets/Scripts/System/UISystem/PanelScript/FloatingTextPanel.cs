using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextPanel : PanelBase
{
    [Header("������������")]
    [SerializeField] private float moveDuration = 1f;    // �ƶ�����ʱ��
    [SerializeField] private float fadeDuration = 0.5f;  // ���뵭������ʱ��
    [SerializeField] private Vector3 offset = new Vector3(0, 800, 0); // UI����ƫ����(����)

    private TextMeshProUGUI textComponent;
    private CanvasGroup canvasGroup;
    private RectTransform textRectTransform;
    private Vector2 startAnchoredPosition;

    private void Awake()
    {
        // �����Ӷ��󲢻�ȡ���
        Transform textTransform = transform.Find("Text");
        if (textTransform != null)
        {
            textComponent = textTransform.GetComponent<TextMeshProUGUI>();
            canvasGroup = textTransform.GetComponent<CanvasGroup>();
            textRectTransform = textTransform.GetComponent<RectTransform>();

            // ���û��CanvasGroup����������һ��
            if (canvasGroup == null)
                canvasGroup = textTransform.gameObject.AddComponent<CanvasGroup>();

            // ��¼��ʼλ�ã�ʹ��anchoredPosition����UIԪ�أ�
            if (textRectTransform != null)
                startAnchoredPosition = textRectTransform.anchoredPosition;
        }
        else
        {
            Debug.LogError("δ�ҵ�Text�Ӷ�������UI�ṹ");
        }
    }

    public void FloatingText(string content, Vector2? customStartPos = null)
    {
        if (textComponent == null || canvasGroup == null || textRectTransform == null) return;

        // �����ı�����
        textComponent.text = content;

        // ����λ�ú�͸����
        if (customStartPos.HasValue)
        {
            textRectTransform.anchoredPosition = customStartPos.Value;
            startAnchoredPosition = customStartPos.Value;
        }
        else
        {
            textRectTransform.anchoredPosition = startAnchoredPosition;
        }
        canvasGroup.alpha = 0f;

        // �������ж���
        Sequence sequence = DOTween.Sequence();

        // ����
        sequence.Append(canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true));

        // ʹ��anchoredPosition����UI�ƶ�
        sequence.Join(textRectTransform.DOAnchorPos(startAnchoredPosition + (Vector2)offset, moveDuration)
            .SetEase(Ease.OutQuad).SetUpdate(true));

        // ����
        sequence.Append(canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true));

        // ������ɺ�ص�
        sequence.OnComplete(() => {
            // ����ӻ����߼�
        });
    }

}
