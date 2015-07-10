using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
        IQuad quad = puzzle[op.row, op.column];
        if (quad.value == QuadValue.Front || quad.value == QuadValue.Back)
        {
            List<Tween> playings = DOTween.PlayingTweens();
            if (playings != null)
            {
                playings.ForEach((Tween each) =>
                {
                    if (each.id != null && each.id.ToString().StartsWith(Style.QuadUnifiedRotateId))
                    {
                        each.Kill(true);
                    }
                });
            }
            puzzle.touchEnable = false;
            TweenDepot depot = new TweenDepot();
            ResolvePresent(depot, puzzle, op, op, 0);
            Sequence sequence = depot.ToSequence();
            sequence.SetId(Style.QuadUnifiedRotateId);
            sequence.OnComplete(() =>
            {
                ResolveTouchData(puzzle, op);
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
    }

    private void ResolvePresent(TweenDepot depot, IPuzzle puzzle, IOperation origin, IOperation op, float delay)
    {
        IQuad[] rowQuads = puzzle.GetRowQuads(op.row);
        if ((op.direction & QuadValue.Left) > 0)
        {
            for (int i = op.column - 1; i >= 0; i--)
            {
                // 左舷依次翻转
                IQuad quad = rowQuads[i];
                float qdelay = (Mathf.Abs(quad.column - op.column) - 1) * Style.QuadRollDelay;
                if (quad.value == QuadValue.Block)
                {
                    //DOTween.Sequence().Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(0, Style.QuadShakeAngle, 0), Style.QuadShakeDuration * 0.25f))
                    //                  .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles + new Vector3(0, -Style.QuadShakeAngle, 0), Style.QuadShakeDuration * 0.25f))
                    //                  .Append(DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, quad.localEulerAngles, Style.QuadShakeDuration * 0.5f).SetEase(Ease.OutBack))
                    //                  .SetDelay(delay + qdelay);
                    break;
                }
                else if ((quad.value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                {
                    VisionSpark spark = new VisionSpark(VisionSparkType.Sprinkle);
                    spark.delay = delay + qdelay;
                    quad.Spark(spark);
                    ResolvePresent(depot, puzzle, origin, new Operation(OpType.TouchClick, quad.row, quad.column, quad.value), delay + qdelay + Style.QuadRollDuration);
                    break;
                }
                else
                {
                    string tid = Style.QuadUnifiedRotateId + "_" + quad.row + "_" + quad.column;
                    TweenInfo lt = depot.GetFirst(tid);
                    if (lt != null && Mathf.Abs(lt.atPosition - delay - qdelay) < Style.QuadRollDuration)
                    {
                        lt.Kill();
                        Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, new Vector3(0, 0, Style.QuadConflictAngle), Style.QuadConflictDuration)
                                           .SetEase(Ease.OutBack)
                                           .SetRelative();
                        depot.Add(delay + qdelay, t);
                    }
                    else
                    {
                        Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, new Vector3(0, Style.QuadRollAngle, 0), Style.QuadRollDuration)
                                        .SetId(tid)
                                        .SetEase(Ease.OutBack)
                                        .SetRelative();
                        depot.Add(delay + qdelay, t);
                    }
                }
            }
        }
        if ((op.direction & QuadValue.Right) > 0)
        {
            for (int i = op.column + 1, len = rowQuads.Length; i < len; i++)
            {
                // 右舷依次翻转
                IQuad quad = rowQuads[i];
                float qdelay = (Mathf.Abs(quad.column - op.column) - 1) * Style.QuadRollDelay;
                if (quad.value == QuadValue.Block)
                {
                    break;
                }
                else if ((quad.value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                {
                    VisionSpark spark = new VisionSpark(VisionSparkType.Sprinkle);
                    spark.delay = delay + qdelay;
                    quad.Spark(spark);
                    ResolvePresent(depot, puzzle, origin, new Operation(OpType.TouchClick, quad.row, quad.column, quad.value), delay + qdelay + Style.QuadRollDuration);
                    break;
                }
                else
                {
                    string tid = Style.QuadUnifiedRotateId + "_" + quad.row + "_" + quad.column;
                    TweenInfo lt = depot.GetFirst(tid);
                    if (lt != null && Mathf.Abs(lt.atPosition - delay - qdelay) < Style.QuadRollDuration)
                    {
                        lt.Kill();
                        Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, new Vector3(0, 0, -Style.QuadConflictAngle), Style.QuadConflictDuration)
                                           .SetEase(Ease.OutBack)
                                           .SetRelative();
                        depot.Add(delay + qdelay, t);
                    }
                    else
                    {
                        Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, new Vector3(0, -Style.QuadRollAngle, 0), Style.QuadRollDuration)
                                       .SetId(tid)
                                       .SetEase(Ease.OutBack)
                                       .SetRelative();
                        depot.Add(delay + qdelay, t);
                    }
                }
            }
        }
        IQuad[] columnQuads = puzzle.GetColumnQuads(op.column);
        if ((op.direction & QuadValue.Up) > 0)
        {
            for (int i = op.row - 1; i >= 0; i--)
            {
                // 上侧依次翻转
                IQuad quad = columnQuads[i];
                float qdelay = (Mathf.Abs(quad.row - op.row) - 1) * Style.QuadRollDelay;
                if (quad.value == QuadValue.Block)
                {
                    break;
                }
                else if ((quad.value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                {
                    VisionSpark spark = new VisionSpark(VisionSparkType.Sprinkle);
                    spark.delay = delay + qdelay;
                    quad.Spark(spark);
                    ResolvePresent(depot, puzzle, origin, new Operation(OpType.TouchClick, quad.row, quad.column, quad.value), delay + qdelay + Style.QuadRollDuration);
                    break;
                }
                else
                {
                    string tid = Style.QuadUnifiedRotateId + "_" + quad.row + "_" + quad.column;
                    TweenInfo lt = depot.GetFirst(tid);
                    if (lt != null && Mathf.Abs(lt.atPosition - delay - qdelay) < Style.QuadRollDuration)
                    {
                        lt.Kill();
                        Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, new Vector3(0, 0, Style.QuadConflictAngle), Style.QuadConflictDuration)
                                           .SetEase(Ease.OutBack)
                                           .SetRelative();
                        depot.Add(delay + qdelay, t);
                    }
                    else
                    {
                        Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, new Vector3(-Style.QuadRollAngle, 0, 0), Style.QuadRollDuration)
                                       .SetId(tid)
                                       .SetEase(Ease.OutBack)
                                       .SetRelative();
                        depot.Add(delay + qdelay, t);
                    }
                }
            }
        }
        if ((op.direction & QuadValue.Down) > 0)
        {
            for (int i = op.row + 1, len = columnQuads.Length; i < len; i++)
            {
                // 下侧依次翻转
                IQuad quad = columnQuads[i];
                float qdelay = (Mathf.Abs(quad.row - op.row) - 1) * Style.QuadRollDelay;
                if (quad.value == QuadValue.Block)
                {
                    break;
                }
                else if ((quad.value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                {
                    VisionSpark spark = new VisionSpark(VisionSparkType.Sprinkle);
                    spark.delay = delay + qdelay;
                    quad.Spark(spark);
                    ResolvePresent(depot, puzzle, origin, new Operation(OpType.TouchClick, quad.row, quad.column, quad.value), delay + qdelay + Style.QuadRollDuration);
                    break;
                }
                else
                {
                    string tid = Style.QuadUnifiedRotateId + "_" + quad.row + "_" + quad.column;
                    TweenInfo lt = depot.GetFirst(tid);
                    if (lt != null && Mathf.Abs(lt.atPosition - delay - qdelay) < Style.QuadRollDuration)
                    {
                        lt.Kill();
                        Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, new Vector3(0, 0, -Style.QuadConflictAngle), Style.QuadConflictDuration)
                                           .SetEase(Ease.OutBack)
                                           .SetRelative();
                        depot.Add(delay + qdelay, t);
                    }
                    else
                    {
                        Tweener t = DOTween.To(() => quad.localEulerAngles, x => quad.localEulerAngles = x, new Vector3(Style.QuadRollAngle, 0, 0), Style.QuadRollDuration)
                                       .SetId(tid)
                                       .SetEase(Ease.OutBack)
                                       .SetRelative();
                        depot.Add(delay + qdelay, t);
                    }
                }
            }
        }
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

    public void ResolveTouchData(IPuzzle puzzle, IOperation op)
    {
        IQuad quad = puzzle[op.row, op.column];
        if (quad.value == QuadValue.Front || quad.value == QuadValue.Back)
        {
            ResolveData(puzzle, op, op);
        }
    }

    private void ResolveData(IPuzzle puzzle, IOperation origin, IOperation op)
    {
        IQuad[] rowQuads = puzzle.GetRowQuads(op.row);
        if ((op.direction & QuadValue.Left) > 0)
        {
            ResolveDataLine(puzzle, origin, rowQuads, op.column - 1, -1, -1);
        }
        if ((op.direction & QuadValue.Right) > 0)
        {
            ResolveDataLine(puzzle, origin, rowQuads, op.column + 1, rowQuads.Length, 1);
        }
        IQuad[] columnQuads = puzzle.GetColumnQuads(op.column);
        if ((op.direction & QuadValue.Up) > 0)
        {
            ResolveDataLine(puzzle, origin, columnQuads, op.row - 1, -1, -1);
        }
        if ((op.direction & QuadValue.Down) > 0)
        {
            ResolveDataLine(puzzle, origin, columnQuads, op.row + 1, columnQuads.Length, 1);
        }
    }

    private void ResolveDataLine(IPuzzle puzzle, IOperation origin, IQuad[] quads, int from, int to, int step)
    {
        // from闭to开
        for (int i = from; i != to; i += step)
        {
            if (quads[i].value == QuadValue.Block)
            {
                break;
            }
            else if ((quads[i].value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
            {
                ResolveData(puzzle, origin, new Operation(OpType.TouchClick, quads[i].row, quads[i].column, quads[i].value));
                break;
            }
            else
            {
                quads[i].value = (QuadValue)(QuadValue.Back - quads[i].value);
            }
        }
    }

    public bool ResolveIsLoop(IPuzzle puzzle)
    {
        bool result = false;
        for (int i = 0; i < puzzle.rows; i++)
        {
            for (int j = 0; j < puzzle.columns; j++)
            {
                if ((puzzle[i, j].value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
                {
                    if (IsLoop(new List<IQuad>(), puzzle, new Operation(OpType.TouchClick, i, j, puzzle[i, j].value)))
                    {
                        result = true;
                        break;
                    }
                }
            }
        }
        return result;
    }

    private bool IsLoop(List<IQuad> arrows, IPuzzle puzzle, IOperation op)
    {
        IQuad[] rowQuads = puzzle.GetRowQuads(op.row);
        if ((op.direction & QuadValue.Left) > 0)
        {
            if (IsLoopLine(arrows, puzzle, rowQuads, op.column - 1, -1, -1))
            {
                return true;
            }
        }
        if ((op.direction & QuadValue.Right) > 0)
        {
            if (IsLoopLine(arrows, puzzle, rowQuads, op.column + 1, rowQuads.Length, 1))
            {
                return true;
            }
        }
        IQuad[] columnQuads = puzzle.GetColumnQuads(op.column);
        if ((op.direction & QuadValue.Up) > 0)
        {
            if (IsLoopLine(arrows, puzzle, columnQuads, op.row - 1, -1, -1))
            {
                return true;
            }
        }
        if ((op.direction & QuadValue.Down) > 0)
        {
            if (IsLoopLine(arrows, puzzle, columnQuads, op.row + 1, columnQuads.Length, 1))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsLoopLine(List<IQuad> arrows, IPuzzle puzzle, IQuad[] quads, int from, int to, int step)
    {
        bool result = false;
        // from闭to开
        for (int i = from; i != to; i += step)
        {
            if (quads[i].value == QuadValue.Block)
            {
                break;
            }
            else if ((quads[i].value & (QuadValue.Left | QuadValue.Right | QuadValue.Up | QuadValue.Down)) > 0)
            {
                if (arrows.IndexOf(quads[i]) == -1)
                {
                    arrows.Add(quads[i]);
                    result = IsLoop(arrows, puzzle, new Operation(OpType.TouchClick, quads[i].row, quads[i].column, quads[i].value));
                }
                else
                {
                    result = true;
                }
                break;
            }
        }
        return result;
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
