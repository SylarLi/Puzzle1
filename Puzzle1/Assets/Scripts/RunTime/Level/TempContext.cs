using Core;
using DG.Tweening;
using UnityEngine;

public class TempContext : MonoBehaviour
{
    private ILevel level;

    private LevelView levelView;

    private void Awake()
    {
        InitContext();
        InitLevel();
    }

    private void InitContext()
    {
        DOTween.Init(false, false, LogBehaviour.Default).SetCapacity(100, 20);
    }

    private void InitLevel()
    {
        int rows = 7;
        int columns = 7;
        int complication = 3;

        level = new Level(rows, columns);

        IPuzzle puzzle = level.puzzle;
        for (int i = 0; i < puzzle.rows; i++)
        {
            for (int j = 0; j < puzzle.columns; j++)
            {
                puzzle[i, j] = new Quad(i, j, 1);
            }
        }

        System.Random random = new System.Random();
        IResolver resolver = level.resolver;
        for (int i = 0; i < complication; i++)
        {
            IOperation operation = new Operation(random.Next(puzzle.rows), random.Next(puzzle.columns));
            resolver.Apply(puzzle, operation);
        }

        GameObject levelgo = new GameObject("level");
        levelView = levelgo.AddComponent<LevelView>();
        levelView.level = level;
    }
}
