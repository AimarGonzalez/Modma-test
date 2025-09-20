using AG.Gameplay.Characters;
using System;

namespace AG.Gameplay.Systems
{
	public class ArenaEvents	
	{
		public event Action<Character> OnCharacterCreated;
		public event Action<Character> OnCharacterRemoved;

		public event Action<Character, Character> OnCharacterAttacked;
		public event Action<Character, Character> OnProjectileFired;
		public event Action<Character, Character> OnProjectileHit;

		public void TriggerCharacterCreated(Character character)
		{
			OnCharacterCreated?.Invoke(character);
		}

		public void TriggerCharacterRemoved(Character character)
		{
			OnCharacterRemoved?.Invoke(character);
		}

		public void TriggerCharacterAttacked(Character attacker, Character target)
		{
			OnCharacterAttacked?.Invoke(attacker, target);
		}

		public void TriggerProjectileFired(Character attacker, Character target)
		{
			OnProjectileFired?.Invoke(attacker, target);
		}

		public void TriggerProjectileHit(Character attacker, Character target)
		{
			OnProjectileHit?.Invoke(attacker, target);
		}
	}
}