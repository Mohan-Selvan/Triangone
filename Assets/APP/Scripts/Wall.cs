using UnityEngine;

using Pixelplacement;
using Pixelplacement.TweenSystem;

public class Wall : MonoBehaviour
{
    const string COLOR_PROPERTY = "_BaseColor";

    [Header("References")]
    [SerializeField] MeshFilter meshFilter = null;
    [SerializeField] MeshRenderer meshRenderer = null;
    [SerializeField] PolygonCollider2D polygonCollider2D = null;

    [Header("Testing only")]
    [SerializeField] private bool _isSafe = true;

    private TweenBase _tween = null;

    private Material _material = null;
    private Mesh _mesh = null;

    //Helpers
    GameSettings GameSettings => GameSettings.Instance;

    public bool IsSafe { get => _isSafe; private set => _isSafe = value; }

    private void Start()
    {
        if(meshRenderer.material == null)
        {
            Debug.LogError("No material on mesh renderer", this.gameObject);
            return;
        }
            
        _material = new Material(meshRenderer.material);
        _material.hideFlags = HideFlags.HideAndDontSave;

        meshRenderer.material = _material;

        _mesh = meshFilter.mesh;
    }

    public void SetWallSafeState(bool isSafe, bool animate = true)
    {
        _isSafe = isSafe;

        if (!animate)
        {
            Color targetColor = _isSafe ? GameSettings.WallSafeColor : GameSettings.WallDangerColor;

            _material.SetColor(COLOR_PROPERTY, targetColor);

        }
        else 
        {
            if(_tween != null && _tween.Status == Tween.TweenStatus.Running)
            {
                _tween.Stop();
            }

            Color currentColor = _material.GetColor(COLOR_PROPERTY);
            Color targetColor = _isSafe ? GameSettings.WallSafeColor : GameSettings.WallDangerColor;

            _tween = Tween.ShaderColor(_material, COLOR_PROPERTY, startValue: currentColor, endValue: targetColor,
                duration: 0.3f, delay: 0f, easeCurve: Tween.EaseInOut,
                startCallback: () =>
                {
                    try
                    {

                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Exception handling tween : {e}");
                    }
                },

                completeCallback: () =>
                {
                    try
                    {
                        
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Exception handling tween : {e}");
                    }
                });
        }
    }

    #region Getters

    public Mesh GetMesh()
    {
        return _mesh;
    }

    public PolygonCollider2D GetPolygonCollider2D()
    {
        return polygonCollider2D;
    }

    #endregion

}
