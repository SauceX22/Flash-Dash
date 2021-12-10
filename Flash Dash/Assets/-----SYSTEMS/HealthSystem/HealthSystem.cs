using System;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;

    private int health;
    private int healthMax;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return healthMax;
    }

    public float GetHealthPerCent()
    {
        return (float)health / healthMax;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        
        if (health < 0) health = 0;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        health += healAmount;

        if (health > 100) health = 100;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
