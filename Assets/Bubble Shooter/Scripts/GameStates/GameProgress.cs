using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using SNGames.BubbleShooter;
using System;

public class GameProgress : State
{
    private GameManager gameStateManager;
    public GameProgress(GameManager gameManager)
    {
        gameStateManager = gameManager;
    }

    public override void Enter()
    {
        //Start game timer
        gameStateManager.StartCoroutine(StartGameTimer());
    }

    public override void Exit()
    {
        
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

            if (levelGenData.totalGameTimeInSeconds - timePassed >= 0)
                scoreController.UpdateTimer(TimerUtility.ConvertSecondsToTimer(levelGenData.totalGameTimeInSeconds - timePassed));
        }
    }
}
