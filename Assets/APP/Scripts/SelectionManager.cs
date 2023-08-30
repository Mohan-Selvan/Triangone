using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class SelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RingHandler ringHandler = null;

    [Header("Settings")]
    [SerializeField] float blockMaxMoveSpeed = 100f;

    [Header("Debug only")]
    [SerializeField] bool enableUpdate = false;

    [Header("Testing only")]
    [SerializeField] Block currentSelectedBlock = null;
    [SerializeField] Vector3 offset = default;

    private Camera mainCamera = null;

    //Trackers
    Collider2D[] colliders = default;

    System.Action<Block> OnBlockCleared = null;
    System.Action<Block> OnBlockBroken = null;

    public void Initialize(System.Action<Block> onBlockCleared, System.Action<Block> onBlockBroken)
    {
        mainCamera = Camera.main;

        colliders = new Collider2D[2];

        this.OnBlockCleared = onBlockCleared;
        this.OnBlockBroken = onBlockBroken;
    }

    internal void Deinitialize()
    {

    }

    internal void EnableSelection(bool value)
    {
        enableUpdate = value;
    }

    private void Update()
    {
        if((!enableUpdate)) { return; }

        // Is trying to select
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.yellow, duration: 0.3f);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, GameSettings.Instance.BlockLayerMask);
            
            if(hit.collider != null)
            {
                Block block = hit.collider.GetComponentInParent<Block>();

                if (block != null)
                {
                    Debug.Log($"Selecting Block : {block.BlockID}!");

                    if(TrySelectBlock(block))
                    {
                        offset = Helpers.GetWorldMousePosition(Input.mousePosition, mainCamera) - currentSelectedBlock.transform.position;
                    }
                    else
                    {
                        DeselectCurrentBlock();
                    }
                }
                else
                {
                    Debug.LogError("Selected object does not have a block component!");
                }
            }
            else
            {
                Debug.Log("No object hit");
                DeselectCurrentBlock();
            }
        }
        else if(Input.GetMouseButton(0)) //User holding a block
        {
            if(currentSelectedBlock != null)
            {
                Vector3 worldMousePosition = Helpers.GetWorldMousePosition(Input.mousePosition, mainCamera);

                Vector3 currentPosition = currentSelectedBlock.transform.position;
                Vector3 targetPosition = worldMousePosition - offset;

                currentSelectedBlock.transform.position = Vector3.Lerp(currentPosition, targetPosition, blockMaxMoveSpeed * Time.deltaTime);
            }
        }

        if(currentSelectedBlock != null)
        {
            int blockCollisionCount = currentSelectedBlock.IsBlockColliding(GameSettings.Instance.BlockLayerMask, ref this.colliders);

            if (blockCollisionCount > 0)
            {
                // Game over
                Block b = currentSelectedBlock;
                DeselectCurrentBlock();
                OnBlockBroken?.Invoke(b);
                return;
            }

            int ringCollisionCount  = currentSelectedBlock.IsBlockColliding(GameSettings.Instance.RingLayerMask, ref this.colliders);

            for(int i = 0; i < ringCollisionCount; i++)
            {
                if(colliders[i].TryGetComponent<Wall>(out Wall wall))
                {
                    if (wall.IsSafe)
                    {
                        Block b = currentSelectedBlock;
                        DeselectCurrentBlock();

                        this.OnBlockCleared?.Invoke(b);
                    }
                    else
                    {
                        // Game over
                        Block b = currentSelectedBlock;
                        DeselectCurrentBlock();
                        OnBlockBroken?.Invoke(b);

                        return;
                    }
                }
            }
        }
    }

    #region Selection

    public bool TrySelectBlock(Block targetBlock)
    {
        if (currentSelectedBlock != null)
        {
            currentSelectedBlock.HandleSelectionStateChanged(isSelected: false);
            currentSelectedBlock = null;
        }
        
        if(targetBlock.IsLocked)
        {
            //TODO :: Handle sound here
            return false;
        }

        targetBlock.HandleSelectionStateChanged(isSelected: true);
        currentSelectedBlock = targetBlock;

        return true;
    }

    public void DeselectCurrentBlock()
    {
        if (currentSelectedBlock != null)
        {
            currentSelectedBlock.HandleSelectionStateChanged(isSelected: false);
            currentSelectedBlock = null;
        }
    }

    #endregion
}
