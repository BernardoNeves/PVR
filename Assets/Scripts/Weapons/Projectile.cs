using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float damage;
    public float timeLeftLife = 5f;

    private float timeDelta;

    void Update() {

        timeDelta += Time.deltaTime;

        if (timeDelta > timeLeftLife) {

            Destroy(this.gameObject);

        }

    }

    private void OnTriggerEnter(Collider collider) {

        if (collider.tag == "Player") {

            GameManager.instance.PlayerHealth.Damage(damage);

            Destroy(this.gameObject);

        }
        
    }

}