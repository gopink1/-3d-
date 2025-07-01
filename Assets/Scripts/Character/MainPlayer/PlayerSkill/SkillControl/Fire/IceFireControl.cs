using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFireControl : MonoBehaviour
{
    //����ɵ���Ч��
    public float flightSpeed = 10f;
    public float maxDistance = 20f;

    private Vector3 target; // Ŀ��λ�ã��ɴ��λ����߼���ȡ��

    private ParticleSystem particleSys;
    [SerializeField] LayerMask layers;

    private void Awake()
    {
        particleSys = transform.Find("AoEBlue").GetComponent<ParticleSystem>();
        particleSys.Stop();
    }
    public void Launch(Vector3 target,float maxDistance)
    {
        this.target = target;
        this.maxDistance = maxDistance;
        //transform.position = ����.transform.position + ����.transform.forward * �������;
        StartCoroutine(FlyToTarget());
    }

    IEnumerator FlyToTarget()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = target;

        float elapsed = 0f;
        while (elapsed < maxDistance / flightSpeed)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, flightSpeed * Time.deltaTime);

            // ���߼������Ŀ��
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, flightSpeed * Time.deltaTime,layers))
            {
                OnHit(hit.collider);
                yield break; // ��ǰ����Э��
            }

            elapsed +=   Time.deltaTime;
            yield return null;
        }

        // δ����ʱ������������ʧ
        OnMissed();
    }

    void OnHit(Collider target)
    {
        //�ж��Ƿ�ʹ����

        // �����˺��߼������ɱ�ըЧ����
        if (particleSys.isStopped)
        {
            particleSys.Play();
        }

        TimerManager.MainInstance.TryEnableOneGameTimer(1f, () =>
        {
            Destroy(gameObject);
        });
    }

    void OnMissed()
    {
        Destroy(gameObject, 0.2f);
    }



}
