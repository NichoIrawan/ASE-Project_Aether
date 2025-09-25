
using System;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public Vector3 position;
    public EnemyController.StateMachine currentState;
    public float stunTimer;

    public EnemyData(Vector3 position, EnemyController.StateMachine currentState, float stunTimer)
    {
        this.position = position; 
        this.currentState = currentState;
        this.stunTimer = stunTimer;
    }
}