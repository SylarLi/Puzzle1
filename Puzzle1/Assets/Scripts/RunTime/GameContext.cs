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
        DOTween.Init(false, false, LogBehaviour.Verbose).SetCapacity(100, 20);
        SimplePool.inst.Bind<Mesh>(SimplePoolItemType.QuadMesh, Style.GetQuadMesh);
        SimplePool.inst.Bind<Mesh>(SimplePoolItemType.ArrowMesh, Style.GetArrowMesh);
    }

    private void InitLevel()
    {
        new LevelController();
    }
}
