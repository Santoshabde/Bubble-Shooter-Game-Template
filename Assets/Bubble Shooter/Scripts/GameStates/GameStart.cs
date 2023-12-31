using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using SNGames.BubbleShooter;

public class GameStart : State
{
    private GameManager gameStateManager;
    private LevelGenData currentLevelGenData;

    private LevelGenerator levelGenerator;

    public GameStart(GameManager gameManager)
    {
        gameStateManager = gameManager;
    }

    public override void Enter()
    {
        Application.targetFrameRate = 120;

        levelGenerator = ServiceRegistry.Get<LevelGenerator>();
        gameStateManager.InGameUIManager.CloseAllDialogs();

        gameStateManager.StartCoroutine(GameStart_IEnum());
    }

    public override void Exit()
    {
       
    }

    public override void Tick(float deltaTime)
    {
        
    }

    private IEnumerator GameStart_IEnum()
    {
        yield return null;

        SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.OnNewLevelStart);

        //Fetch the level, Update current level
        int currentLevel = LocalSaveSystem.playerInGameStats.currentLevel;

        //Fetch that particular levelGendata
        gameStateManager.currentLevelGenData = InGameLevelData.Data[currentLevel.ToString()];

        currentLevelGenData = gameStateManager.currentLevelGenData;

        LevelData.currentLevelGenData = currentLevelGenData;

        gameStateManager.InGameUIManager.OpenDialog<LevelTargetSSummaryDialog>();

        yield return new WaitForSeconds(3.7f);

        //Generate level - either random or level josn
        if (currentLevelGenData.generateRandomLevel)
            levelGenerator.GenerateRandomLevel(currentLevelGenData.startSpawnPosition.x, currentLevelGenData.startSpawnPosition.y, currentLevelGenData.numberOfRows, currentLevelGenData.numberOfColumns, true);
        else
            levelGenerator.GenerateLevelFromLevelJson(currentLevelGenData.levelJson);


        //Update HUD
        gameStateManager.ScoreController.UpdateTimer(TimerUtility.ConvertSecondsToTimer(currentLevelGenData.totalGameTimeInSeconds));
        gameStateManager.ScoreController.UpdateRacoonsToRescue(currentLevelGenData.targetRacoonsToSave);
        gameStateManager.ScoreController.InitialGoalSetUp(currentLevelGenData.targetBubbles);

        gameStateManager.SwitchState(new GameProgress(gameStateManager));
    }
}
