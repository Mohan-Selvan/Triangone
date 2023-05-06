using UnityEngine;
using Pixelplacement;

public class GameSettings : Singleton<GameSettings>
{
    [SerializeField] private Color _blockHighlightColor = Color.white;
    [SerializeField] private Color _blockDefaultColor = Color.blue;
    [SerializeField] private Color _blockLockedColor = Color.magenta;

    [SerializeField] private Color _wallSafeColor = Color.green;
    [SerializeField] private Color _wallDangerColor = Color.red;

    [SerializeField] LayerMask blockLayerMask = default;
    [SerializeField] LayerMask ringLayerMask = default;

    public Color BlockHighlightColor { get => _blockHighlightColor; set => _blockHighlightColor = value; }
    public Color BlockDefaultColor { get => _blockDefaultColor; set => _blockDefaultColor = value; }
    public Color BlockLockedColor { get => _blockLockedColor; set => _blockLockedColor = value; }

    public LayerMask BlockLayerMask { get => blockLayerMask; set => blockLayerMask = value; }
    public LayerMask RingLayerMask { get => ringLayerMask; set => ringLayerMask = value; }
    public Color WallSafeColor { get => _wallSafeColor; set => _wallSafeColor = value; }
    public Color WallDangerColor { get => _wallDangerColor; set => _wallDangerColor = value; }
}
