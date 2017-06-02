#region Namespaces
using UnityEngine.Events;
using UnityEngine;

using System.Linq;
using System;
#endregion

namespace RGS.Animation
{
	#region Data

	[Serializable]
	public class Data
	{
		public string name = "";

		[ContextMenuItem("Sort Sprites By Name", "DoSort"), Tooltip("Use names like: Sprite_001 or Sprite_01")]
		public Sprite[] frames = null;

		public bool loop = false;

		[Range(10, 60)]
		public int fps = 24;

		public ANIMATION_TYPE type = ANIMATION_TYPE.NULL;

		/// <summary>
		/// Set This Enum By Code According Your Necessity.
		/// </summary>
		public Enum genericType;

		/// <summary>
		/// Event Called When The Animation Start.
		/// </summary>
		public OnStartAnimation onStartAnimation = null;
		public delegate void OnStartAnimation();

		/// <summary>
		/// Event Called When The Animation Has Changed.
		/// </summary>
		public OnChangeAnimation onChangeAnimation = null;
		public delegate void OnChangeAnimation();

		/// <summary>
		/// Event Called When The Animation Ended and Will Loop.
		/// </summary>
		public OnLoopAnimation onLoopAnimation = null;
		public delegate void OnLoopAnimation();

		/// <summary>
		/// Event Called When The Animation Ended and Will Not Loop.
		/// </summary>
		public OnCompleteAnimation onCompleteAnimation = null;
		public delegate void OnCompleteAnimation();

		[Header("Event Called When The Animation Ended and Will Not Loop")]
		public UnityEvent completeAnimationEvent = null;
		[Header("Event Called When The Animation Has Changed")]
		public UnityEvent changeAnimationEvent = null;
		[Header("Event Called When The Animation Start")]
		public UnityEvent startAnimationEvent = null;
		[Header("Event Called When The Animation Ended and Will Loop")]
		public UnityEvent loopAnimationEvent = null;
	}

	#endregion

	[CreateAssetMenu(fileName = "AnimationData", menuName = "RGS/Animation Data", order = -1)]
	public class AnimationData : ScriptableObject
	{
		[SerializeField]
		private AnimationController m_animationController = null;

		[SerializeField]
		private Data[] m_animations = null;

		[ContextMenu("Copy Data")]
		private void Fill ()
		{
			m_animations = m_animationController.animations.ToArray();
		}

		[ContextMenu("Paste Data")]
		private void Transfer ()
		{
			m_animationController.animations = m_animations.ToList();
		}
	}
}
