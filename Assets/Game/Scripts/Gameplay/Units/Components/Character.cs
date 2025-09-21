using AG.Core.Pool;
using AG.Core.UI;
using AG.Gameplay.Systems;
using AG.Gameplay.Units;
using AG.Gameplay.Units.Data;
using MoreMountains.Feedbacks;
using SharedLib.ComponentCache;
using SharedLib.Physics;
using SharedLib.StateMachines;
using Sirenix.OdinInspector;
using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace AG.Gameplay.Characters
{
	[DisallowMultipleComponent]
	[DebuggerDisplay("name: {name}, state: {StateId}, team: {_team}, health: {_health}/{_maxHealth}")]
	public class Character : SubComponent, IPooledComponent
	{
		[Serializable]
		public struct CharacterStates
		{
			[SerializeField]
			public StateId SpawningState;

			[SerializeField]
			public StateId CinematicState;

			[SerializeField]
			public StateId CombatState;
		}

		// ------------- Events -------------
		public event Action<StateId, StateId> OnStateChanged;
		public event Action<float, float> OnHealthChanged;
		public event Action<Character, float> OnHitReceived;


		// ------------- Inspector fields -------------	

		[SerializeField]
		private Team _team;

		[SerializeField]
		private CharacterStates _characterStates;

		[FormerlySerializedAs("_stats")]
		[SerializeField, Required, InlineEditor]
		private CharacterStatsSO _characterStats;

		[SerializeField, InlineEditor]
		private MMF_Player _spawnningFeedbacks;

		[BoxGroup("Instance")]
		[ShowInInspector]
		private float _health;

		[BoxGroup("Instance")]
		private float _maxHealth;

		// ------------- DEGUG BOX -------------

		[SerializeField]
		[BoxGroup("Debug Panel"), PropertyOrder(9999)]
		private bool _showDebugPanel = true;

		[SerializeField]
		[BoxGroup("Debug Panel"), PropertyOrder(9999)]
		private GUIUtils.PanelPlacement _panelPosition;

		[SerializeField]
		[BoxGroup("Debug Panel"), PropertyOrder(9999)]
		private float _panelMargin;

		// ------------- Dependencies -------------

		[Inject]
		private ArenaEvents _arenaEvents;

		[Inject]
		private IObjectResolver _objectResolver;

		// ------------- Components --------------------

		private ColliderListener _colliderListener;

		// ------------- Public properties -------------

		public Team Team => _team;
		public bool IsPlayer => _team == Team.Player;
		public bool IsEnemy => _team == Team.Enemy;
		public float Health
		{
			get => _health;
			set
			{
				float previousHealth = _health;
				_health = value;
				OnHealthChanged?.Invoke(previousHealth, _health);
			}
		}

		public float MaxHealth => _maxHealth;
		public CharacterStatsSO CharacterStats => _characterStats;
		public StateId StateId => _stateMachine.CurrentStateId;

		// ------------- Private fields -------------

		private StateMachine _stateMachine;


		protected void Awake()
		{
			transform.IsChildOf(transform.parent);
			_health = _characterStats.MaxHealth;
			_maxHealth = _characterStats.MaxHealth;

#if UNITY_EDITOR
			// VContainer injection for prefabs dropped to the scene from the project window
			PlayModeAutoInject();
#endif

			_colliderListener = Root.Get<ColliderListener>();

			IState[] states = Root.GetAll<IState>().ToArray();
			foreach (IState state in states)
			{
				_stateMachine.AddState(state);
			}
		}

		private void OnDestroy()
		{
			(this as IPooledComponent).OnReturnToPool();
		}

		// -------- Poolable ---------------------------------------
		void IPooledComponent.OnBeforeGetFromPool()
		{
		}
		void IPooledComponent.OnAfterGetFromPool()
		{
			Subscribe();
		}

		[Button("Spawn", ButtonSizes.Large), PropertyOrder(DebugUI.Order)]
		public void Spawn()
		{
			_stateMachine.SetState(_characterStates.SpawningState);
		}

		[Button("Cinematic", ButtonSizes.Large), PropertyOrder(DebugUI.Order)]
		public void Cinematic()
		{
			_stateMachine.SetState(_characterStates.CinematicState);
		}

		[Button("Fight", ButtonSizes.Large)]
		public void Fight()
		{
			_stateMachine.SetState(_characterStates.CombatState);
		}

		void IPooledComponent.OnReturnToPool()
		{
			Unsubscribe();
		}
		void IPooledComponent.OnDestroyFromPool()
		{
		}
		// ----------------------------------------------------------

		private void Subscribe()
		{
			_colliderListener.OnMouseDownEvent += OnMouseDown;
			_stateMachine.OnStateFinishedWithoutTransition += OnStateFinishedWithoutTransition;
			_stateMachine.OnStateTransition += OnStateTransition;
		}

		private void Unsubscribe()
		{
			_colliderListener.OnMouseDownEvent -= OnMouseDown;
			_stateMachine.OnStateFinishedWithoutTransition -= OnStateFinishedWithoutTransition;
			_stateMachine.OnStateTransition -= OnStateTransition;
		}

		private void OnMouseDown()
		{
			_showDebugPanel = !_showDebugPanel;
		}

		protected virtual void OnStateFinishedWithoutTransition(StateId finishedState)
		{
			if (finishedState == _characterStates.SpawningState || finishedState == _characterStates.CinematicState)
			{
				_stateMachine.SetState(_characterStates.CombatState);
			}
		}

		private void OnStateTransition(StateId prevState, StateId newState)
		{
			OnStateChanged?.Invoke(prevState, newState);
		}

		public void Hit(Character source, float damage)
		{
			OnHitReceived?.Invoke(source, damage);
		}

#if UNITY_EDITOR
		// VContainer injection for prefabs dropped to the scene from the project window
		private void PlayModeAutoInject()
		{
			// Inject only in playMode
			if (!Application.isPlaying)
			{
				return;
			}

			if (_objectResolver == null)
			{
				_objectResolver = FindFirstObjectByType<GameBootstrap>().Container;
			}

			_objectResolver.InjectGameObject(gameObject);
		}
#endif

		void OnGUI()
		{
			DrawDebugGUI();
		}

		private void DrawDebugGUI()
		{
			if (!_showDebugPanel)
			{
				return;
			}

			GUIUtils.Property[] properties;
			properties = new[] {
				new GUIUtils.Property ("Name", name),
				new GUIUtils.Property ("Target", "None"),
				new GUIUtils.Property ("Flags", "todo")
			};

			GUIUtils.DrawDebugPanel(properties, transform, _panelPosition, _panelMargin, () => _showDebugPanel = false);
		}

#if UNITY_EDITOR
		[Button("DEPRECATED - ForceInitializeCharacter")]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order - 1)]
		private void ForceInitializeCharacter()
		{
			//ForceAwakeSubComponents();
		}

		/*
		private void ForceAwakeSubComponents()
		{
			foreach (var component in CharacterComponents)
			{
				component.ForceAwake(this);
			}
		}
		*/
#endif //UNITY_EDITOR
	}
}