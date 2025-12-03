using Project.Code.Gameplay.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Code.UI
{
    public class Healthbar : MonoBehaviour
    {
    
        [SerializeField]
        private BaseStats baseStats;
        
        [SerializeField]
        private Image healthBarImage;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            if (!baseStats) return;
            
            baseStats.OnHealthChanged += BaseStatsOnOnHealthChanged;
        }

        private void BaseStatsOnOnHealthChanged(float currentHealth, float maxHealth)
        {
            if (!healthBarImage) return;
            
            healthBarImage.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
        }

    }
}
