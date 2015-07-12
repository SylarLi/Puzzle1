using Core;
using UnityEngine;

public interface IVision : IEventDispatcher
{
    float alpha { get; }

    float parentAlpha { get; set; }

    float localAlpha { get; set; }

    Vector3 localPosition { get; set; }

    Vector3 localEulerAngles { get; set; }

    Vector3 localScale { get; set; }

    VisionMaterial material { get; set; }

    Vector2[] uvOffsets { get; set; }           // 正面UVOffse+反面UVOffset

    bool touchEnable { get; set; }

    void Spark(VisionSpark spark);
}
