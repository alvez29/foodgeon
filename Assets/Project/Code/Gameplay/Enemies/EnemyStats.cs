using Project.Code.Core;
using Project.Code.Core.Data;
using Project.Code.Core.Data.Enums;
using Project.Code.Core.Data.ScriptableObjects;
using Project.Code.Gameplay.Combat.HitFlash;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Enemies
{
    public class EnemyStats : BaseStats
    {
        [Header("Enemy Stats")]
        [SerializeField] private string enemyName;
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private Material deathMaterial;

        public string EnemyName => enemyName;
        public EnemyType EnemyType => enemyType;
        public Flavor Flavor => Constants.Stats.Enemy.GetFlavorByType(EnemyType);
        public EnemyReward EnemyReward => Constants.Stats.Enemy.GetEnemyRewardByFlavour(Flavor);
        
        public bool CanBeEaten => IsDead;
        
        private HitFlashComponent _hitFlashComponent;
        private MeshRenderer _meshRenderer;

        #region Unity Functions

        protected override void Awake()
        {
            base.Awake();
            
            _hitFlashComponent = GetComponent<HitFlashComponent>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        #endregion
        
        # region Public Methods
        public void Initialize(int depth, EnemyType type)
        {
            enemyType = type;
            var multiplier = 1f + (depth * 0.1f);
            // Logic to scale stats based on depth could go here
            
            // Optionally update name or visual based on type if needed
            gameObject.name = $"Enemy_{type}";
        }
        #endregion

        #region Override Methods

        protected override void Die()
        {
            // If in the moment of dead HitFlashComponent is flashing, wait until finish and the change material
            if (_hitFlashComponent && _hitFlashComponent.IsFlashing)
            {
                _hitFlashComponent.OnFlashFinished += OnFlashComponentFinished;
            }
            // If not, just change material instantly
            else if (_meshRenderer)
            {
                _meshRenderer.material = deathMaterial;
            }
            
            base.Die();
        }

        #endregion

        #region Private Functions

        private void OnFlashComponentFinished()
        {
            if (_meshRenderer) _meshRenderer.material = deathMaterial;
        }

        #endregion
    }
}