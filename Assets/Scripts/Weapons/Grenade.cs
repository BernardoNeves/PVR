using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public float delay = 3f;
    public float radius = 5f;
    public float force = 10f;
    public float damage = 10f;

    private float countDown;
    private bool hasExploded;

    void Start () {

        countDown = delay;

    }

    void Update () {

        countDown -= Time.deltaTime;

        if (countDown <= 0f && !hasExploded) {

            Explode();
            hasExploded = true;

        }

    }

    void Explode () {

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders) {

            Rigidbody rigidbody = nearbyObject.GetComponent<Rigidbody>();

            if (rigidbody != null) {

                rigidbody.AddExplosionForce(force, transform.position, radius);

            }

            HealthInterface healthInterface = nearbyObject.GetComponent<HealthInterface>();
            healthInterface?.Damage(damage);

        }

        Destroy(this.gameObject); 

    } 

}