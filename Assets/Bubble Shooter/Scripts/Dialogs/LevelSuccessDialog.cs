using DG.Tweening;
using SNGames.CommonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SNGames.BubbleShooter
{
    public class LevelSuccessDialog : BaseUIDialog
    {
        [SerializeField] private InGameBubblesData inGameBubbleData;
        [SerializeField] private Transform parentTransform;
        [SerializeField] private GoalTarget goalTarget;
        [SerializeField] private Transform targetPannelToAnimate;

        private LevelGenData currentLevelGenData = null;
        private List<GoalTarget> spawnedGoalTargets = new List<GoalTarget>();

        public override void OnOpenDialog()
        {
            base.OnOpenDialog();

            currentLevelGenData = LevelData.currentLevelGenData;
            InitialGoalSetUp(currentLevelGenData.targetBubbles);

            //Animation part!!
            targetPannelToAnimate.localScale = Vector3.zero;
            Sequence dialogAnimationSeq = DOTween.Sequence();
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one * 1.1f), 0.4f));
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one * 0.9f), 0.2f));
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one), 0.2f));
        }

        public override void OnCloseDialog()
        {
            base.OnCloseDialog();

            spawnedGoalTargets.ForEach(t => Destroy(t.gameObject));
        }

        public void InitialGoalSetUp(List<TargetLevelBubble> targetBubbles)
        {
            spawnedGoalTargets = new List<GoalTarget>();
            foreach (var item in targetBubbles)
            {
                GoalTarget spawnedTarget = Instantiate(goalTarget);
                spawnedTarget.transform.SetParent(parentTransform);
                spawnedTarget.transform.gameObject.SetActive(true);
                spawnedTarget.SetTarget(item.targetNumber, inGameBubbleData.BubbleIdAndSprite[item.targetBubble], true);
                spawnedGoalTargets.Add(spawnedTarget);
            }
        }

        public void OnNextLevelButtonClick()
        {
            //Fetching currenlt Player in Game Stat
            PlayerInGameStats currentPlayerInGameStats = LocalSaveSystem.playerInGameStats;
            currentPlayerInGameStats.currentLevel += 1;

            //Updating Local Save Data
            LocalSaveSystem.playerInGameStats = currentPlayerInGameStats;

            Sequence dialogAnimationSeq = DOTween.Sequence();
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one) * 1.1f, 0.2f));
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.zero), 0.5f));
            dialogAnimationSeq.OnComplete(() => GameManager.Instance.SwitchState(new GameStart(GameManager.Instance)));
        }
    }
}