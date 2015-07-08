using System;
using UnityEngine;
using DG.Tweening;

public class PuzzleView : MonoBehaviour
{
    public const float PuzzleDepth = 20;
    public const float QuadGap = 0.1f;
    public const float RollDelay = 0.05f;

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
                quadgo.transform.localPosition = new Vector3(j * (QuadView.QuadSize + QuadGap) + QuadView.QuadSize / 2 - center.x, i * (QuadView.QuadSize + QuadGap) + QuadView.QuadSize / 2 - center.y, 0);
                _quadViews[i, j] = quadgo.AddComponent<QuadView>();
                _quadViews[i, j].quad = _puzzle[i, j];
            }
        }
    }

    public void Roll(IOperation op, Action callBack)
    {
        float maxDelay = 0;
        IQuad[] rowQuads = puzzle.GetRowQuads(op.row);
        for (int i = op.column - 1; i >= 0; i--)
        {
            QuadView qv = _quadViews[rowQuads[i].row, rowQuads[i].column];
            float delay = (Mathf.Abs(rowQuads[i].column - op.column) - 1) * RollDelay;
            maxDelay = Mathf.Max(maxDelay, delay);
            if (rowQuads[i].value == QuadValue.Block)
            {
                qv.BlockShake(new Vector3(0, QuadView.ShakeAngle, 0), delay);
                break;
            }
            else
            {
                qv.QuadRoll(new Vector3(0, QuadView.RollAngle, 0), delay);
            }
        }
        for (int i = op.column + 1, len = rowQuads.Length; i < len; i++)
        {
            QuadView qv = _quadViews[rowQuads[i].row, rowQuads[i].column];
            float delay = (Mathf.Abs(rowQuads[i].column - op.column) - 1) * RollDelay;
            maxDelay = Mathf.Max(maxDelay, delay);
            if (rowQuads[i].value == QuadValue.Block)
            {
                qv.BlockShake(new Vector3(0, -QuadView.ShakeAngle, 0), delay);
                break;
            }
            else
            {
                qv.QuadRoll(new Vector3(0, -QuadView.RollAngle, 0), delay);
            }
        }
        IQuad[] columnQuads = puzzle.GetColumnQuads(op.column);
        for (int i = op.row - 1; i >= 0; i--)
        {
            QuadView qv = _quadViews[columnQuads[i].row, columnQuads[i].column];
            float delay = (Mathf.Abs(columnQuads[i].row - op.row) - 1) * RollDelay;
            maxDelay = Mathf.Max(maxDelay, delay);
            if (columnQuads[i].value == QuadValue.Block)
            {
                qv.BlockShake(new Vector3(QuadView.ShakeAngle, 0, 0), delay);
                break;
            }
            else
            {
                qv.QuadRoll(new Vector3(QuadView.RollAngle, 0, 0), delay);
            }
        }
        for (int i = op.row + 1, len = columnQuads.Length; i < len; i++)
        {
            QuadView qv = _quadViews[columnQuads[i].row, columnQuads[i].column];
            float delay = (Mathf.Abs(columnQuads[i].row - op.row) - 1) * RollDelay;
            maxDelay = Mathf.Max(maxDelay, delay);
            if (columnQuads[i].value == QuadValue.Block)
            {
                qv.BlockShake(new Vector3(-QuadView.ShakeAngle, 0, 0), delay);
                break;
            }
            else
            {
                qv.QuadRoll(new Vector3(-QuadView.RollAngle, 0, 0), delay);
            }
        }
        DOTween.To(() => 0f, x => x = 0f, 0f, QuadView.RollDuration + maxDelay).OnComplete(() => callBack());
    }
}
