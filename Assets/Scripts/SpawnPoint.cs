using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public IdleBehaviorType IdleBehavior;
    public AggroBehaviorType AggroBehavior;

    public Vector3 Positon => transform.position;
}
