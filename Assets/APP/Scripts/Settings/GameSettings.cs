using UnityEngine;
using Pixelplacement;

public class GameSettings : Singleton<GameSettings>
{
    [SerializeField] private Color _blockHighlightColor = Color.white;
    [SerializeField] private Color _blockDefaultColor = Color.blue;

    [SerializeField] LayerMask blockLayerMask = default;
    [SerializeField] LayerMask ringLayerMask = default;

    public Color BlockHighlightColor { get => _blockHighlightColor; set => _blockHighlightColor = value; }
    public Color BlockDefaultColor { get => _blockDefaultColor; set => _blockDefaultColor = value; }
    public LayerMask BlockLayerMask { get => blockLayerMask; set => blockLayerMask = value; }
    public LayerMask RingLayerMask { get => ringLayerMask; set => ringLayerMask = value; }
}
