using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraControl : MonoBehaviour
{
    [SerializeField, Header("相机参数")] private float controlSpeed;
    [SerializeField] private Vector2 viewRange;
    [SerializeField] private Vector3 currentVelocity;
    [SerializeField] private float freeViewSmoothTime;
    [SerializeField] private float lockedViewSmoothTime;

    private Transform playerTrans;  //玩家的位置
    private Transform lockedTarget; //敌人的位置
    private Vector3 LockedViewPoint;//视角锁定的中心点（为锁定目标和玩家位置的中心）
    [SerializeField] float lookTargetOffset;        //锁定后相机与锁定位置的偏移量
    [SerializeField] float lockedCameraHeight;     //锁定后相机高度

    [SerializeField] float smoothPositionVelocity;

    [SerializeField] float circleRadius; // 相机围绕玩家圆周运动的半径
    [SerializeField] float circleYOffset;// 相机围绕玩家圆周运动的Y坐标
    private Vector2 getInput;
    private Vector3 cameraRotation;


    private bool isLockView;
    public bool IsLocked
    {
        get => isLockView;
    }


    private void Awake()
    {
        playerTrans = GameObject.FindWithTag("looktarget").transform;
    }
    private void Start()
    {
    }
    private void Update()
    {
        GetInput();
    }
    private void LateUpdate()
    {
        UpdateRoattion();
        UpdatePosition();
    }


    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameEventManager.MainInstance.AddEventListening<Transform,bool>(EventHash.OnCameraViewLock, LockViewToTarget);
        
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameEventManager.MainInstance.RemoveEvent<Transform,bool>(EventHash.OnCameraViewLock, LockViewToTarget);
    }
    //获取输入
    private void GetInput()
    {
        if (!isLockView)
        {
            //获取输入得到的角度
            getInput.y += GameInputManager.MainInstance.CameraView.x;
            getInput.x -= GameInputManager.MainInstance.CameraView.y;
            getInput.x = Mathf.Clamp(getInput.x, viewRange.x, viewRange.y);//限定范围
        }

    }
    //改变角度,更新摄像机的角度
    private void UpdateRoattion()
    {
        if (isLockView && lockedTarget != null)
        {
            //找到锁定位置
            LockedViewPoint = (playerTrans.position - lockedTarget.position) * 0.3f + lockedTarget.position;//锁定位置为敌人与玩家之间十分之三位置

            Vector3 direction = (LockedViewPoint - playerTrans.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);//旋转

            // 使用 Slerp 进行平滑插值
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lockedViewSmoothTime);
            transform.rotation  = targetRotation;

            float heightDifference = transform.position.y - LockedViewPoint.y;
            float pitchAngle = Mathf.Atan2(heightDifference, Vector3.Distance(playerTrans.position, LockedViewPoint)) * Mathf.Rad2Deg;
            // 限制俯仰角范围
            pitchAngle = Mathf.Clamp(pitchAngle, viewRange.x, viewRange.y);
            // 设置最终的旋转
            transform.rotation = Quaternion.Euler(pitchAngle, targetRotation.eulerAngles.y, 0f);
            //Debug.Log("当前为锁定状态的欧拉角"+transform.eulerAngles);
        }
        else
        {
            cameraRotation = Vector3.SmoothDamp(cameraRotation, new Vector3(getInput.x, getInput.y, 0), ref currentVelocity, freeViewSmoothTime);
            transform.eulerAngles = cameraRotation;
            //Debug.Log("当前为非锁定状态的欧拉角"+transform.eulerAngles);

        }
    }
    //锁定跟随人物
    private void UpdatePosition()
    {
        if (isLockView && lockedTarget != null)
        {
            // 如果锁定目标，则计算摄像机的新位置
            Vector3 direction = (lockedTarget.position - playerTrans.position).normalized;
            // 计算摄像机的最终位置
            Vector3 cameraPosition = playerTrans.position + (-(direction)  * lookTargetOffset);
            cameraPosition.y = lockedCameraHeight;
            transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothPositionVelocity * Time.deltaTime);

        }
        else
        {
            Vector3 targetPosition = playerTrans.position + (-transform.forward * lookTargetOffset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothPositionVelocity * Time.deltaTime);
        }
    }

    private void LockViewToTarget(Transform target,bool isLock)
    {
        isLockView = isLock;
        lockedTarget = target;

        // 平滑过渡 cameraRotation 到锁定方向
        cameraRotation = transform.eulerAngles;
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(playerTrans.position + new Vector3(0, circleYOffset, 0), circleRadius);
    }
}
