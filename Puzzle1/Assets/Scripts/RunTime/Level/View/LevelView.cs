using Core;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    public const float LevelDepth = 20;

    private ILevel _level;

    private PuzzleView _puzzleView;

    private LevelInput _levelInput;

    private void Awake()
    {
        _levelInput = gameObject.AddComponent<LevelInput>();
        _levelInput.AddEventListener(LevelInputEvent.TouchStart, TouchStartHandler);
        _levelInput.AddEventListener(LevelInputEvent.TouchEnd, TouchEndHandler);
        _levelInput.AddEventListener(LevelInputEvent.TouchClick, TouchClickHandler);
    }

    public ILevel level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
            UpdateLevelView();
        }
    }

    private void UpdateLevelView()
    {
        GameObject puzzlego = new GameObject("puzzle");
        puzzlego.transform.parent = transform;
        puzzlego.transform.localPosition = new Vector3(0, 0, PuzzleView.PuzzleDepth);
        _puzzleView = puzzlego.AddComponent<PuzzleView>();
        _puzzleView.puzzle = _level.puzzle;
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
        if (_level.record.opValid)
        {
            QuadView quadView = (e.data as Collider).GetComponent<QuadView>();
            quadView.TouchClick();
            IOperation op = new Operation(quadView.quad.row, quadView.quad.column);
            _level.record.Push(op);
            _level.record.opValid = false;
            _puzzleView.Roll(op, () =>
            {
                _level.resolver.Apply(_level.puzzle, op);
                _level.record.opValid = true;
            });
        }
    }
}
