using AG.Core.Pool;
using AG.Core.UI;
using AG.Gameplay.Systems;
using AG.Gameplay.Units;
using AG.Gameplay.Units.Data;
using Modma.Game.Scripts.Gameplay.Units.Components;
using SharedLib.ComponentCache;
using Sirenix.OdinInspector;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace AG.Gameplay.Characters
{
	// Add ExecuteInEditMode so OnGUI draws the debug panel in the editor
	[DisallowMultipleComponent]
	[DebuggerDisplay("name: {name}, state: {_state}, team: {_team}, health: {_health}/{_maxHealth}")]
	public class Character : SubComponent, IPooledComponent
	{
		[SerializeField]
		private Team _team;
		[SerializeField]
		private CharacterState _state;
		[FormerlySerializedAs("_stats")]
		[SerializeField, Required, InlineEditor]
		private CharacterStatsSO _characterStats;

		[BoxGroup("Instance")]
		[ShowInInspector]
		private int _health;

		[BoxGroup("Instance")]
		private int _maxHealth;

		// ------------- DEGUG BOX -------------

		[SerializeField]
		[BoxGroup("Debug Panel"), PropertyOrder(9999)]
		private bool _showDebugPanel = false;

		[SerializeField]
		[BoxGroup("Debug Panel"), PropertyOrder(9999)]
		private GUIUtils.PanelPlacement _panelPosition;

		[SerializeField]
		[BoxGroup("Debug Panel"), PropertyOrder(9999)]
		private float _panelMargin = 0f;

		// ------------- Dependencies -------------

		[Inject]
		private ArenaEvents _arenaEvents;

		[Inject]
		private IObjectResolver _objectResolver;

		// ------------- Public properties -------------

		public Team Team => _team;
		public bool IsPlayer => _team == Team.Player;
		public bool IsEnemy => _team == Team.Enemy;
		public int Health => _health;
		public int MaxHealth => _maxHealth;
		public CharacterStatsSO CharacterStats => _characterStats;


		protected void Awake()
		{
			transform.IsChildOf(transform.parent);
			_health = _characterStats.MaxHealth;
			_maxHealth = _characterStats.MaxHealth;

#if UNITY_EDITOR
			// VContainer injection for prefabs added from the editor.
			AutoInject();
#endif

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				//ForceAwakeSubComponents();
			}
#endif
		}

		private void OnDestroy()
		{
			(this as IPooledComponent).OnReturnToPool();
		}

		// -------- Poolable ---------------------------------------
		void IPooledComponent.OnBeforeGetFromPool()
		{
			Reset();
		}
		void IPooledComponent.OnAfterGetFromPool()
		{
			Subscribe();
		}
		void IPooledComponent.OnReturnToPool()
		{
			Unsubscribe();
			NotifyCharacterRemoved();
		}
		void IPooledComponent.OnDestroyFromPool()
		{
		}
		// ----------------------------------------------------------

		public void Reset()
		{
		}

		public void NotifyCharacterRemoved()
		{
			_arenaEvents.TriggerCharacterRemoved(this);
		}

		private void Subscribe()
		{
		}

		private void Unsubscribe()
		{
		}

		public void SetState(CharacterState newState)
		{
			_state = newState;
		}

		private void OnMouseDown()
		{
			_showDebugPanel = !_showDebugPanel;
		}

#if UNITY_EDITOR
		// VContainer injection for prefabs dropped to the scene from the project window
		private void AutoInject()
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

		void OnDrawGizmos()
		{
			// Handles.BeginGUI();
			// DrawDebugGUI();
			// Handles.EndGUI();
		}

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