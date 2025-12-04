using System;
using System.Collections;
using Project.Code.Gameplay.Player.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Code.UI
{
    public class TestText : MonoBehaviour
    {

        [SerializeField] private PlayerStats playerStats;
        
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        private void Start()
        {
            StartCoroutine(UpdateText());
        }

        private IEnumerator UpdateText()
        {
            while (true)
            {
                var strength = playerStats.Strength;
                var speed = playerStats.Speed;
                var belly = playerStats.BellyCount;

                var printText = $"Strength: {strength}, \nSpeed: {speed}, \nBelly: {belly}";
            
                _text.text = printText;
                yield return new WaitForSeconds(0.5f);    
            }
        }
    }
    
}
