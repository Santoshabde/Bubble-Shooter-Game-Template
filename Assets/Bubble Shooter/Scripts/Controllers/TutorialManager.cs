using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject hand_BG;

    private void Awake()
    {
        StartCoroutine(DisableTuts());
        IEnumerator DisableTuts()
        {
            yield return new WaitForSeconds(2.5f);
            hand.GetComponent<SpriteRenderer>().DOFade(0, 1f);
            hand_BG.GetComponent<SpriteRenderer>().DOFade(0, 1f);

            yield return new WaitForSeconds(1f);
            hand.SetActive(false);
            hand_BG.SetActive(false);
        }
    }
}
