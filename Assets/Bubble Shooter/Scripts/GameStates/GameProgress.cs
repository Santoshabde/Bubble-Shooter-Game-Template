using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using SNGames.BubbleShooter;
using System;

public class GameProgress : State
{
    private GameManager gameStateManager;
    private Coroutine timerCoroutine;

    public GameProgress(GameManager gameManager)
    {
        gameStateManager = gameManager;
    }

    public override void Enter()
    {
        //Start game timer
        if(timerCoroutine != null)
        {
            gameStateManager.StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        timerCoroutine = gameStateManager.StartCoroutine(StartGameTimer());
    }

    public override void Exit()
    {
        if (timerCoroutine != null)
        {
            gameStateManager.StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            gameStateManager.ScoreController.UpdateTimer("--");
        }
    }

    public override void Tick(float deltaTime)
    {
        
    }

    private IEnumerator StartGameTimer()
    {
        DateTime startTime = DateTime.Now;
        LevelGenData levelGenData = gameStateManager.currentLevelGenData;
        ScoreController scoreController = gameStateManager.ScoreController;

        int timePassed = (int)DateTime.Now.Subtract(startTime).TotalSeconds;
        while (levelGenData.totalGameTimeInSeconds - timePassed >= 0)
        {
            yield return new WaitForSeconds(1f);
            timePassed = (int)DateTime.Now.Subtract(startTime).TotalSeconds;

            int timeRemaining = levelGenData.totalGameTimeInSeconds - timePassed;
            if (timeRemaining >= 0)
                scoreController.UpdateTimer(TimerUtility.ConvertSecondsToTimer(timeRemaining), timeRemaining < 10);
        }

        gameStateManager.SwitchState(new GameFailed(gameStateManager, GameFailedType.GameTimeOut));
    }
}
