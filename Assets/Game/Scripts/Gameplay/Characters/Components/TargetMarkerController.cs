using SharedLib.ComponentCache;
using System;
using UnityEngine;

namespace AG.Gameplay.Characters.Components
{
	public class TargetMarkerController : SubComponent
	{
		// ------------- Inspector fields -------------
		[SerializeField]
		private Transform _targetMarker;

		// ------------- Dependencies -------------
		private PlayerCombatState _combatState;

		// ------------- Private fields -------------
		private Character _target;

		private void Awake()
		{
			_combatState = Root.GetComponent<PlayerCombatState>();
		}

		private void OnEnable()
		{
			_combatState.OnTargetChanged += OnTargetChanged;
		}

		private void OnDisable()
		{
			_combatState.OnTargetChanged -= OnTargetChanged;
		}

		private void OnTargetChanged(Character target)
		{
			_target = target;
			UpdateView();
		}

		private void Update()
		{
			UpdateView();
		}

		private void UpdateView()
		{
			if (_target == null)
			{
				_targetMarker.gameObject.SetActive(false);
			}
			else
			{
				_targetMarker.gameObject.SetActive(true);
				
				_targetMarker.position = new Vector3(
					_target.transform.position.x,
					_targetMarker.position.y, // keep original heigh
					_target.transform.position.z);
			}
		}
	}
}

