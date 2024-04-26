using _Scripts.Runtime.Entity.CharacterController.States.BaseStates;
using UnityEngine;

namespace _Scripts.Runtime.Entity.CharacterController.StateFactory
{
    public class CharacterStateFactory
    {
        private readonly CharacterStateMachine _context;

        public InitializationState InitializationState;
        public CharacterWalkState WalkState;
        public CharacterIdleState IdleState;
        public CharacterJumpState JumpState;

        public CharacterStateFactory(CharacterStateMachine currentContext)
        {
            _context = currentContext;
        }

        public CharacterBaseState Idle()
        {
            return IdleState ??= new CharacterIdleState(_context, this);
        }
        public CharacterBaseState Walk()
        {
            return WalkState ??= new CharacterWalkState(_context, this);
        }
        public CharacterBaseState Initialize()
        {
            return InitializationState ??= new InitializationState(_context, this);
        }
    
        public CharacterBaseState Jump(bool isMoving = false)
        {
            JumpState ??= new CharacterJumpState(_context, this, isMoving);
            JumpState.IsMoving = isMoving;
            return JumpState;
        }
    }
}
