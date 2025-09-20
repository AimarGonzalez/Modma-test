using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AG.Gameplay
{
    [Serializable]
    public class Flags : MonoBehaviour
    {
        [ReadOnly, ShowInInspector]
        private Dictionary<FlagSO, int> _flags = new();

        public bool IsAnyFlagActive(IEnumerable<FlagSO> targetFlags)
        {
            foreach (FlagSO targetFlag in targetFlags)
            {
                if (IsFlagActive(targetFlag))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsFlagActive(FlagSO flagSO)
        {
            return _flags.TryGetValue(flagSO, out int value) && value > 0;
        }

        public void RaiseFlags(List<FlagSO> actionFlags)
        {
            foreach (FlagSO flag in actionFlags)
            {
                if (_flags.ContainsKey(flag))
                {
                    _flags[flag]++;
                }
                else
                {
                    _flags[flag] = 1;
                }
            }
        }

        public void LowerFlags(List<FlagSO> flags)
        {
            foreach (var flag in flags)
            {
                if (_flags.ContainsKey(flag))
                {
                    _flags[flag]--;
                }
                else
                {
                    Debug.LogError($"Failed to lower flag {flag.name}. Flag not found.");
                }
            }
        }

        public void Reset()
        {
            _flags.Clear();
        }
    }
}