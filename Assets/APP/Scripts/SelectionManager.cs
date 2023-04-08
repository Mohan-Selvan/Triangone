using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class SelectionManager : MonoBehaviour
{

    Dictionary<int, Block> blocksMap = null;

    [Header("Testing only")]
    [SerializeField] Block currentSelectedBlock = null;
    [SerializeField] Vector3 offset = default;

    private Camera mainCamera = null;

    private void Start()
    {
        mainCamera = Camera.main;

        blocksMap = new Dictionary<int, Block>();
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
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

                    if(TrySelectBlock(block.BlockID))
                    {
                        offset = GetWorldMousePosition(Input.mousePosition) - currentSelectedBlock.transform.position;
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
        else if(Input.GetMouseButton(0))
        {
            if(currentSelectedBlock != null)
            {
                Vector3 worldMousePosition = GetWorldMousePosition(Input.mousePosition);

                currentSelectedBlock.transform.position = worldMousePosition - offset;
            }
        }

        if(currentSelectedBlock != null)
        {
            bool isBlockColliding = currentSelectedBlock.IsBlockColliding(GameSettings.Instance.BlockLayerMask);

            if (isBlockColliding)
            {
                Debug.LogError("Game over!!");
            }

            bool isRingColliding = currentSelectedBlock.IsBlockColliding(GameSettings.Instance.RingLayerMask);

            if (isRingColliding)
            {
                Block b = currentSelectedBlock;
                DeselectCurrentBlock();
                b.HandleBlockTouchedRing();
                return;
            }
        }
    }

    private Vector3 GetWorldMousePosition(Vector2 inputMousePosition)
    {
        Vector3 mousePosition = inputMousePosition;
        mousePosition.z = -10f;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;

        return worldPosition;
    }

    public void UpdateBlocks(List<Block> blocks)
    {
        blocksMap = blocks.ToDictionary((x) =>
        {
            return x.BlockID;
        });
    }

    public bool TrySelectBlock(int blockID)
    {
        if (currentSelectedBlock != null)
        {
            currentSelectedBlock.HandleSelectionStateChanged(isSelected: false);
            currentSelectedBlock = null;
        }

        if (!blocksMap.ContainsKey(blockID))
        {
            Debug.LogError($"Block with ID ({blockID}) not found");
            return false;
        }

        Block targetBlock = blocksMap[blockID];
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

    public void DeselectAllBlocks()
    {
        foreach(var kvp in blocksMap)
        {
            kvp.Value.HandleSelectionStateChanged(isSelected: false);
        }
    }
}
