#region Namespaces
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System;
using System.Collections;
using System.Collections.Generic;

using RGS.Animation;
#endregion

/// <summary>
/// Set This Enumeration According To Your Necessity But Prefer Use GenericType
/// </summary>
public enum ANIMATION_TYPE
{
	NULL = -1,

	IDLE,
	MOVE,
	JUMP,
	ATTACK
}

public class AnimationController : MonoBehaviour
{
	/// <summary>
	/// Set This Array According To Your Necessity
	/// </summary>
	public static readonly string[] ANIMATIONS_NAMES = new string[]
	{
		"Idle",
		"Move",
		"Jump",
		"Fall",
		"Intro",
		"Death",
		"Split",
		"Attack"
	};

	#region Variables & Properties

	[SerializeField]
	private bool m_unscaledAnimation = false;

	[SerializeField, ContextMenuItem("Set Sprite", "SetSprite")]
	protected List<Data> m_animations = new List<Data>();
	public List<Data> animations
	{
		get { return m_animations; }
		set { m_animations = value; }
	}

	public delegate void OnChangeAnimation();
	public OnChangeAnimation onChangeAnimation = null;

	protected Data m_currentAnimation = null;
	public Data currentAnimation
	{
		get
		{
			return m_currentAnimation;
		}
	}

	protected float m_secondsPerFrame = 0.0f;
	protected float m_nextFrameTime = 0.0f;
	protected float m_time
	{
		get
		{
			return m_unscaledAnimation ? Time.unscaledTime : Time.time;
		}
	}

	public float duration
	{
		get
		{
			return m_currentAnimation.frames.Length * m_currentAnimation.fps;
		}

		set
		{
			m_currentAnimation.fps = (int)(value / m_currentAnimation.frames.Length);
		}
	}

	protected int m_currentFrame = 0;
	public int currentFrame
	{
		get
		{
			return m_currentFrame;
		}
	}

	protected bool m_playing = false;
	public bool isPlaying
	{
		get
		{
			return m_playing;
		}
	}

	protected bool m_done = false;
	public bool isDone
	{
		get
		{
			return m_done;
		}
	}

	#endregion

	#region Editor Method

	private void DoSort ()
	{
		foreach (Data anim in m_animations)
		{
			System.Array.Sort(anim.frames, (a, b) => a.name.CompareTo(b.name));
		}
	}

	private void SetSprite ()
	{
		if(m_animations[0].frames == null || m_animations[0].frames.Length == 0)
		{
			Debug.LogWarning("Você não têm frames adicionados nessa animação!");
			return;
		}
	}

	#endregion

	#region Unity Methods

	protected virtual void Update ()
	{
		Animation();
	}

	#endregion

	#region Gets Methods

	public Data GetAnimationByGenericType (Enum genericType)
	{
		return m_animations.Find(a => a.genericType == genericType);
	}

	public Data GetAnimationByType (ANIMATION_TYPE type)
	{
		return m_animations.Find(a => a.type == type);
	}

	public Data GetAnimationByName (string name)
	{
		return m_animations.Find(a => a.name == name);
	}

	#endregion
		
	#region Play Methods

	public void PlayByGenericType (Enum genericType)
	{
		PlayByIndex(m_animations.FindIndex(a => a.genericType == genericType));
	}

	public void PlayByType (ANIMATION_TYPE type)
	{
		PlayByIndex(m_animations.FindIndex(a => a.type == type));
	}

	public void PlayByName (string name)
	{
		PlayByIndex(m_animations.FindIndex(a => a.name == name));
	}
	
	public void PlayByIndex (int index)
	{
		if (index < 0)
		{
			return;
		}

		if(m_currentAnimation != null)
		{
			if(m_currentAnimation.onChangeAnimation != null)
			{
				m_currentAnimation.onChangeAnimation();
			}

			if(m_currentAnimation.changeAnimationEvent.GetPersistentEventCount() > 0)
			{
				m_currentAnimation.changeAnimationEvent.Invoke();
			}

			if(onChangeAnimation != null)
			{
				onChangeAnimation();
			}
		}
		
		m_currentAnimation = m_animations[index];
		
		m_secondsPerFrame = 1.0f / m_currentAnimation.fps;
		m_nextFrameTime = m_time;
		m_currentFrame = -1;
		m_playing = true;
		m_done = false;

		if(m_currentAnimation.onStartAnimation != null)
		{
			m_currentAnimation.onStartAnimation();
		}

		if(m_currentAnimation.startAnimationEvent.GetPersistentEventCount() > 0)
		{
			m_currentAnimation.startAnimationEvent.Invoke();
		}
	}

	public void PlayByTypeWithDelay (Enum genericType, float delay)
	{
		InvokeDelay(PlayByGenericType, delay, genericType);
	}

	public void PlayByTypeWithDelay (ANIMATION_TYPE type, float delay)
	{
		InvokeDelay(PlayByType, delay, type);
	}

	public void PlayByNameWithDelay (string name, float delay)
	{
		InvokeDelay(PlayByName, delay, name);
	}

	public void PlayByIndexWithDelay (int index, float delay)
	{
		InvokeDelay(PlayByIndex, delay, index);
	}

	#endregion

	#region Animation Methods

	private void Animation ()
	{
		if (!m_playing || m_time < m_nextFrameTime)
		{
			return;
		}

		m_currentFrame++;

		if (m_currentFrame >= m_currentAnimation.frames.Length)
		{
			if (!m_currentAnimation.loop)
			{
				m_done = true;
				m_playing = false;

				if(m_currentAnimation.onCompleteAnimation != null)
				{
					m_currentAnimation.onCompleteAnimation();
				}

				if(m_currentAnimation.completeAnimationEvent.GetPersistentEventCount() > 0)
				{
					m_currentAnimation.completeAnimationEvent.Invoke();
				}

				return;
			}

			if(m_currentAnimation.onLoopAnimation != null)
			{
				m_currentAnimation.onLoopAnimation();
			}

			if(m_currentAnimation.loopAnimationEvent.GetPersistentEventCount() > 0)
			{
				m_currentAnimation.loopAnimationEvent.Invoke();
			}

			m_currentFrame = 0;
		}

		m_nextFrameTime += m_secondsPerFrame;
	}

	public void Stop ()
	{
		m_playing = false;
	}
	
	public void Resume ()
	{
		m_playing = true;
		m_nextFrameTime = m_time + m_secondsPerFrame;
	}

	#endregion

	#region Coroutines

	protected Coroutine InvokeDelay (Action<int> action, float time, int index)
	{
		return StartCoroutine(InvokeImpl(action, time, index));
	}

	protected Coroutine InvokeDelay (Action<Enum> action, float time, Enum genericType)
	{
		return StartCoroutine(InvokeImpl(action, time, genericType));
	}

	protected Coroutine InvokeDelay (Action<ANIMATION_TYPE> action, float time, ANIMATION_TYPE type)
	{
		return StartCoroutine(InvokeImpl(action, time, type));
	}

	protected Coroutine InvokeDelay (Action<string> action, float time, string name)
	{
		return StartCoroutine(InvokeImpl(action, time, name));
	}

	protected IEnumerator InvokeImpl (Action<int> action, float time, int index)
	{
		yield return new WaitForSeconds(time);

		action(index);
	}

	protected IEnumerator InvokeImpl (Action<ANIMATION_TYPE> action, float time, ANIMATION_TYPE type)
	{
		yield return new WaitForSeconds(time);

		action(type);
	}

	protected IEnumerator InvokeImpl (Action<Enum> action, float time, Enum genericType)
	{
		yield return new WaitForSeconds(time);

		action(genericType);
	}

	protected IEnumerator InvokeImpl (Action<string> action, float time, string name)
	{
		yield return new WaitForSeconds(time);

		action(name);
	}

	#endregion
}
