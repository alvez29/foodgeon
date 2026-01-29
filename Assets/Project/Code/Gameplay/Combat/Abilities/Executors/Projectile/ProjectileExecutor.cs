using System.Collections;
using Project.Code.Core;
using Project.Code.Core.Data.ScriptableObjects;
using Project.Code.Core.Interfaces;
using Project.Code.Gameplay.Projectile;
using Project.Code.Gameplay.Stats;
using UnityEngine;

namespace Project.Code.Gameplay.Combat.Abilities.Executors.Projectile
{
    /// <summary>
    /// Executor for projectile abilities.
    /// Spawn projectile and bind the hit event.
    /// </summary>
    [CreateAssetMenu(fileName = "New Projectile Executor", menuName = "Foodgeon/Executors/Projectile Executor")]
    public class ProjectileExecutor : AbilityExecutor
    {
        #region Fields

        [Header("Projectile Settings")] [SerializeField]
        private float projectileDiameter = 0.3f;

        [SerializeField] private float projectileLifeSpan = 10f;
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private ProjectileBehaviour projectilePrefab;
        [SerializeField] private LayerMask projectileCollisionLayers;

        #endregion

        #region Properties

        private float _projectileDiameterValue = -1.0f;

        #endregion


        #region Private methods
        
        private float GetProjectileDiameterValue(CharacterController characterController)
        {
            return projectileDiameter * Constants.Stats.RangeBaseUnit(characterController);
        }

        private void Shoot(GameObject caster, CharacterController casterCharacterController, AbilityData abilityData)
        {
            if (Mathf.Approximately(_projectileDiameterValue, -1.0f))
                _projectileDiameterValue = GetProjectileDiameterValue(casterCharacterController);

            var instance = Instantiate(projectilePrefab,
                caster.transform.position + new Vector3(0, Constants.Stats.Height(casterCharacterController) / 2, 0),
                Quaternion.identity);

            instance.InitializeData(projectileSpeed, _projectileDiameterValue / 2,
                projectileCollisionLayers, caster.transform.forward, projectileLifeSpan);
            instance.OnProjectileHit += (otherObject) => OnHit(caster, otherObject, abilityData);
        }

        #endregion

        #region Override Methods

        public override void Execute(GameObject caster, CharacterController controller, AbilityData data)
        {
            Shoot(caster, controller, data);
        }

        public override void OnHit(GameObject caster, GameObject target, AbilityData data)
        {
            // Calculate damage based on caster's strength and ability's damage multiplier
            //TODO: This GetComponent can be optimized with interface
            var userStats = caster.GetComponent<BaseStats>();
            var baseDamage = userStats?.Strength ?? 10f;
            var abilityPower = data.Power;

            // Apply damage to target
            if (!target.TryGetComponent(out IDamageable damageable)) return;

            var damageDealt = damageable.TakeDamage(caster, new BaseStats.DamageData(baseDamage, abilityPower));
            Debug.Log(
                $"[Projectile Executor] Damaged {target.name} for {damageDealt} (base: {baseDamage}, power: {abilityPower})");
        }

        #endregion
    }
}