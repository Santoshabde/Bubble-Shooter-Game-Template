using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfinityScrollItem : MonoBehaviour
{
    public int val;
    public int Val
    {
        set
        {
            val = value;
            text.text = value.ToString();
        }
        get
        {
            return val;
        }
    }

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Vector3 position;

    private void Update()
    {
        position = GetComponent<RectTransform>().position;
    }
}
