using UnityEngine;

[System.Serializable]
public class LifeManager
{
	public int life = 0;
	public int maxLife = 0;

	public bool isAlive
	{
		get
		{
			return (life > 0);
		}
	}

	public delegate void DeathEvent ();
	public event DeathEvent Death;

	public LifeManager (int maxLife)
	{
		this.maxLife = maxLife;
		life = this.maxLife;
	}

	public void TakeDamage (int damage)
	{
		if(!isAlive)
		{
			return;
		}

		life -= damage;
		life = Mathf.Clamp(life, 0 , maxLife);

		if(!isAlive)
		{
			Death();
		}
	}
}
