using Core;
using UnityEngine;
using DG.Tweening;

public class LevelController
{
    private int rank = 11;

    private LevelInput levelInput;

    private ILevel level;

    private PuzzleView puzzleView;

    public LevelController()
    {
        InitListener();
        EaseToNextLevel();
    }

    private void InitListener()
    {
        GameObject inputgo = new GameObject("InputListener");
        levelInput = inputgo.AddComponent<LevelInput>();
        levelInput.AddEventListener(LevelInputEvent.TouchStart, TouchStartHandler);
        levelInput.AddEventListener(LevelInputEvent.TouchEnd, TouchEndHandler);
        levelInput.AddEventListener(LevelInputEvent.TouchClick, TouchClickHandler);
        levelInput.AddEventListener(LevelInputEvent.Key, KeyHandler);
    }

    private void EaseToNextLevel()
    {
        ILevel memory = level;
        PuzzleView reality = puzzleView;

        Sequence sequence = DOTween.Sequence();

        if (memory != null)
        {
            // 淡出
            Tween out1 = DOTween.To(() => memory.puzzle.localPosition, x => memory.puzzle.localPosition = x, memory.puzzle.localPosition + new Vector3(0, 0, -20), 1)
                                .SetEase(Ease.InOutCubic);
            Tween out2 = DOTween.To(() => memory.puzzle.localAlpha, x => memory.puzzle.localAlpha = x, 0, 1)
                                .SetEase(Ease.InOutCubic);
            sequence.Join(out1).Join(out2);
        }

        // Next
        NextLevel();

        // 淡入
        Tween in1 = DOTween.To(() => level.puzzle.localPosition, x => level.puzzle.localPosition = x, level.puzzle.localPosition + new Vector3(0, 0, 20), 1)
                            .SetEase(Ease.InOutCubic).From();
        Tween in2 = DOTween.To(() => level.puzzle.localAlpha, x => level.puzzle.localAlpha = x, 0, 1)
                            .SetEase(Ease.InOutCubic).From();
        sequence.Join(in1).Join(in2);

        sequence.OnComplete(() =>
        {
            if (reality != null)
            {
                GameObject.Destroy(reality.gameObject);
            }
            level.puzzle.touchEnable = true;
        });
    }

    private void NextLevel()
    {
        level = new Level();
        level.MakePuzzle(PuzzleParams.GetPuzzleParamsByRank(rank++));
        level.puzzle.localPosition = new Vector3(0, 0, Style.PuzzleDepth);
        level.puzzle.AddEventListener(PuzzleEvent.SolveChange, SolveChangeHandler);

        GameObject puzzlego = new GameObject("Puzzle");
        puzzleView = puzzlego.AddComponent<PuzzleView>();
        puzzleView.data = level.puzzle;
    }

    private void SolveChangeHandler(IEvent e)
    {
        EaseToNextLevel();
    }

    private void TouchStartHandler(IEvent e)
    {
        IQuad quad = (e.data as Collider).GetComponent<QuadView>().data;
        level.resolver.ResolveTouch(level.puzzle, new Operation(OpType.TouchStart, quad.row, quad.column));
    }

    private void TouchEndHandler(IEvent e)
    {
        Collider quadViewCollider = e.data as Collider;
        if (quadViewCollider != null)
        {
            IQuad quad = quadViewCollider.GetComponent<QuadView>().data;
            level.resolver.ResolveTouch(level.puzzle, new Operation(OpType.TouchEnd, quad.row, quad.column));
        }
    }

    private void TouchClickHandler(IEvent e)
    {
        if (level.puzzle.touchEnable)
        {
            IQuad quad = (e.data as Collider).GetComponent<QuadView>().data;
            IOperation op = new Operation(OpType.TouchClick, quad.row, quad.column);
            level.record.Push(op);
            level.resolver.ResolveTouch(level.puzzle, op);
        }
    }

    private void KeyHandler(IEvent e)
    {
        switch ((KeyCode)e.data)
        {
            case KeyCode.R:
                {
                    if (level.puzzle.touchEnable)
                    {
                        level.resolver.ResolveTouch(level.puzzle, level.record.Pop());
                    }
                    break;
                }
        }
    }
}
