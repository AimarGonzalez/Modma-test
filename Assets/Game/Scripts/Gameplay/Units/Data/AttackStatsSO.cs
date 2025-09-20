using UnityEngine;
using Sirenix.OdinInspector;

namespace AG.Gameplay.Cards.CardStats
{
	public enum AttackType
	{
		Melee,
		Ranged,
	}
	
	[CreateAssetMenu(fileName = "AttackStats", menuName = "AG/Character/AttackStats")]
	public class AttackStatsSO : ScriptableObject
	{
		[Tooltip("Type of attack")]
		[SerializeField]
		private AttackType _attackType;

		[Tooltip("Base damage per attack")]
		[SerializeField]
		private float _damage;

		[Tooltip("Time between attacks in seconds")]
		[SerializeField]
		private float _attackSpeed;

		[Tooltip("Range of attack")]
		[SerializeField]
		private float _attackRange;

		[Tooltip("Range of sight")]
		[SerializeField]
		private float _sightRange;

		[Tooltip("Area damage radius (0 for single target)")]
		[SerializeField]
		private float _areaDamageRadius;

		[Tooltip("Speed of projectile")]
		[SerializeField, ShowIf(nameof(IsRanged))]
		private float _projectileSpeed = 1000f;


		// Public getters for properties
		public AttackType AttackType => _attackType;
		public float Damage => _damage;
		public float AttackRange => _attackRange;
		public float AreaDamageRadius => _areaDamageRadius;
		public float SightRange => _sightRange;
		public float Cooldown => _attackSpeed;
		public float ProjectileSpeed => _projectileSpeed;

		// ------------------------------
		public bool IsRanged => _attackType == AttackType.Ranged;
		public bool IsMelee => _attackType == AttackType.Melee;
	}
}