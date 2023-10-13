using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public abstract class StateMachine : MonoBehaviour
    {
        [SerializeField] private string currentCharacterState;

        private State currentState;

        public void SwitchState(State newState)
        {
            currentCharacterState = newState.ToString();

            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        private void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }
    }
}