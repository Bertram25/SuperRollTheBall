using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleData : MonoBehaviour
{
    public int scoreValue = 10;
    public float nominalHeight = 1.2f;

    public float StartXPosition
    {
        get { return Random.Range(-3f, 3f); }
    }
}
