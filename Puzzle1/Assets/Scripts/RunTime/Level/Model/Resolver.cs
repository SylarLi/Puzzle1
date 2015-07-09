using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Resolver : IResolver
{
    public void ResolveTouch(IPuzzle puzzle, IOperation op)
    {
        switch (op.type)
        {
            case OpType.TouchClick:
                {
                    ResolveTouchClick(puzzle, op);
                    break;
                }
            case OpType.TouchStart:
                {
                    ResolveTouchStart(puzzle, op);
                    break;
                }
            case OpType.TouchEnd:
                {
                    ResolveTouchEnd(puzzle, op);
                    break;
                }
        }
    }

    private void ResolveTouchClick(IPuzzle puzzle, IOperation op)
    {
        List<Tween> playings = DOTween.PlayingTweens();
        if (playings != null)
        {
            playings.ForEach((Tween each) =>
            {
                if (each.id.ToString().StartsWith(Style.QuadUnifiedRotateId))
                {
                    each.Kill(true);
                }
            });
        }
        Sequence sequence = DOTween.Sequence();
        IQuad[] rowQuads = puzzle.GetRowQuads(op.row);
        for (int i = op.column - 1; i >= 0; i--)
        {
            // 左舷依次翻转
            IQuad quad = rowQuads[i];
            float delay = (Mathf.Abs(quad.column - op.column) - 1) * Style.QuadRollDelay;
            if (quad.value == QuadValue.Block)
            {
                Sequence s = DOTween.Sequence()
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(0, Style.QuadShakeAngle, 0), Style.QuadShakeDuration * 0.25f))
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles - new Vector3(0, Style.QuadShakeAngle, 0), Style.QuadShakeDuration * 0.25f))
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles, Style.QuadShakeDuration * 0.5f).SetEase(Ease.OutBack));
                sequence.Insert(delay, s);
                break;
            }
            else
            {
                Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(0, Style.QuadRollAngle, 0), Style.QuadRollDuration)
                                   .SetId(Style.QuadUnifiedRotateId + "_" + quad.row + "_" + quad.column)
                                   .SetEase(Ease.OutBack);
                sequence.Insert(delay, t);
            }
        }
        for (int i = op.column + 1, len = rowQuads.Length; i < len; i++)
        {
            // 右舷依次翻转
            IQuad quad = rowQuads[i];
            float delay = (Mathf.Abs(quad.column - op.column) - 1) * Style.QuadRollDelay;
            if (quad.value == QuadValue.Block)
            {
                Sequence s = DOTween.Sequence()
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(0, -Style.QuadShakeAngle, 0), Style.QuadShakeDuration * 0.25f))
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles - new Vector3(0, -Style.QuadShakeAngle, 0), Style.QuadShakeDuration * 0.25f))
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles, Style.QuadShakeDuration * 0.5f).SetEase(Ease.OutBack));
                sequence.Insert(delay, s);
                break;
            }
            else
            {
                Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(0, -Style.QuadRollAngle, 0), Style.QuadRollDuration)
                                   .SetId(Style.QuadUnifiedRotateId + "_" + quad.row + "_" + quad.column)
                                   .SetEase(Ease.OutBack);
                sequence.Insert(delay, t);
            }
        }
        IQuad[] columnQuads = puzzle.GetColumnQuads(op.column);
        for (int i = op.row - 1; i >= 0; i--)
        {
            // 上侧依次翻转
            IQuad quad = columnQuads[i];
            float delay = (Mathf.Abs(quad.row - op.row) - 1) * Style.QuadRollDelay;
            if (quad.value == QuadValue.Block)
            {
                Sequence s = DOTween.Sequence()
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(-Style.QuadShakeAngle, 0, 0), Style.QuadShakeDuration * 0.25f))
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles - new Vector3(-Style.QuadShakeAngle, 0, 0), Style.QuadShakeDuration * 0.25f))
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles, Style.QuadShakeDuration * 0.5f).SetEase(Ease.OutBack));
                sequence.Insert(delay, s);
                break;
            }
            else
            {
                Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(-Style.QuadRollAngle, 0, 0), Style.QuadRollDuration)
                                   .SetId(Style.QuadUnifiedRotateId + "_" + quad.row + "_" + quad.column)
                                   .SetEase(Ease.OutBack);
                sequence.Insert(delay, t);
            }
        }
        for (int i = op.row + 1, len = columnQuads.Length; i < len; i++)
        {
            // 下侧依次翻转
            IQuad quad = columnQuads[i];
            float delay = (Mathf.Abs(quad.row - op.row) - 1) * Style.QuadRollDelay;
            if (quad.value == QuadValue.Block)
            {
                Sequence s = DOTween.Sequence()
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(Style.QuadShakeAngle, 0, 0), Style.QuadShakeDuration * 0.25f))
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles - new Vector3(Style.QuadShakeAngle, 0, 0), Style.QuadShakeDuration * 0.25f))
                                    .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles, Style.QuadShakeDuration * 0.5f).SetEase(Ease.OutBack));
                sequence.Insert(delay, s);
                break;
            }
            else
            {
                Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(Style.QuadRollAngle, 0, 0), Style.QuadRollDuration)
                                   .SetId(Style.QuadUnifiedRotateId + "_" + quad.row + "_" + quad.column)
                                   .SetEase(Ease.OutBack);
                sequence.Insert(delay, t);
            }
        }
        puzzle.touchEnable = false;
        sequence.OnComplete(() =>
        {
            ResolveData(puzzle, op);
            if (IsSolved(puzzle))
            {
                puzzle.solved = true;
            }
            else
            {
                puzzle.touchEnable = true;
            }
        });
    }

    private void ResolveTouchStart(IPuzzle puzzle, IOperation op)
    {
        IQuad quad = puzzle[op.row, op.column];
        string tid = Style.QuadUnifiedScaleId + "_" + quad.row + "_" + quad.column;
        List<Tween> playings = DOTween.TweensById(tid, true);
        if (playings != null)
        {
            playings.ForEach((Tween each) => each.Kill(true));
        }
        DOTween.To(
            () => quad.localScale,
            x => quad.localScale = x,
            new Vector3(Style.QuadTouchScale, Style.QuadTouchScale, 1),
            Style.QuadTouchScaleDuration
        ).SetId(tid);
    }

    private void ResolveTouchEnd(IPuzzle puzzle, IOperation op)
    {
        IQuad quad = puzzle[op.row, op.column];
        string tid = Style.QuadUnifiedScaleId + "_" + quad.row + "_" + quad.column;
        List<Tween> playings = DOTween.TweensById(tid, true);
        if (playings != null)
        {
            playings.ForEach((Tween each) => each.Kill(true));
        }
        DOTween.To(
            () => quad.localScale,
            x => quad.localScale = x,
            Vector3.one,
            Style.QuadTouchScaleDuration
        ).SetId(tid);
    }

    public void ResolveData(IPuzzle puzzle, IOperation op)
    {
        IQuad[] rowQuads = puzzle.GetRowQuads(op.row);
        for (int i = op.column - 1; i >= 0; i--)
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

    public bool IsSolved(IPuzzle puzzle)
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
}
