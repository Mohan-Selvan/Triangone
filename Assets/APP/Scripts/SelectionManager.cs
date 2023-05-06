using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class SelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RingHandler ringHandler = null;

    [Header("Settings")]
    [SerializeField] Vector2 knockForceRange = Vector2.up;

    [Header("Debug only")]
    [SerializeField] bool enableUpdate = true;

    [Header("Testing only")]
    [SerializeField] Block currentSelectedBlock = null;
    [SerializeField] Vector3 offset = default;

    private Camera mainCamera = null;

    //Trackers
    Collider2D[] colliders = default;

    //Collections
    Dictionary<int, Block> blocksMap = null;

    //Helpers
    GameManager gameManager => GameWorld.Instance.GameManager;

    private void Start()
    {
        mainCamera = Camera.main;

        colliders = new Collider2D[5];
        blocksMap = new Dictionary<int, Block>();

        gameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void OnDestroy()
    {
        gameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.RUNNING)
        {
            enableUpdate = true;
        }

        if(newState == GameState.PAUSED || newState == GameState.ENDING)
        {
            DeselectCurrentBlock();
        }
    }

    private void Update()
    {
        if((!enableUpdate) || (gameManager.CurrentGameState != GameState.RUNNING)) { return; }

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
        else if(Input.GetMouseButton(0))
        {
            if(currentSelectedBlock != null)
            {
                Vector3 worldMousePosition = Helpers.GetWorldMousePosition(Input.mousePosition, mainCamera);

                currentSelectedBlock.transform.position = worldMousePosition - offset;
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
                HandleGameOver(b);

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
                        HandleBlockTouchedRing(b);
                    }
                    else
                    {
                        // Game over
                        Block b = currentSelectedBlock;
                        DeselectCurrentBlock();
                        HandleGameOver(b);

                        return;
                    }
                }
            }
        }
    }

    public void Initialize(List<Block> blocks)
    {
        blocksMap = blocks.ToDictionary((x) =>
        {
            return x.BlockID;
        });
    }

    private void HandleBlockTouchedRing(Block block)
    {
        block.HandleBlockTouchedRing();

        blocksMap.Remove(block.BlockID);

        LockRandomBlocks();

        if (blocksMap.Count == 0)
        {
            GameWorld.Instance.GameManager.HandleLevelComplete();
            return;
        }

        ringHandler.RandomizeWallSafeStates(numberOfUnsafeWalls: (ringHandler.WallCount / 2));
    }

    private void LockRandomBlocks()
    {
        DeselectCurrentBlock();

        List<int> blockIDs = blocksMap.Keys.ToList();
        Helpers.ShuffleList<int>(ref blockIDs);

        int totalCount = blocksMap.Count;

        float maxPercent = 0.2f;
        int lockCount = Mathf.FloorToInt(blocksMap.Count * maxPercent);

        for(int i = 0; i < totalCount; i++)
        {
            bool shouldLock = (i + 1) <= lockCount;
            blocksMap[blockIDs[i]].LockBlock(value: shouldLock, animate: true);
        }
    }

    private void HandleGameOver(Block currentBlock)
    {
        //Game over
        Debug.LogError("Game over!!");

        enableUpdate = false;

        foreach(Block b in blocksMap.Values)
        {
            if(b == currentBlock) { continue; }

            //Calculating knock range
            float maxDistance = 10f;

            Vector2 direction = (b.transform.position - currentBlock.transform.position).normalized;
            float distance = Mathf.Clamp(Vector2.Distance(b.transform.position, currentBlock.transform.position), 0f, maxDistance);

            float t = Mathf.InverseLerp(0, maxDistance, distance);
            float knockMagnitude = Mathf.Lerp(knockForceRange.x, knockForceRange.y, 1f - t);

            Rigidbody2D rb = b.GetRigidBody();

            //Unlocking rigidbody
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.bodyType = RigidbodyType2D.Dynamic;

            rb.AddForce(direction * knockMagnitude);
        }

        //Ending game
        gameManager.EndGame();
    }

    #region Selection

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

    public void DeselectAllBlocks()
    {
        foreach(var kvp in blocksMap)
        {
            kvp.Value.HandleSelectionStateChanged(isSelected: false);
        }
    }

    #endregion
}
