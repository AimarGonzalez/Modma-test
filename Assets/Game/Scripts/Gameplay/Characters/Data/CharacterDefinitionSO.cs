using AG.Gameplay.Characters;
using UnityEngine;

namespace AG.Gameplay.Characters.Data
{
	[CreateAssetMenu(fileName = "CharacterDefinition", menuName = "AG/Character/Character Definition")]
	public class CharacterDefinitionSO : ScriptableObject
	{
		[SerializeField]
		private Character _prefab;

		public GameObject Prefab => _prefab.gameObject;
	}
}