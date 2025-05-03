using UnityEngine.SceneManagement;

namespace LanderFighter
{
    public class ReturnMainMenuState : BattleStateBase
    {
        public override BattleState GetNextState()
        {
            throw new System.NotImplementedException();
        }

        public override void OnEnter()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public override BattleStateResult OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
