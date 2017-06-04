using UnityEngine;
using RGS.Animation;
using System.Collections;

public enum ENEMY_STATE
{
	IDLE,
	MOVE,
	DEATH
}

public class Enemy : MonoBehaviour
{
	[System.Serializable]
	public struct Data
	{
		public string name;

		public int life;
		public int points;

		public float scale;
		public float speed;

		public AnimationData animationData;
	}

	private Enemy.Data m_data;
	public Enemy.Data data
	{
		get
		{
			return m_data;
		}

		set
		{
			m_data = value;
		}
	}

	private bool m_dieBylevel = false;
	private bool m_facingRight = true;

	private ENEMY_STATE m_state = ENEMY_STATE.IDLE;

	private Vector3 direction { get { return m_facingRight ? Vector3.right : Vector3.left; } }

	[SerializeField]
	private AudioClip[] m_sounds = null;

	[SerializeField]
	private AudioSource m_audioSource = null;

	[SerializeField]
	private Transform m_facePoint = null;

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

	private void FixedUpdate ()
	{
		if(PauseManager.Instance.isPaused || !GameManager.Instance.hasStarted || GameManager.Instance.gameOver)
		{
			return;
		}

		if(m_state == ENEMY_STATE.MOVE)
		{
			transform.localPosition += direction * (m_data.speed * Time.fixedDeltaTime);
		}
	}

	public void SetInformations (Enemy.Data newData, bool facingRight)
	{
		data = newData;

		m_dieBylevel = false;

		transform.localScale = new Vector3(m_data.scale, m_data.scale, 1.0f);

		m_animationController.animations = m_data.animationData.animationsList;

		m_lifeManager = new LifeManager(m_data.life);
		m_lifeManager.Death += Death;

		if(m_facingRight != facingRight)
		{
			Flip();
		}

		SetState(ENEMY_STATE.MOVE);

		gameObject.SetActive(true);
	}

	public void SetState(ENEMY_STATE newState)
	{
		m_state = newState;

		ANIMATION_TYPE t = ANIMATION_TYPE.IDLE;

		switch(m_state)
		{
			case ENEMY_STATE.DEATH:
				t = ANIMATION_TYPE.JUMP;

				m_audioSource.Stop();
				m_audioSource.loop = false;
				m_audioSource.clip = m_sounds[3];
				m_audioSource.Play();
				break;

			case ENEMY_STATE.MOVE:
				t = ANIMATION_TYPE.MOVE;
				PlayAudioMove();
				break;

			default:
				m_audioSource.Stop();
				break;
		}

		m_animationController.PlayByType(t);
	}

	public void Flip ()
	{
		m_facingRight = !m_facingRight;

		m_animationController.spriteRenderer.flipX = !m_facingRight;

		m_facePoint.localPosition = new Vector3(m_facingRight ? 0.35f : -0.35f , m_facePoint.localPosition.y, 0.0f);
	}

	private void Death ()
	{
		gameObject.SetActive(false);

		WaveManager.Instance.UpdateKilledEnemys();

		if(m_dieBylevel)
		{
			return;
		}

		ScoreManager.Instance.AddPoints(m_data.points);
	}

	private void PlayAudioMove ()
	{
		m_audioSource.Stop();
		m_audioSource.loop = true;
		m_audioSource.clip = m_sounds[0];
		m_audioSource.Play();
	}

	public void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.layer == 12)
		{
			m_dieBylevel = true;
			m_lifeManager.TakeDamage(1);
		}
	}

	private void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject == Character.Instance.gameObject)
		{
			m_audioSource.Stop();
			m_audioSource.loop = false;
			m_audioSource.clip = m_sounds[2];
			m_audioSource.Play();

			Character.Instance.lifeManager.TakeDamage(1);

			WaveManager.Instance.StopSpawn();
		}
		else
		{
			m_audioSource.Stop();
			m_audioSource.loop = false;
			m_audioSource.clip = m_sounds[1];
			m_audioSource.Play();

			Invoke("PlayAudioMove", 0.1f);
		}
	}
}
