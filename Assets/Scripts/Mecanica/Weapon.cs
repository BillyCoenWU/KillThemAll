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

		public AudioClip shootAudio = null;

		public WEAPON_TYPE type = WEAPON_TYPE.PISTOL;
	}


	[SerializeField]
	private Transform m_cannon = null;

	[SerializeField]
	private AudioSource m_audioSource = null;

	private Data m_data = null;

	private Bullet m_bullet = null;

	private bool m_reloading = false;

	private int m_currentBullets = 0;

	private float m_currentTime = 0.0f;

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
		if(s_instance != null)
		{
			DestroyImmediate(s_instance);
		}

		s_instance = this;
	}

	private void Start ()
	{
		SetWeapon(BoxSpawnManager.Instance.basicData);
	}

	public void WeaponUpdate ()
	{
		if(Input.GetMouseButtonUp(0))
		{
			m_audioSource.Stop();
			return;
		}

		if(Input.GetMouseButton(0))
		{
			m_currentTime += Time.deltaTime;

			if(m_reloading)
			{
				if(m_currentTime >= m_data.reload)
				{
					m_currentBullets = 0;
					m_currentTime = 0.0f;
					m_reloading = false;
					return;
				}
			}
			else
			{
				if(m_currentTime >= m_data.cooldown)
				{
					Shoot();
				}
			}
		}
	}

	public void SetWeapon (Data newData)
	{
		m_data = newData;

		spriteRenderer.sprite = m_data.sprite;

		m_audioSource.clip = m_data.shootAudio;
	}

	public void Flip ()
	{

	}

	private void Shoot ()
	{
		m_audioSource.Play();

		m_bullet = GameManager.Instance.LoadBullet();
		m_bullet.transform.localPosition = m_cannon.position;
		m_bullet.damage = m_data.damage;

		m_currentTime = 0.0f;

		m_currentBullets++;
		if(m_currentBullets >= m_data.bullets)
		{
			if(m_data.type == WEAPON_TYPE.MACHINEGUN)
			{
				m_audioSource.Stop();
			}
			m_reloading = true;
		}
	}
}
