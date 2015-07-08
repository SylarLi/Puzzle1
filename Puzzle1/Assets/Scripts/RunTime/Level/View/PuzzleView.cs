using System;
using UnityEngine;
using DG.Tweening;

public class PuzzleView : MonoBehaviour
{
    public const float PuzzleDepth = 20;
    public const float QuadGap = 0.1f;
    public const float RotateDuration = 0.5f;
    public const float RotateDelay = 0.05f;

    private IPuzzle _puzzle;

    private QuadView[,] _quadViews;

    private void Awake()
    {

    }

    public IPuzzle puzzle
    {
        get
        {
            return _puzzle;
        }
        set
        {
            _puzzle = value;
            UpdatePuzzleView();
        }
    }

    private void UpdatePuzzleView()
    {
        _quadViews = new QuadView[_puzzle.rows, _puzzle.columns];
        Vector3 center = new Vector3((_puzzle.columns * (QuadView.QuadSize + QuadGap) - QuadGap) / 2, (_puzzle.rows * (QuadView.QuadSize + QuadGap) - QuadGap) / 2);
        for (int i = 0; i < _puzzle.rows; i++)
        {
            for (int j = 0; j < _puzzle.columns; j++)
            {
                GameObject quadgo = new GameObject(string.Format("quad_{0}_{1}", i, j));
                quadgo.transform.parent = transform;
                quadgo.transform.localPosition = new Vector3(j * (QuadView.QuadSize + QuadGap) + QuadView.QuadSize / 2 - QuadGap - center.x, i * (QuadView.QuadSize + QuadGap) + QuadView.QuadSize / 2 - QuadGap - center.y, 0);
                _quadViews[i, j] = quadgo.AddComponent<QuadView>();
                _quadViews[i, j].quad = _puzzle[i, j];
            }
        }
    }

    public void Roll(IOperation op, Action callBack)
    {
        float maxDelay = 0;
        IQuad[] rowQuads = _puzzle.GetRowQuads(op.row);
        foreach (IQuad quad in rowQuads)
        {
            if (quad.column != op.column)
            {
                QuadView qv = _quadViews[quad.row, quad.column];
                int factor = quad.column < op.column ? 1 : -1;
                float delay = (Mathf.Abs(quad.column - op.column) - 1) * RotateDelay;
                PlayQuadTween(qv, qv.localEulerAngles + new Vector3(0, factor * 180, 0), RotateDuration, delay);
                maxDelay = Mathf.Max(delay, maxDelay);
            }
        }
        IQuad[] columnQuads = puzzle.GetColumnQuads(op.column);
        foreach (IQuad quad in columnQuads)
        {
            if (quad.row != op.row)
            {
                QuadView qv = _quadViews[quad.row, quad.column];
                int factor = quad.row > op.row ? 1 : -1;
                float delay = (Mathf.Abs(quad.row - op.row) - 1) * RotateDelay;
                PlayQuadTween(qv, qv.localEulerAngles + new Vector3(factor * 180, 0, 0), RotateDuration, delay);
                maxDelay = Mathf.Max(delay, maxDelay);
            }
        }
        DOTween.To(() => 0f, x => x = 0f, 0f, RotateDuration + maxDelay).OnComplete(() => callBack());
    }

    private Tweener PlayQuadTween(QuadView quadtr, Vector3 to, float duration, float delay)
    {
        Tweener t = DOTween.To(() => quadtr.localEulerAngles, x => quadtr.localEulerAngles = x, to, duration)
            .SetDelay(delay)
            .SetEase(Ease.OutBack);
        return t;
    }
}
