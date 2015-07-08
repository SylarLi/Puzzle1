using Core;
using UnityEngine;

public class LevelController
{
    private int rank = 6;

    private ILevel level;

    private LevelInput levelInput;

    private PuzzleView puzzleView;

    private bool puzzleTouchEnable;

    public LevelController()
    {
        InitListener();
        NextLevel();
    }

    private void InitListener()
    {
        GameObject inputgo = new GameObject("InputListener");
        levelInput = inputgo.AddComponent<LevelInput>();
        levelInput.AddEventListener(LevelInputEvent.TouchStart, TouchStartHandler);
        levelInput.AddEventListener(LevelInputEvent.TouchEnd, TouchEndHandler);
        levelInput.AddEventListener(LevelInputEvent.TouchClick, TouchClickHandler);
    }

    private void NextLevel()
    {
        if (puzzleView != null)
        {
            GameObject.Destroy(puzzleView.gameObject);
            puzzleView = null;
        }

        level = new Level();
        level.MakePuzzle(PuzzleParams.GetPuzzleParamsByRank(rank++));

        GameObject puzzlego = new GameObject("Puzzle");
        puzzlego.transform.localPosition = new Vector3(0, 0, PuzzleView.PuzzleDepth);
        puzzleView = puzzlego.AddComponent<PuzzleView>();
        puzzleView.puzzle = level.puzzle;

        puzzleTouchEnable = true;
    }

    private void TouchStartHandler(IEvent e)
    {
        QuadView quadView = (e.data as Collider).GetComponent<QuadView>();
        quadView.TouchStart();
    }

    private void TouchEndHandler(IEvent e)
    {
        Collider quadViewCollider = e.data as Collider;
        if (quadViewCollider != null)
        {
            QuadView quadView = quadViewCollider.GetComponent<QuadView>();
            quadView.TouchEnd();
        }
    }

    private void TouchClickHandler(IEvent e)
    {
        if (puzzleTouchEnable)
        {
            QuadView quadView = (e.data as Collider).GetComponent<QuadView>();
            quadView.TouchClick();
            IOperation op = new Operation(quadView.quad.row, quadView.quad.column);
            level.record.Push(op);
            puzzleTouchEnable = false;
            puzzleView.Roll(op, () =>
            {
                level.resolver.Apply(level.puzzle, op);
                if (level.resolver.IsWin(level.puzzle))
                {
                    NextLevel();
                }
                else
                {
                    puzzleTouchEnable = true;
                }
            });
        }
    }
}
