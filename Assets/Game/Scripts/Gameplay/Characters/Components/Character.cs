using AG.Core.Pool;
using AG.Core.UI;
using AG.Gameplay.Systems;
using AG.Gameplay.Characters.Data;
using AG.Gameplay.Settings;
using MoreMountains.Feedbacks;
using SharedLib.ComponentCache;
using SharedLib.Physics;
using SharedLib.StateMachines;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
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
	public class Character : SubComponent
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
		[ShowInInspector, HideInEditorMode]
		private float _health;

		[BoxGroup("Instance")]
		[ShowInInspector, HideInEditorMode]
		private float _maxHealth;
		
		// ------------- DEGUG BOX -------------

		[SerializeField]
		[FoldoutGroup("Debug Panel"), PropertyOrder(9999)]
		private bool _showDebugPanel = true;

		[SerializeField]
		[FoldoutGroup("Debug Panel"), PropertyOrder(9999)]
		private GUIUtils.PanelPlacement _panelPosition;

		[SerializeField]
		[FoldoutGroup("Debug Panel"), PropertyOrder(9999)]
		private float _panelMargin;

		// ------------- Dependencies -------------

		[Inject] private ArenaEvents _arenaEvents;
		[Inject] private IObjectResolver _objectResolver;
		[Inject] private GameSettings _gameSettings;

		// ------------- Components --------------------

		private ColliderListener[] _colliderListeners;
		private IDebugPanelDrawer[] _debugPanelDrawers;

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
		
		[BoxGroup("Instance")]
		[ShowInInspector, HideInEditorMode]
		public StateId StateId => _stateMachine.CurrentStateId;

		// ------------- Private fields -------------

		private readonly StateMachine _stateMachine = new();


		protected void Awake()
		{
			transform.IsChildOf(transform.parent);
			_health = _characterStats.MaxHealth;
			_maxHealth = _characterStats.MaxHealth;

#if UNITY_EDITOR
			// VContainer injection for prefabs dropped to the scene from the project window
			PlayModeAutoInject();
#endif

			_colliderListeners = Root.GetAll<ColliderListener>().ToArray();
			_debugPanelDrawers = Root.GetAll<IDebugPanelDrawer>().ToArray();

			IState[] states = Root.GetAll<IState>().ToArray();
			foreach (IState state in states)
			{
				_stateMachine.AddState(state);
			}
		}

		// -------- Poolable ---------------------------------------

		protected virtual void OnEnable()
		{
			Subscribe();
		}

		protected virtual void OnDisable()
		{
			Unsubscribe();
		}

		public void ReleaseToPool()
		{
			Root.Get<PooledGameObject>().ReleaseToPool();
		}
		
		// ------- Unity events -------------------

		private void Update()
		{
			_stateMachine.Update();
		}
		
		// ----------------------------------------------------------

		private void Subscribe()
		{
			foreach (var colliderListener in _colliderListeners)
			{
				colliderListener.OnMouseDownEvent += OnMouseDown;
			}
			_stateMachine.OnStateFinishedWithoutTransition += OnStateFinishedWithoutTransition;
			_stateMachine.OnStateTransition += OnStateTransition;
		}

		private void Unsubscribe()
		{
			foreach (var colliderListener in _colliderListeners)
			{
				colliderListener.OnMouseDownEvent -= OnMouseDown;
			}
			_stateMachine.OnStateFinishedWithoutTransition -= OnStateFinishedWithoutTransition;
			_stateMachine.OnStateTransition -= OnStateTransition;
		}

		private void OnMouseDown()
		{
			_showDebugPanel = !_showDebugPanel;
		}
		
		// ------------ States management ---------
		
		public void SetState(StateId id)
		{
			_stateMachine.SetState(id);
		}

		[Button("Spawn", ButtonSizes.Large), ResponsiveButtonGroup("Actions"), PropertyOrder(DebugUI.Order)]
		public void Spawn()
		{
			SetState(_characterStates.SpawningState);
		}

		[Button("Cinematic", ButtonSizes.Large), ResponsiveButtonGroup("Actions"), PropertyOrder(DebugUI.Order)]
		public void Cinematic()
		{
			SetState(_characterStates.CinematicState);
		}

		[Button("Fight", ButtonSizes.Large), PropertyOrder(DebugUI.Order)]
		public void Fight()
		{
			SetState(_characterStates.CombatState);
		}

		public bool IsFighting => StateId == _characterStates.CombatState;

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
			if (!_gameSettings.CheatSettings.ShowCharacterDebugPanel)
			{
				return;
			}
			
			if (!_showDebugPanel)
			{
				return;
			}

			List<GUIUtils.Property> properties;
			properties = new() {
				new GUIUtils.Property ("Name", name),
				new GUIUtils.Property ("State", _stateMachine.CurrentStateId?.name ?? "no-state"),
			};


			foreach (IDebugPanelDrawer drawer in _debugPanelDrawers)
			{
				drawer.AddDebugProperties(properties);
			}

			GUIUtils.DrawDebugPanel(properties, transform, _panelPosition, _panelMargin, () => _showDebugPanel = false);
		}

#if UNITY_EDITOR
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