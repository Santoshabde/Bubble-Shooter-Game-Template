using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private CanvasGroup splashScreenCanvaGroup;
    
    private void Start()
    {
        StartCoroutine(ShowLevelSelectionScreen());
    }

    private IEnumerator ShowLevelSelectionScreen()
    {
        yield return new WaitForSeconds(2f);

        splashScreenCanvaGroup.DOFade(0, 1f);
    }

    public void LoadLevel(int levelNumber)
    {
        LocalSaveSystem.playerInGameStats.currentLevel = levelNumber;
    }
}
