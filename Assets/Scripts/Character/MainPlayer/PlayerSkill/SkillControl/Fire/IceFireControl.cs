using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFireControl : MonoBehaviour
{
    //火焰飞弹的效果
    public float flightSpeed = 10f;
    public float maxDistance = 20f;

    private Vector3 target; // 目标位置（可传参或射线检测获取）

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
        //transform.position = 主角.transform.position + 主角.transform.forward * 发射距离;
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

            // 射线检测命中目标
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, flightSpeed * Time.deltaTime,layers))
            {
                OnHit(hit.collider);
                yield break; // 提前结束协程
            }

            elapsed +=   Time.deltaTime;
            yield return null;
        }

        // 未命中时到达最大距离消失
        OnMissed();
    }

    void OnHit(Collider target)
    {
        //判断是否使敌人

        // 触发伤害逻辑、生成爆炸效果等
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
