using DG.Tweening;
using UnityEngine;

public class GameContext : MonoBehaviour
{
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
        new LevelController();
    }
}
