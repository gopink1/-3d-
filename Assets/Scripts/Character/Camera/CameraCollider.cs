using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    //从摄像机身后发出射线检测是否有物体
    //按需求缩小偏移量
    [SerializeField, Header("最大小偏移量")] private Vector2 maxOffsetDistance;
    [SerializeField, Header("检测层级"), Space(10)] private LayerMask detectLayer;
    [SerializeField, Header("射线的长度"), Space(10f)] private float detectLength;

    //初始的位置和初始的偏移距离
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
        var detectionDir = transform.TransformPoint(initionPosition * detectLength);//把本地坐标变换为世界坐标
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
