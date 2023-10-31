using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using SNGames.BubbleShooter;

public class GameStart : State
{
    private GameManager gameStateManager;
    private LevelGenData currentLevelGenData;

    public GameStart(GameManager gameManager)
    {
        gameStateManager = gameManager;
    }

    public override void Enter()
    {
        Application.targetFrameRate = 120;

        //Fetch the level, Update current level

        //Fetch that particular levelGendata
        gameStateManager.currentLevelGenData = gameStateManager.InGameLevelData.Data[gameStateManager.currentLevel];

        currentLevelGenData = gameStateManager.currentLevelGenData;
        //Generate level - either random or level josn
        if (currentLevelGenData.generateRandomLevel)
            gameStateManager.LevelGenerator?.GenerateRandomLevel(currentLevelGenData.startSpawnPosition.x, currentLevelGenData.startSpawnPosition.y, currentLevelGenData.numberOfRows, currentLevelGenData.numberOfColumns);
        else
            gameStateManager.LevelGenerator?.GenerateLevelFromLevelJson(currentLevelGenData.levelJson);

        //Update HUD
        gameStateManager.ScoreController.UpdateTimer(TimerUtility.ConvertSecondsToTimer(currentLevelGenData.totalGameTimeInSeconds));
        gameStateManager.ScoreController.UpdateRacoonsToRescue(currentLevelGenData.targetRacoonsToSave);
        gameStateManager.ScoreController.InitialGoalSetUp(currentLevelGenData.targetBubbles);

        gameStateManager.SwitchState(new GameProgress(gameStateManager));
    }

    public override void Exit()
    {
       
    }

    public override void Tick(float deltaTime)
    {
        
    }
}
