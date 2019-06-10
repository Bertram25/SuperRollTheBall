using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttractor : MonoBehaviour
{
    public GameObject attractor;
    public float attractionSpeed = 5f;
    public float distanceForAttraction = 20f;
    public float pushForce = 10f;

    void Update()
    {
        if (attractor == null)
        {
            return;
        }

        if (Vector3.Distance(transform.position, attractor.transform.position) > distanceForAttraction)
        {
            return;
        }

        var sphereCollider = attractor.gameObject.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            float radius = sphereCollider.radius;
            if (Vector3.Distance(transform.position, attractor.transform.position) - radius <= 0f)
            {
                return;
            }
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            attractor.transform.position,
            attractionSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody rigidBody = other.gameObject.GetComponent<Rigidbody>();
            if (rigidBody == null)
            {
                return;
            }
            
            rigidBody.AddForce(Vector3.Normalize(other.transform.position - transform.position) * pushForce, ForceMode.Impulse);
        }
    }
}
