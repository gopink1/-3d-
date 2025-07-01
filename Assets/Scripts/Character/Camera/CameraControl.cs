using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraControl : MonoBehaviour
{
    [SerializeField, Header("�������")] private float controlSpeed;
    [SerializeField] private Vector2 viewRange;
    [SerializeField] private Vector3 currentVelocity;
    [SerializeField] private float freeViewSmoothTime;
    [SerializeField] private float lockedViewSmoothTime;

    private Transform playerTrans;  //��ҵ�λ��
    private Transform lockedTarget; //���˵�λ��
    private Vector3 LockedViewPoint;//�ӽ����������ĵ㣨Ϊ����Ŀ������λ�õ����ģ�
    [SerializeField] float lookTargetOffset;        //���������������λ�õ�ƫ����
    [SerializeField] float lockedCameraHeight;     //����������߶�

    [SerializeField] float smoothPositionVelocity;

    [SerializeField] float circleRadius; // ���Χ�����Բ���˶��İ뾶
    [SerializeField] float circleYOffset;// ���Χ�����Բ���˶���Y����
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
    //��ȡ����
    private void GetInput()
    {
        if (!isLockView)
        {
            //��ȡ����õ��ĽǶ�
            getInput.y += GameInputManager.MainInstance.CameraView.x;
            getInput.x -= GameInputManager.MainInstance.CameraView.y;
            getInput.x = Mathf.Clamp(getInput.x, viewRange.x, viewRange.y);//�޶���Χ
        }

    }
    //�ı�Ƕ�,����������ĽǶ�
    private void UpdateRoattion()
    {
        if (isLockView && lockedTarget != null)
        {
            //�ҵ�����λ��
            LockedViewPoint = (playerTrans.position - lockedTarget.position) * 0.3f + lockedTarget.position;//����λ��Ϊ���������֮��ʮ��֮��λ��

            Vector3 direction = (LockedViewPoint - playerTrans.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);//��ת

            // ʹ�� Slerp ����ƽ����ֵ
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lockedViewSmoothTime);
            transform.rotation  = targetRotation;

            float heightDifference = transform.position.y - LockedViewPoint.y;
            float pitchAngle = Mathf.Atan2(heightDifference, Vector3.Distance(playerTrans.position, LockedViewPoint)) * Mathf.Rad2Deg;
            // ���Ƹ����Ƿ�Χ
            pitchAngle = Mathf.Clamp(pitchAngle, viewRange.x, viewRange.y);
            // �������յ���ת
            transform.rotation = Quaternion.Euler(pitchAngle, targetRotation.eulerAngles.y, 0f);
            //Debug.Log("��ǰΪ����״̬��ŷ����"+transform.eulerAngles);
        }
        else
        {
            cameraRotation = Vector3.SmoothDamp(cameraRotation, new Vector3(getInput.x, getInput.y, 0), ref currentVelocity, freeViewSmoothTime);
            transform.eulerAngles = cameraRotation;
            //Debug.Log("��ǰΪ������״̬��ŷ����"+transform.eulerAngles);

        }
    }
    //������������
    private void UpdatePosition()
    {
        if (isLockView && lockedTarget != null)
        {
            // �������Ŀ�꣬��������������λ��
            Vector3 direction = (lockedTarget.position - playerTrans.position).normalized;
            // ���������������λ��
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

        // ƽ������ cameraRotation ����������
        cameraRotation = transform.eulerAngles;
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(playerTrans.position + new Vector3(0, circleYOffset, 0), circleRadius);
    }
}
