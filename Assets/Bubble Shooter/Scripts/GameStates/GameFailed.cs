using SNGames.BubbleShooter;
using SNGames.CommonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameFailedType
{
    GameTimeOut
}

public class GameFailed : State
{
    private GameManager gameStateManager;
    public GameFailed(GameManager gameManager, GameFailedType gameFailedType)
    {
        gameStateManager = gameManager;
    }

    public override void Enter()
    {
        gameStateManager.InGameUIManager.OpenDialog<LevelFailedDialog>();

        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.OnLevelFail);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {

    }
}
