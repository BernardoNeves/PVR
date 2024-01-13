using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerHealth : HealthManager
{
    [Header("UI")]
    [SerializeField] GameObject hud;
    [SerializeField] TMP_Text localName;
    [SerializeField] GameObject OverHeadUI;
    [SerializeField] Healthbar _healthbar;
    [SerializeField] Healthbar _shieldbar;
    PhotonView PV;
    PlayerManager playerManager;

    [Header("Inventory")]
    public List<ItemStack> itemList = new List<ItemStack>();

    public void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    public PlayerHealth(float health, float maxHealth, float shield, float maxShield) : base(health, maxHealth, shield, maxShield)
    {
        _currentHealth = health;
        _currentMaxHealth = maxHealth;
        _currentShield = shield;
        _currentMaxShield = maxShield;

        if (_currentHealth > _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
        }
    }
    public override void Start()
    {
        base.Start();
        if (!PV.IsMine)
        {
            Destroy(hud);
            _healthbar = OverHeadUI.transform.Find("HealthBar").GetComponentInChildren<Healthbar>();
            _shieldbar = OverHeadUI.transform.Find("ShieldBar").GetComponentInChildren<Healthbar>();

        }
        else
        {
            localName.text = PV.Owner.NickName;
            Destroy(OverHeadUI);
        }


        StartCoroutine(CallItemUpdate());
    }

    public override void Update()
    {
        base.Update();
        if (_healthbar)
        {
            _healthbar.SetCurrent(Health);
            _healthbar.SetMax(MaxHealth);
        }
        if (_shieldbar)
        {
            _shieldbar.SetCurrent(Shield);
            _shieldbar.SetMax(MaxShield);
        }


    }

    public override void Damage(float damageAmount)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damageAmount);
    }

    [PunRPC]
    void RPC_TakeDamage(float damageAmount)
    {
        DamageShield(damageAmount);
    }

    public override void OnDeath()
    {
        if(PV.IsMine)
            playerManager.Death();
    }

    IEnumerator CallItemUpdate()
    {
        foreach (ItemStack i in itemList)
        {
            i.Item.Update(this, i.Stacks);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CallItemUpdate());
    }

    public void CallItemOnPickup()
    {
        foreach (ItemStack i in itemList)
        {
            i.Item.OnPickup(i.Stacks);
        }
    }

    public void CallItemOnHit(EnemyHealth enemyHealth, float damageamount)
    {
        foreach (ItemStack i in itemList)
        {
            i.Item.OnHit(enemyHealth, damageamount, i.Stacks);
        }
    }

}