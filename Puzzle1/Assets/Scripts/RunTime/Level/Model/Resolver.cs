using DG.Tweening;
using UnityEngine;

public class Resolver : IResolver
{
    public void Apply(IPuzzle puzzle, IOperation op)
    {
        switch (op.type)
        {
            case OperationType.TouchClick:
                {
                    ApplyTouchClick(puzzle, op);
                    break;
                }
            case OperationType.TouchStart:
                {
                    ApplyTouchStart(puzzle, op);
                    break;
                }
            case OperationType.TouchEnd:
                {
                    ApplyTouchEnd(puzzle, op);
                    break;
                }
        }
    }

    private void ApplyTouchClick(IPuzzle puzzle, IOperation op)
    {
        float maxDelay = 0f;
        IQuad[] rowQuads = puzzle.GetRowQuads(op.row);
        for (int i = op.column - 1; i >= 0; i--)
        {
            IQuad quad = rowQuads[i];
            float delay = (Mathf.Abs(quad.column - op.column) - 1) * Style.QuadRollDelay;
            maxDelay = Mathf.Max(maxDelay, delay);
            if (quad.value == QuadValue.Block)
            {
                Sequence shakeSeq = DOTween.Sequence();
                shakeSeq.AppendInterval(delay)
                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(0, Style.QuadShakeAngle, 0), Style.QuadShakeDuration * 0.25f))
                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles - new Vector3(0, Style.QuadShakeAngle, 0), Style.QuadShakeDuration * 0.25f))
                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles, Style.QuadShakeDuration * 0.5f).SetEase(Ease.OutBack));
                break;
            }
            else
            {
                DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(0, Style.QuadRollAngle, 0), Style.QuadRollDuration)
                    .SetDelay(delay)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => quad.value = (QuadValue)(QuadValue.Back - quad.value));
            }
        }
        for (int i = op.column + 1, len = rowQuads.Length; i < len; i++)
        {
            if (rowQuads[i].value == QuadValue.Block)
            {
                break;
            }
            else
            {
                rowQuads[i].value = (QuadValue)(QuadValue.Back - rowQuads[i].value);
            }
        }
        IQuad[] columnQuads = puzzle.GetColumnQuads(op.column);
        for (int i = op.row - 1; i >= 0; i--)
        {
            if (columnQuads[i].value == QuadValue.Block)
            {
                break;
            }
            else
            {
                columnQuads[i].value = (QuadValue)(QuadValue.Back - columnQuads[i].value);
            }
        }
        for (int i = op.row + 1, len = columnQuads.Length; i < len; i++)
        {
            if (columnQuads[i].value == QuadValue.Block)
            {
                break;
            }
            else
            {
                columnQuads[i].value = (QuadValue)(QuadValue.Back - columnQuads[i].value);
            }
        }
    }

    private void ApplyTouchStart(IPuzzle puzzle, IOperation op)
    {

    }

    private void ApplyTouchEnd(IPuzzle puzzle, IOperation op)
    {

    }

    public bool IsWin(IPuzzle puzzle)
    {
        bool pass = true;
        QuadValue value = QuadValue.Block;
        for (int i = 0; i < puzzle.rows; i++)
        {
            for (int j = 0; j < puzzle.columns; j++)
            {
                if (value == QuadValue.Block && 
                    (puzzle[i, j].value == QuadValue.Front || puzzle[i, j].value == QuadValue.Back))
                {
                    value = puzzle[i, j].value;
                }
                if ((value == QuadValue.Front || value == QuadValue.Back) &&
                    (puzzle[i, j].value == QuadValue.Front || puzzle[i, j].value == QuadValue.Back) &&
                    value != puzzle[i, j].value)
                {
                    pass = false;
                    break;
                }
            }
        }
        return pass;
    }

    public bool IsLose(IPuzzle puzzle)
    {
        return false;
    }
}
