using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public virtual void Update(PlayerHealth playerHealth, int stacks)
    {

    }

    public virtual void OnPickup(int stacks)
    {

    }

    public virtual void OnHit(EnemyHealth enemyHealth, float damageamount, int stacks)
    {

    }
}

public class HealingItem : Item
{
    public override void Update(PlayerHealth playerHealth, int stacks)
    {
        playerHealth.Heal(2.5f * stacks);
    }
}

public class MaxHealthItem : Item
{
    public override void OnPickup(int stacks)
    {
        GameManager.instance.Player.GetComponent<PlayerHealth>().MaxHealth = 100 + 10 * stacks;
    }
}
public class ShieldRate : Item
{
    public override void OnPickup(int stacks)
    {
        GameManager.instance.Player.GetComponent<PlayerHealth>()._shieldRechargeAmount = 1 + stacks;
    }
}

public class MaxShieldItem : Item
{
    public override void OnPickup(int stacks)
    {
        GameManager.instance.Player.GetComponent<PlayerHealth>().MaxShield = 100 + 10 * stacks;
    }
}

public class SpeedItem : Item
{
    public override void OnPickup(int stacks)
    {
        GameManager.instance.Player.GetComponent<StarterAssets.FirstPersonController>().MoveSpeed = 4 + 0.25f * stacks;
        GameManager.instance.Player.GetComponent<StarterAssets.FirstPersonController>().SprintSpeed = 6 + 0.25f * stacks;
    }
}

public class JumpItem : Item
{
    public override void OnPickup(int stacks)
    {
        GameManager.instance.Player.GetComponent<StarterAssets.FirstPersonController>().JumpHeight = 1.2f + 0.125f  * stacks;
    }
}

public class GravityItem : Item
{
    public override void OnPickup(int stacks)
    {
        GameManager.instance.Player.GetComponent<StarterAssets.FirstPersonController>().Gravity = -15 + 0.5f * stacks;
    }
}

public class DamageItem : Item
{
    public override void OnHit(EnemyHealth enemyHealth, float damageamount, int stacks)
    {
        enemyHealth.Damage( 1.6f * stacks);
    }
}

public class LifeStealItem : Item
{
    public override void OnHit(EnemyHealth enemyHealth, float damageamount, int stacks)
    {
        GameManager.instance.PlayerHealth.Heal(damageamount/10 * stacks);
    }
}

