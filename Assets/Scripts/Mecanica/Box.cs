using UnityEngine;

public class Box : MonoBehaviour
{
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
		BoxSpawnManager.Instance.StartSpawnBox();
		gameObject.SetActive(false);
	}
}
