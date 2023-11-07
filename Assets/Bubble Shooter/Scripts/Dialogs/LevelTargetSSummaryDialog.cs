using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace SNGames.BubbleShooter
{
    public class LevelTargetSSummaryDialog : BaseUIDialog
    {
        [SerializeField] private InGameBubblesData inGameBubbleData;
        [SerializeField] private TextMeshProUGUI racoonsLivesToSave;
        [SerializeField] private Transform parentTransform;
        [SerializeField] private GoalTarget goalTarget;
        [SerializeField] private Transform targetPannelToAnimate;
        [SerializeField] private TextMeshProUGUI currentLevelInfo;
        [SerializeField] private Image bgImage;

        private LevelGenData currentLevelGenData = null;
        private List<GoalTarget> spawnedGoalTargets = new List<GoalTarget>();

        public override void OnOpenDialog()
        {
            base.OnOpenDialog();

            currentLevelInfo.text = "Level: " + LocalSaveSystem.playerInGameStats.currentLevel;

            currentLevelGenData = LevelData.currentLevelGenData;
            InitialGoalSetUp(currentLevelGenData.targetBubbles);

            StartCoroutine(DisableTheDialog());

            //Animation part!!
            targetPannelToAnimate.localScale = Vector3.zero;
            bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0);

            bgImage.DOFade(0.9843f, 0.4f).SetEase(Ease.InSine);

            Sequence dialogAnimationSeq = DOTween.Sequence();
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one * 1.1f), 0.4f)).SetEase(Ease.InSine);
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one * 0.9f), 0.2f)).SetEase(Ease.InSine);
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one), 0.2f)).SetEase(Ease.InSine);
        }

        public override void OnCloseDialog()
        {
            base.OnCloseDialog();
            spawnedGoalTargets.ForEach(t =>
            {
                if (t != null) Destroy(t.gameObject);
            });
        }

        public void InitialGoalSetUp(List<TargetLevelBubble> targetBubbles)
        {
            spawnedGoalTargets = new List<GoalTarget>();
            foreach (var item in targetBubbles)
            {
                GoalTarget spawnedTarget = Instantiate(goalTarget);
                spawnedTarget.transform.SetParent(parentTransform);
                spawnedTarget.transform.gameObject.SetActive(true);
                spawnedTarget.transform.localScale = Vector3.one;
                spawnedTarget.SetTarget(item.targetNumber, inGameBubbleData.BubbleIdAndSprite[item.targetBubble]);
                spawnedGoalTargets.Add(spawnedTarget);
            }
        }

        private IEnumerator DisableTheDialog()
        {
            yield return new WaitForSeconds(3f);

            bgImage.DOFade(0, 1.2f).SetEase(Ease.InSine);

            Sequence dialogAnimationSeq = DOTween.Sequence();
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one) * 1.1f, 0.2f));
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.zero), 0.5f));
            yield return new WaitForSeconds(0.72f);

            OnCloseDialog();
        }
    }
}