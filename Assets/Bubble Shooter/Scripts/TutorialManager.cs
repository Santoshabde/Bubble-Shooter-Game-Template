using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject hand_BG;

    private void Awake()
    {
        StartCoroutine(DisableTuts());
        IEnumerator DisableTuts()
        {
            yield return new WaitForSeconds(3f);
            hand.SetActive(false);
            hand_BG.SetActive(false);
        }
    }
}
