using Project.Code.Gameplay.Combat.Abilities.PrimaryAbilities;
using Project.Code.Gameplay.Player;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.Abilities.Base
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class AbilitySystem : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField] private Ability basicAbility;
        [SerializeField] private Ability specialAbility;

        private float _lastBasicAbilityTime = -999f;
        private float _lastSpecialAbilityTime = -999f;

        private PlayerInputHandler _inputHandler;

        private void Awake()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
            if (!basicAbility) basicAbility = ScriptableObject.CreateInstance<ScratchAbility>();
        }

        private void OnEnable()
        {
            _inputHandler.OnSimpleAbilityPerformed += TryUseBasicAbility;
            _inputHandler.OnSpecialAbilityPerformed += TryUseSpecialAbility;

        }

        private void OnDisable()
        {
            _inputHandler.OnSimpleAbilityPerformed -= TryUseBasicAbility;
            _inputHandler.OnSpecialAbilityPerformed -= TryUseSpecialAbility;
        }

        private void TryUseBasicAbility()
        {
            if (basicAbility == null) return;
            if (!(Time.time >= _lastBasicAbilityTime + basicAbility.Cooldown)) return;
            
            basicAbility.Activate(gameObject);
            _lastBasicAbilityTime = Time.time;
        }

        private void TryUseSpecialAbility()
        {
            if (specialAbility == null) return;
            if (!(Time.time >= _lastSpecialAbilityTime + specialAbility.Cooldown)) return;
            
            specialAbility.Activate(gameObject);
            _lastSpecialAbilityTime = Time.time;
        }
    }
}
