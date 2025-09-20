using AG.Gameplay.Systems;
using AG.Gameplay.Units;
using SharedLib.ComponentCache;
using SharedLib.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace AG.Gameplay.Characters.Components
{
	public class PlayerCombatComponent : SubComponent
	{
		public enum AttackState
		{
			None,
			PlayingAttackAnimation,
			Cooldown
		}
		
		[ShowInInspector, ReadOnly]
		private AttackState _state;

		[ShowInInspector, ReadOnly]
		private Character _target;
		
		[ShowInInspector, ReadOnly]
		private float _attackSpeed;

		private Timer _timer;

		public AttackState State
		{
			get => _state;
			set => _state = value;
		}

		public Character Target
		{
			get => _target;
			set
			{
				_target = value;
			}
		}
		
		public bool HasTarget => _target != null;

		public float Cooldown => _timer?.TimeLeft ?? 0f;

		[Inject]
		private ArenaEvents _arenaEvents;

		public bool IsPlayingAnimation => _state == AttackState.PlayingAttackAnimation;
		

		protected void Awake()
		{
			_timer = new Timer();
			_timer.OnFinished += OnCooldownFinished;
		}

		private void Update()
		{
			_timer.Elapsed(Time.deltaTime);
		}

		public void Attack()
		{
			switch (_state)
			{
				case AttackState.None:
					_state = AttackState.PlayingAttackAnimation;
					StartCooldown();


					//Play animation
					//Listen for animation event

					OnAnimationHitEvent();
					_state = AttackState.Cooldown;

					break;

				case AttackState.PlayingAttackAnimation:
				case AttackState.Cooldown:
					// do nothing
					break;

				default:
					Debug.LogError($"Unknown attack state: {_state}");
					break;
			}
		}

		private void OnAnimationHitEvent()
		{
			// TODO: Move to Action so we can differentiate between mele and ranged actions. 
			/*
			if (Character.CombatStats.IsMelee)
			{
				_arenaEvents.TriggerCharacterAttacked(Character, _target);
			}
			else
			{
				_arenaEvents.TriggerProjectileFired(Character, _target);
			}
			*/
		}

		private void StartCooldown()
		{
			_timer.Start(_attackSpeed);
		}

		private void OnCooldownFinished()
		{
			_state = AttackState.None;
		}
	}
}

