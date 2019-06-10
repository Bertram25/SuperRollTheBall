using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float offset = 3f;
    private Vector3 _originPosition;
    private bool _moveLeft = true;
    private float startTimeDelta = 0f;

    void Start()
    {
        _originPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _moveLeft = Random.Range(0, 100) > 50;

        startTimeDelta = transform.position.z * 10f;
    }

    void Update()
    {
        MoveGameObject();
    }

    private void MoveGameObject()
    {
        startTimeDelta -= Time.unscaledTime;
        if (startTimeDelta > 0f)
        {
            return;
        }

        float destXPosition = _moveLeft ? _originPosition.x - offset : _originPosition.x + offset;
        float newXPosition = Mathf.Lerp(transform.position.x, destXPosition, Time.deltaTime);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        if (Mathf.Abs(newXPosition - destXPosition) < 0.2f)
        {
            _moveLeft = !_moveLeft;
        }
    }
}
