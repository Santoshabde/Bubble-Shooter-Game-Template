using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using TMPro;
using DG.Tweening;

namespace SNGames.BubbleShooter
{
    public class LevelTargetSSummaryDialog : BaseUIDialog
    {
        [SerializeField] private InGameBubblesData inGameBubbleData;
        [SerializeField] private TextMeshProUGUI racoonsLivesToSave;
        [SerializeField] private Transform parentTransform;
        [SerializeField] private GoalTarget goalTarget;
        [SerializeField] private Transform targetPannelToAnimate;

        private LevelGenData currentLevelGenData = null;
        private List<GoalTarget> spawnedGoalTargets = new List<GoalTarget>();

        public override void OnOpenDialog()
        {
            base.OnOpenDialog();

            targetPannelToAnimate.localScale = Vector3.zero;

            currentLevelGenData = LevelData.currentLevelGenData;
            InitialGoalSetUp(currentLevelGenData.targetBubbles);

            //Animation part!!
            Sequence dialogAnimationSeq = DOTween.Sequence();
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one * 1.1f), 0.4f));
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one * 0.9f), 0.2f));
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one), 0.2f));

            StartCoroutine(DisbleTheDialog());
        }

        public override void OnCloseDialog()
        {
            // targetPannelToAnimate.DOScale((Vector3.zero), 0.5f).OnComplete(() => base.OnCloseDialog());

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

        private IEnumerator DisbleTheDialog()
        {
            yield return new WaitForSeconds(3f);
            Sequence dialogAnimationSeq = DOTween.Sequence();
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.one) * 1.1f, 0.2f));
            dialogAnimationSeq.Append(targetPannelToAnimate.DOScale((Vector3.zero), 0.5f));
            yield return new WaitForSeconds(0.7f);
            OnCloseDialog();
        }
    }
}