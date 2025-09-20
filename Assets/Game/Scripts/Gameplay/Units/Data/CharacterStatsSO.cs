using UnityEngine;

namespace AG.Gameplay.Units.Data
{
	[CreateAssetMenu(fileName = "CharacterStats", menuName = "AG/Character/CharacterStats")]
	public class CharacterStatsSO : ScriptableObject
	{
		[SerializeField]
		private string _name;

		[SerializeField]
		private int _maxHealth;

		[SerializeField]
		private int _movementSpeed;

		public string Name => _name;
		public int MaxHealth => _maxHealth;
		public int MovementSpeed => _movementSpeed;
	}
}