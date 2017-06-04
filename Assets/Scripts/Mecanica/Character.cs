using UnityEngine;

public class Character : MonoBehaviour
{
	private const string HORIZONTAL = "Horizontal";

	private bool m_canJump = false;

	private float m_axisX = 0.0f;
	private const float m_speed = 3.0f;

	private readonly Vector2 JUMP_FORCE = new Vector2(0.0f, 8.0f);

	[SerializeField]
	private AudioClip[] m_sounds = null;

	[SerializeField]
	private AudioSource m_audioSource = null;

	[SerializeField]
	private SpriteRendererAnimation m_animationController = null;
	public SpriteRendererAnimation animationController
	{
		get
		{
			return m_animationController;
		}
	}

	[SerializeField]
	private LifeManager m_lifeManager = null;
	public LifeManager lifeManager
	{
		get
		{
			return m_lifeManager;
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

	private Rigidbody2D m_rigidbody2D = null;
	public new Rigidbody2D rigidbody2D
	{
		get
		{
			if(m_rigidbody2D == null)
			{
				m_rigidbody2D = GetComponent<Rigidbody2D>();
			}

			return m_rigidbody2D;
		}
	}

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

	private static Character s_instance = null;
	public static Character Instance
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

		m_animationController.PlayByType(ANIMATION_TYPE.IDLE);
	}

	private void Start ()
	{
		m_lifeManager = new LifeManager(1);
		m_lifeManager.Death += this.Death;
		m_lifeManager.Death += GameManager.Instance.EndGame;
	}
	
	private void Update ()
    {
		if(GameManager.Instance.gameOver || !GameManager.Instance.hasStarted)
		{
			return;
		}

		if(PauseManager.Instance.isPaused)
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				PauseManager.Instance.Resume();
			}

			return;
		}
		else
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				PauseManager.Instance.Pause();
				return;
			}
		}

		m_axisX = Input.GetAxis(HORIZONTAL);

		Flip();
		Jump();
		ChangeAnimation();
		Weapon.Instance.WeaponUpdate();
	}

	private void FixedUpdate ()
	{
		if(PauseManager.Instance.isPaused || GameManager.Instance.gameOver || !GameManager.Instance.hasStarted)
		{
			return;
		}

		transform.localPosition += Vector3.right * (m_axisX * m_speed * Time.fixedDeltaTime);
	}

	private void Flip ()
	{
		if((m_axisX < 0.0f && !spriteRenderer.flipX) || (m_axisX > 0.0f && spriteRenderer.flipX))
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}
	}

	private void Jump ()
	{
		if(!m_canJump)
		{
			return;
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			m_audioSource.Stop();
			m_audioSource.loop = false;
			m_audioSource.clip = m_sounds[1];
			m_audioSource.Play();

			m_canJump = false;
			rigidbody2D.velocity = Constantes.VECTOR_TWO;
			rigidbody2D.AddForce(JUMP_FORCE, ForceMode2D.Impulse);

			m_animationController.PlayByType(ANIMATION_TYPE.JUMP);
		}
	}

	private void Death ()
	{
		m_audioSource.Stop();
		m_audioSource.loop = false;
		m_audioSource.clip = m_sounds[3];
		m_audioSource.Play();
	}

	private void ChangeAnimation ()
	{
		if(m_animationController.currentAnimation.type == ANIMATION_TYPE.IDLE && m_axisX != 0.0f)
		{
			PlayAudioMove();
			m_animationController.PlayByType(ANIMATION_TYPE.MOVE);
		}
		else if(m_animationController.currentAnimation.type == ANIMATION_TYPE.MOVE && Mathf.Approximately(m_axisX, 0.0f))
		{
			m_animationController.PlayByType(ANIMATION_TYPE.IDLE);

			m_audioSource.loop = false;
			m_audioSource.Stop();
		}
	}

	private void PlayAudioMove ()
	{
		m_audioSource.Stop();
		m_audioSource.loop = true;
		m_audioSource.clip = m_sounds[0];
		m_audioSource.Play();
	}

	private void OnCollisionStay2D (Collision2D other)
	{
		if(!m_canJump)
		{
			if(Physics2D.OverlapCircle(transform.localPosition, 0.1f, 1<<10) == other.collider)
			{
				m_audioSource.Stop();
				m_audioSource.loop = false;
				m_audioSource.clip = m_sounds[2];
				m_audioSource.Play();

				m_canJump = true;

				if(Mathf.Approximately(m_axisX, 0.0f))
				{
					m_animationController.PlayByType(ANIMATION_TYPE.IDLE);
				}
				else
				{
					m_animationController.PlayByType(ANIMATION_TYPE.MOVE);
					Invoke("PlayAudioMove", 0.1f);
				}
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.layer == 12)
		{
			m_lifeManager.TakeDamage(1);
		}
	}

	private void OnTriggerStay2D (Collider2D other)
	{
		if(other.gameObject.layer == 13)
		{
			Box b = other.GetComponent<Box>();
			Weapon.Instance.SetWeapon(b.weaponData);
			b.Reset();
		}
	}
}
