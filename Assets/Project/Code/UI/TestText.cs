using System.Collections;
using Project.Code.Gameplay.Player.Stats;
using Project.Code.Gameplay.States;
using Project.Code.Gameplay.States.StatesLibrary.Player.Grounded;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Code.UI
{
    public class TestText : MonoBehaviour
    {

        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private PlayerStateManager playerState;
        
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
                var state = playerState?.CurrentState ?? playerState?.PlayerIdleState ?? new PlayerIdleState();

                var printText = $"Strength: {strength}, \nSpeed: {speed}, \nBelly: {belly}, \nState: {state.ToString()}";
            
                _text.text = printText;
                yield return new WaitForSeconds(0.5f);    
            }
        }
    }
    
}
