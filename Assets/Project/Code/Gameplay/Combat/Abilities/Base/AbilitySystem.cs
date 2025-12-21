using Project.Code.Core.Data.ScriptableObjects;
using Project.Code.Gameplay.Player;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.Abilities.Base
{
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerEvolutionComponent))]
    public class AbilitySystem : MonoBehaviour
    {
        #region Serialized Fields
        
        [Header("Abilities")]
        [SerializeField] private AbilityData basicAbility;
        
        #endregion

        #region Fields

        private float _lastBasicAbilityTime = -999f;
        private float _lastSpecialAbilityTime = -999f;

        private PlayerInputHandler _inputHandler;
        private PlayerEvolutionComponent _playerEvolutionComponent;
        
        #endregion

        #region Unity Functions

        private void Awake()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
            _playerEvolutionComponent = GetComponent<PlayerEvolutionComponent>();
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
        
        #endregion

        #region Private Methods

        private void TryUseBasicAbility()
        {
            if (basicAbility == null) return;
            if (!(Time.time >= _lastBasicAbilityTime + basicAbility.Cooldown)) return;
            
            basicAbility.Use(gameObject);
            _lastBasicAbilityTime = Time.time;
        }

        private void TryUseSpecialAbility()
        {
            if (!_playerEvolutionComponent)
            {
                Debug.LogError("Player has no evolution component is null");
            }

            if (!(Time.time >= _lastSpecialAbilityTime + _playerEvolutionComponent.GetCurrentEvolutionCooldown()))
            {
                Debug.LogWarning("Special ability is in cooldown");
                return;
            }
            
            _playerEvolutionComponent.UseSpecialAbility();
            _lastSpecialAbilityTime = Time.time;
        }
        
        #endregion
    }
}
