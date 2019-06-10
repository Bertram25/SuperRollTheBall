using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttractorData : MonoBehaviour
{
    public float nominalHeight = 1f;

    public float minX = -4f;
    public float maxX = 4f;

    public float StartXPosition
    {
        get { return Random.Range(minX, maxX); }
    }
}
