using AG.Gameplay.Characters.Data;
using System;
using UnityEngine;

namespace AG.Gameplay.Levels
{
	[CreateAssetMenu(fileName = "LevelDefinition", menuName = "AG/LevelDefinition")]
	public class LevelDefinitionSO : ScriptableObject
	{
		[Serializable]
		public class Wave
		{
			public CharacterDefinitionSO[] CharacterDefinitions;
		}

		public Wave[] Waves;
	}
}