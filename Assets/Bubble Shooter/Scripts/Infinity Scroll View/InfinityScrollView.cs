using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

public class InfinityScrollView : MonoBehaviour
{
    [SerializeField] private Vector2 centerXRectPosValueRange;
    [SerializeField] private InfinityScrollItem defaultItem;
    [SerializeField] private InfinityScrollItem middleItem;
    [SerializeField] private InfinityScrollItem prevMiddleItem;
    [SerializeField] private Transform content;
    [SerializeField] private string middleItemDebug;

    Dequeue<InfinityScrollItem> deque = new Dequeue<InfinityScrollItem>();

    private void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            AddToItemPool(Instantiate(defaultItem));
        }

        InfinityScrollItem i0 = GetFromPool();
        i0.transform.SetParent(content);
        i0.Val = 0;
        deque.AddBack(i0);

        InfinityScrollItem i1 = GetFromPool();
        i1.transform.SetParent(content);
        i1.Val = 1;
        deque.AddBack(i1);

        InfinityScrollItem i2 = GetFromPool();
        i2.transform.SetParent(content);
        i2.Val = 2;
        deque.AddBack(i2);

        InfinityScrollItem i3 = GetFromPool();
        i3.transform.SetParent(content);
        i3.Val = 3;
        deque.AddBack(i3);
    }

    private void Update()
    {
        foreach (var item in deque)
        {
            RectTransform rect = item.GetComponent<RectTransform>();
            if (rect.position.x > centerXRectPosValueRange.x && rect.position.x <= centerXRectPosValueRange.y)
            {
                middleItem = item;
                middleItemDebug = item.gameObject.name;
            }
        }

        if(prevMiddleItem != middleItem)
        {
            //Middle item changed
            prevMiddleItem = middleItem;
        }
    }


    List<InfinityScrollItem> poolItems = new List<InfinityScrollItem>();
    public void AddToItemPool(InfinityScrollItem item)
    {
        item.gameObject.SetActive(false);
        poolItems.Add(item);
    }

    public InfinityScrollItem GetFromPool()
    {
        foreach (var item in poolItems)
        {
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }

        return null;
    }
}
