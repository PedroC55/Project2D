using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;

    // Update is called once per frame
    void Update()
    {
        if (_enemyAI.IsDead())
        {
            Destroy(gameObject);
        }

    }
}
