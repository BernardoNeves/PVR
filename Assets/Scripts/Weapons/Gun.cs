using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    [Header("References")]
    [SerializeField] public GunData gunData;
    [SerializeField] public Transform PlayerCamera;
    [SerializeField] WeaponUI _weaponUI;

    float timeSinceLastShot = 0;

    private void Awake()
    {
        gunData.currentAmmo = gunData.magSize;
    }

    public void OnEnable() {

        PlayerBehaviour.shootInput += Shoot;
        PlayerBehaviour.reloadInput += StartReload;

    }

    private void OnDisable()
    {
        PlayerBehaviour.shootInput -= Shoot;
        PlayerBehaviour.reloadInput -= StartReload;
        gunData.reloading = false;
    }

    public void StartReload()
    {

        if (!gunData.reloading && this.gameObject.activeSelf) {

            StartCoroutine(Reload());
        
        }

    }

    private IEnumerator Reload()
    {

        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }

    private bool CanShoot() {

        float timeBetweenShot = 1f / (gunData.fireRate / 60f);


        if (gunData.reloading == true) {

            return false;

        } else if (timeSinceLastShot < timeBetweenShot) {

            return false;

        } else {

            return true;

        }

    }

    public void Shoot() {

        var ray = new Ray(PlayerCamera.position, PlayerCamera.forward);
        RaycastHit hitInfo;

        if (gunData.currentAmmo > 0) {

            if (CanShoot()) {

                if (Physics.Raycast(ray, out hitInfo, gunData.maxDistance))
                {

                    HealthInterface healthInterface = hitInfo.transform.GetComponent<HealthInterface>();
                    healthInterface?.Damage(gunData.damage);
                    EnemyHealth enemyHealth = hitInfo.transform.GetComponent<EnemyHealth>();
                    if(enemyHealth != null && enemyHealth.Health > 0)
                    {
                        GameManager.instance.Player.GetComponent<PlayerHealth>().CallItemOnHit(enemyHealth, gunData.damage);
                    }

                } 

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShoot();

            }

        }

    }
    
    private void Update() {

        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(PlayerCamera.position, PlayerCamera.forward * gunData.maxDistance);

        _weaponUI.UpdateInfo(gunData.magSize, gunData.currentAmmo);

    }

    private void OnGunShoot() { }   

}