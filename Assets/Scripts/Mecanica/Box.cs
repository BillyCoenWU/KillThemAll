using UnityEngine;

public class Box : MonoBehaviour
{
	private AudioSource m_sound = null;
	public AudioSource sound
	{
		set
		{
			m_sound = value;
		}
	}

	private Weapon.Data m_weaponData = null;
	public Weapon.Data weaponData
	{
		get
		{
			return m_weaponData;
		}

		set
		{
			m_weaponData = value;
		}
	}

	private Transform m_transform = null;
	public new Transform transform
	{
		get
		{
			if(m_transform == null)
			{
				m_transform = base.transform;
			}

			return m_transform;
		}
	}

	public void Reset ()
	{
		m_sound.Play();
		BoxSpawnManager.Instance.StartSpawnBox();
		gameObject.SetActive(false);
	}
}
