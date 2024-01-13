using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Create New weapon")]
public class GunData : ScriptableObject
{

    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector] public bool reloading;

}