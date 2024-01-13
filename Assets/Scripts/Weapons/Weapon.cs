using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [Header("References")]
    public GunData weaponData;
    public Animator animator;
    PhotonView PV;


    private float timeSinceLastAttack = 0;
    public bool isAttacking = false;
    private void Awake()
    {
        PV = GetComponentInParent<PhotonView>();
    }

    public void OnEnable() {
        if (PV.IsMine)
            PlayerBehaviour.shootInput += Attack;
    }

    private void OnDisable()
    {
        PlayerBehaviour.shootInput -= Attack;
    }

    private bool CanAttack() {

        float timeBetweenAttack = 1f / (weaponData.fireRate / 60f);


        if (weaponData.reloading == true) {

            return false;

        } else if (timeSinceLastAttack < timeBetweenAttack) {

            return false;

        }
        return true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        PhotonView collider_PV = collider.GetComponent<PhotonView>();

        if (collider_PV == null || collider_PV.IsMine)
            return;

        if (isAttacking)
        {
            HealthInterface healthInterface = collider.transform.GetComponent<HealthInterface>();
            healthInterface?.Damage(weaponData.damage);
        }
    }

    public void Attack() {

        if (CanAttack()) {
            animator.SetTrigger("Attack");
            StartCoroutine(Attacking());
            timeSinceLastAttack = 0;
        }
    }
    
    private void Update() {
        timeSinceLastAttack += Time.deltaTime;
    }

    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking = true;
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }
}