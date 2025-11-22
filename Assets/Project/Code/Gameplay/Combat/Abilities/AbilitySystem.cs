using Project.Code.Gameplay.Combat.Abilities.Base;
using Project.Code.Gameplay.Player;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.Abilities
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class AbilitySystem : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField] private Ability basicAttack;
        [SerializeField] private Ability specialAttack;

        private float _lastBasicAttackTime = -999f;
        private float _lastSpecialAttackTime = -999f;

        private PlayerInputHandler _inputHandler;

        private void Awake()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void OnEnable()
        {
            _inputHandler.OnSimpleAttackPerformed += TryUseBasicAttack;
            _inputHandler.OnSpecialAttackPerformed += TryUseSpecialAttack;
        }

        private void OnDisable()
        {
            _inputHandler.OnSimpleAttackPerformed -= TryUseBasicAttack;
            _inputHandler.OnSpecialAttackPerformed -= TryUseSpecialAttack;
        }

        private void TryUseBasicAttack()
        {
            if (basicAttack == null) return;
            if (!(Time.time >= _lastBasicAttackTime + basicAttack.Cooldown)) return;
            
            basicAttack.Activate(gameObject);
            _lastBasicAttackTime = Time.time;
        }

        private void TryUseSpecialAttack()
        {
            if (specialAttack == null) return;
            if (!(Time.time >= _lastSpecialAttackTime + specialAttack.Cooldown)) return;
            
            specialAttack.Activate(gameObject);
            _lastSpecialAttackTime = Time.time;
        }
    }
}
