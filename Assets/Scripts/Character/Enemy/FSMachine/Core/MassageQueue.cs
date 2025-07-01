using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Message
{
    public float damage;
    public string hitName;
    public float currentHealth;

    public Message(float damage, string hitName, float currentHealth)
    {
        this.damage = damage;
        this.hitName = hitName;
        this.currentHealth = currentHealth;
    }
}

public class MassageQueue
{
    private Queue<Message> queue = new Queue<Message>();

    // 入队操作
    public void Enqueue(Message message)
    {
        queue.Enqueue(message);
    }

    // 出队操作
    public bool Dequeue(out Message message)
    {
        if (queue.Count > 0)
        {
            message = queue.Dequeue();
            return true;
        }
        message = new Message(0, null, 0);
        return false;
    }
}
