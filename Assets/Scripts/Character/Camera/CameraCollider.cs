using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    //���������󷢳����߼���Ƿ�������
    //��������Сƫ����
    [SerializeField, Header("���Сƫ����")] private Vector2 maxOffsetDistance;
    [SerializeField, Header("���㼶"), Space(10)] private LayerMask detectLayer;
    [SerializeField, Header("���ߵĳ���"), Space(10f)] private float detectLength;

    //��ʼ��λ�úͳ�ʼ��ƫ�ƾ���
    private Vector3 initionPosition;
    private float initionOffsetDistance;

    private Transform mainCamera;
    private void Awake()
    {
        mainCamera= transform.Find("Main Camera");
    }
    private void Start()
    {
        initionPosition = transform.localPosition.normalized;
        initionOffsetDistance = maxOffsetDistance.y;
    }
    private void OnEnable()
    {
        
    }
    private void LateUpdate()
    {
        UpdateCameraCollider();
    }
    private void UpdateCameraCollider()
    {
        var detectionDir = transform.TransformPoint(initionPosition * detectLength);//�ѱ�������任Ϊ��������
        if (Physics.Linecast(transform.position, detectionDir, out var hit, detectLayer, QueryTriggerInteraction.Ignore))
        {
            initionOffsetDistance = Mathf.Clamp(initionOffsetDistance * .8f, maxOffsetDistance.x, maxOffsetDistance.y);
        }
        else
        {
            initionOffsetDistance = maxOffsetDistance.y;
        }
        mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition,
            initionPosition * (initionOffsetDistance - 0.1f),
            Time.deltaTime);
    }
}
