using System;
using UnityEngine;
using DG.Tweening;

public class PuzzleView : VisisonView<IPuzzle>
{
    private QuadView[,] _quadViews;

    private void Awake()
    {

    }

    protected override void Trigger()
    {
        base.Trigger();
        InitPuzzleView();
    }

    private void InitPuzzleView()
    {
        _quadViews = new QuadView[data.rows, data.columns];
        Vector3 center = new Vector3((data.columns * (Style.QuadSize + Style.QuadGap) - Style.QuadGap) / 2, (data.rows * (Style.QuadSize + Style.QuadGap) - Style.QuadGap) / 2);
        for (int i = 0; i < data.rows; i++)
        {
            for (int j = 0; j < data.columns; j++)
            {
                GameObject quadgo = new GameObject(string.Format("quad_{0}_{1}", i, j));
                quadgo.transform.parent = transform;
                data[i, j].localPosition = new Vector3(j * (Style.QuadSize + Style.QuadGap) + Style.QuadSize / 2 - center.x, i * (Style.QuadSize + Style.QuadGap) + Style.QuadSize / 2 - center.y, 0);
                _quadViews[i, j] = quadgo.AddComponent<QuadView>();
                _quadViews[i, j].data = data[i, j];
            }
        }
    }
}
