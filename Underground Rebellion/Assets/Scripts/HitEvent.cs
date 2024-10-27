using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    public delegate void HitHandler(int damage, GameObject sender, GameObject receiver);
    public static event HitHandler OnHit;

    public static void GetHit(int damage, GameObject sender, GameObject receiver)
    {
        OnHit?.Invoke(damage,sender, receiver);
    }
}
