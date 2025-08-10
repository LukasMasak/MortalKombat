using UnityEngine;

public struct NormalMapSettings
{
    public Texture2D sourceTexture;
    public int strengthEdges;
    public int blurEdgesRadius;
    public int strengthBorder;
    public int blurBorderRadius;
    public int softenBorder;
    public float slopePercentageBorder;
    public int finalBlurRadius;
}
