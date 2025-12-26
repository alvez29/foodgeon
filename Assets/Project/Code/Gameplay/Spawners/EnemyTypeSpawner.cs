using System;
using System.Collections;
using System.Collections.Generic;
using Project.Code.Core.Data.Enums;
using Project.Code.Gameplay.Enemies;
using UnityEngine;

namespace Project.Code.Gameplay.Spawners
{
    public class EnemyTypeSpawner : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private EnemyStats enemyPrefab;
        [SerializeField] private float spawnDistance = 20f;
        [SerializeField] private int gridColumns = 5;
        [SerializeField] private float respawnDelay = 1f;

        private void Start()
        {
            SpawnInitialEnemies();
        }

        private void SpawnInitialEnemies()
        {
            if (enemyPrefab == null)
            {
                Debug.LogError("[EnemyTypeSpawner] No enemy prefab assigned!");
                return;
            }

            var types = Enum.GetValues(typeof(EnemyType));
            int validEnemyCount = 0;

            foreach (EnemyType type in types)
            {
                if (type == EnemyType.None) continue;

                // Calculate grid position
                int row = validEnemyCount / gridColumns;
                int col = validEnemyCount % gridColumns;

                Vector3 xOffset = transform.right * (col * spawnDistance);
                Vector3 zOffset = transform.forward * (row * spawnDistance);
                
                Vector3 spawnPosition = transform.position + xOffset + zOffset;
                
                SpawnEnemy(type, spawnPosition);
                CreateLabel(type, spawnPosition);
                
                validEnemyCount++;
            }
        }

        private void SpawnEnemy(EnemyType type, Vector3 position)
        {
            EnemyStats enemyInstance = Instantiate(enemyPrefab, position, Quaternion.identity);
            
            // Initialize with specific type
            enemyInstance.Initialize(1, type);
            
            System.Action onDeathAction = null;
            onDeathAction = () =>
            {
                enemyInstance.OnDeath -= onDeathAction;
                StartCoroutine(RespawnRoutine(type, position));
            };
            
            enemyInstance.OnDeath += onDeathAction;
        }

        private void CreateLabel(EnemyType type, Vector3 position)
        {
            GameObject labelObj = new GameObject($"Label_{type}");
            labelObj.transform.position = position + Vector3.up * 2f; // Position slightly above
            labelObj.transform.SetParent(transform);

            TextMesh textMesh = labelObj.AddComponent<TextMesh>();
            textMesh.text = type.ToString();
            textMesh.characterSize = 0.2f;
            textMesh.fontSize = 64;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.color = Color.white;
            
            // Always face the camera
            labelObj.AddComponent<LookAtCamera>();
        }
        
        // Simple helper for looking at camera if we don't use the existing Billboard
        private class LookAtCamera : MonoBehaviour
        {
            void Update()
            {
                if (UnityEngine.Camera.main != null)
                    transform.rotation = Quaternion.LookRotation(transform.position - UnityEngine.Camera.main.transform.position);
            }
        }

        private IEnumerator RespawnRoutine(EnemyType type, Vector3 position)
        {
            yield return new WaitForSeconds(respawnDelay);
            SpawnEnemy(type, position);
        }
    }
}
