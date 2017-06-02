using UnityEngine;

public enum WEAPON_TYPE : int
{
	PISTOL = 0,
	MACHINEGUN,
	ROCKET_LAUNCHER,
}

public class Weapon : MonoBehaviour
{
	[System.Serializable]
	public class Data
	{
		public int damage = 0;
		public int bullets = 0;

		public float reload = 0.0f;
		public float cooldown = 0.0f;

		public Sprite sprite = null;

		public WEAPON_TYPE type = WEAPON_TYPE.PISTOL;
	}

	private Data m_data = null;

	private SpriteRenderer m_spriteRenderer = null;
	public SpriteRenderer spriteRenderer
	{
		get
		{
			if(m_spriteRenderer == null)
			{
				m_spriteRenderer = GetComponent<SpriteRenderer>();
			}

			return m_spriteRenderer;
		}
	}

	private static Weapon s_instance = null;
	public static Weapon Instance
	{
		get
		{
			return s_instance;
		}
	}

	private void Awake ()
	{
		s_instance = this;
	}

	private void Start ()
	{
		SetWeapon(BoxSpawnManager.Instance.basicData);
	}

	public void SetWeapon (Data newData)
	{
		m_data = newData;

		spriteRenderer.sprite = m_data.sprite;
	}
}
