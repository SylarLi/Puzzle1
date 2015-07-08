using Core;
using UnityEngine;

/// <summary>
/// TouchStart-->TouchEnd-->TouchClick
/// </summary>
public class LevelInput : MonoListener
{
    private const float RayInterval = 0.1f;
    private float rayTick = 0;

    private Collider touchStart;

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            TouchPhase phase = Input.GetTouch(0).phase;
            if (phase == TouchPhase.Began)
            {
                touchStart = TryRayCastHit(Input.GetTouch(0).position);
                if (touchStart != null)
                {
                    DispatchEvent(new LevelInputEvent(LevelInputEvent.TouchStart, touchStart));
                }
            }
            else if (phase == TouchPhase.Ended)
            {
                DispatchEvent(new LevelInputEvent(LevelInputEvent.TouchEnd, touchStart));
                if (touchStart != null)
                {
                    Collider currentTouch = TryRayCastHit(Input.GetTouch(0).position);
                    if (currentTouch != null)
                    {
                        if (currentTouch == touchStart)
                        {
                            DispatchEvent(new LevelInputEvent(LevelInputEvent.TouchClick, currentTouch));
                        }
                    }
                }
                touchStart = null;
            }
            else if (touchStart != null && phase == TouchPhase.Moved)
            {
                if ((rayTick += Time.deltaTime) > RayInterval)
                {
                    rayTick = 0;
                    Collider currentTouch = TryRayCastHit(Input.GetTouch(0).position);
                    if (currentTouch != touchStart)
                    {
                        DispatchEvent(new LevelInputEvent(LevelInputEvent.TouchEnd, touchStart));
                        touchStart = null;
                    }
                }
            }
        }
#endif
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = TryRayCastHit(Input.mousePosition);
            if (touchStart != null)
            {
                DispatchEvent(new LevelInputEvent(LevelInputEvent.TouchStart, touchStart));
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DispatchEvent(new LevelInputEvent(LevelInputEvent.TouchEnd, touchStart));
            if (touchStart != null)
            {
                Collider currentTouch = TryRayCastHit(Input.mousePosition);
                if (currentTouch != null)
                {
                    if (currentTouch == touchStart)
                    {
                        DispatchEvent(new LevelInputEvent(LevelInputEvent.TouchClick, currentTouch));
                    }
                }
            }
            touchStart = null;
        }
        else if (touchStart != null && Input.GetMouseButton(0))
        {
            if ((rayTick += Time.deltaTime) > RayInterval)
            {
                rayTick = 0;
                Collider currentTouch = TryRayCastHit(Input.mousePosition);
                if (currentTouch != touchStart)
                {
                    DispatchEvent(new LevelInputEvent(LevelInputEvent.TouchEnd, touchStart));
                    touchStart = null;
                }
            }
        }
#endif
    }

    private Collider TryRayCastHit(Vector2 mousePosition)
    {
        Collider collider = null;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, PuzzleView.PuzzleDepth + 1))
        {
            collider = hit.collider;
        }
        return collider;
    }
}
