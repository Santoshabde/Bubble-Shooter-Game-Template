using SNGames.BubbleShooter;
using SNGames.CommonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSuccess : State
{
    private GameManager gameStateManager;
    public GameSuccess(GameManager gameManager)
    {
        gameStateManager = gameManager;
    }


    public override void Enter()
    {
        gameStateManager.StartCoroutine(OnGameSuccess());
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
       
    }

    IEnumerator OnGameSuccess()
    {
        //Spawn VFX
        VFXData.SpawnVFX("Confetti", Vector3.zero, 2f);
        yield return new WaitForSeconds(0.4f);
        VFXData.SpawnVFX("Confetti", new Vector3(1.44f, 3, 0), 2f);
        yield return new WaitForSeconds(0.4f);
        VFXData.SpawnVFX("Confetti", new Vector3(-1.79f, 1.77f, 0), 2f);
        yield return new WaitForSeconds(0.4f);
        VFXData.SpawnVFX("Confetti", new Vector3(-0.95f, -2.62f, 0), 2f);
        yield return new WaitForSeconds(2f);

        //Dialog
        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.OnLevelSuccess);
        gameStateManager.InGameUIManager.OpenDialog<LevelSuccessDialog>();
    }
}
