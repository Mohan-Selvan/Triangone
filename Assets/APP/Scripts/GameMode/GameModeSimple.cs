using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameModeSimple : GameModeBase
{
    public override string GameMode_PrettyName => "Game mode simple";

    [Header("References")]
    [SerializeField] LevelGenerator levelGenerator = null;
    [SerializeField] RingHandler ringHandler = null;
    [SerializeField] SelectionManager selectionManager = null;

    [Header("Visual effects")]
    [SerializeField] FXHandler fxHandler = null;

    [Header("Level settings")]
    [SerializeField] bool loadFromFile = false;

    [Header("Level settings")]
    [SerializeField] PointFieldGenerator pointFieldGenerator = null;

    [Space(10)]
    [SerializeField] string filePath = string.Empty;

    [Header("Settings")]
    [SerializeField] Vector2 knockForceRange = Vector2.up;

    private List<Block> blocks = null;
    private Dictionary<int, Block> blocksMap = null;

    internal override IEnumerator InitializeLevel()
    {
        base.InitializeLevel();

        Debug.Log($"Initializing game mode : {GameMode_PrettyName}");

        ringHandler.Initialize();
        levelGenerator.Initialize();
        fxHandler.Initialize();

        //Initializing Selection manager
        selectionManager.Initialize(HandleBlockCleared, HandleBlockBroken);
        selectionManager.EnableSelection(false);

        yield return null;
    }

    internal override IEnumerator StartLevel()
    {
        base.StartLevel();

        Debug.Log($"Starting game mode : {GameMode_PrettyName}");


        if (blocks != null && blocks.Count > 0)
        {
            Debug.LogError("Uncleared blocks found, Deleting all blocks");
            foreach (Block b in blocks)
            {
                Destroy(b);
            }

            blocks.Clear();
        }

        if (loadFromFile)
        {
            LevelData levelData = LevelDataLoader.LoadLevelFromFile(filePath);
            blocks = levelGenerator.GenerateLevel(levelData);
        }
        else
        {
            //Generating points from point field
            List<Point> points = pointFieldGenerator.GetPoints();

            //Generate a new level
            blocks = levelGenerator.GenerateLevel(points);
        }


        blocksMap = new Dictionary<int, Block>();
        blocksMap = blocks.ToDictionary((x) =>
        {
            return x.BlockID;
        });

        yield return new WaitForSeconds(1f);

        //Updating selection states for all blocks
        foreach (var kvp in blocksMap)
        {
            kvp.Value.HandleSelectionStateChanged(isSelected: false);
        }

        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].EnableBlock(value: true, animate: true, delay: Random.Range(0.1f, 1f));
        }

        ringHandler.RandomizeWallSafeStates(ringHandler.WallCount / 2);

        selectionManager.EnableSelection(true);
    }

    private void HandleBlockCleared(Block block)
    {
        block.HandleBlockCleared();

        fxHandler.HandleBlockCleared(block);

        blocksMap.Remove(block.BlockID);

        if(blocksMap.Count > 3)
        {
            LockRandomBlocks();
        }
        else
        {
            UnlockAllBlocks();
        }

        if (blocksMap.Count == 0)
        {
            //GameWorld.Instance.GameManager.HandleLevelComplete();
            return;
        }

        ringHandler.RandomizeWallSafeStates(numberOfUnsafeWalls: (ringHandler.WallCount / 2));
    }

    private void HandleBlockBroken(Block block)
    {
        fxHandler.HandleBlockBroken(block);

        //Game over
        Debug.LogError("Game over!!");

        selectionManager.EnableSelection(false);

        foreach (Block b in blocksMap.Values)
        {
            if (b == block) { continue; }

            //Calculating knock range
            float maxDistance = 10f;

            Vector2 direction = (b.transform.position - block.transform.position).normalized;
            float distance = Mathf.Clamp(Vector2.Distance(b.transform.position, block.transform.position), 0f, maxDistance);

            float t = Mathf.InverseLerp(0, maxDistance, distance);
            float knockMagnitude = Mathf.Lerp(knockForceRange.x, knockForceRange.y, 1f - t);

            Rigidbody2D rb = b.GetRigidBody();

            //Unlocking rigidbody
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.bodyType = RigidbodyType2D.Dynamic;

            rb.AddForce(direction * knockMagnitude);
        }
    }


    private void LockRandomBlocks()
    {
        selectionManager.DeselectCurrentBlock();

        List<int> blockIDs = blocksMap.Keys.ToList();
        Helpers.ShuffleList<int>(ref blockIDs);

        int totalCount = blocksMap.Count;

        float maxPercent = 0.2f;
        int lockCount = Mathf.FloorToInt(blocksMap.Count * maxPercent);

        for (int i = 0; i < totalCount; i++)
        {
            bool shouldLock = (i + 1) <= lockCount;
            blocksMap[blockIDs[i]].LockBlock(value: shouldLock, animate: true);
        }
    }


    private void UnlockAllBlocks()
    {
        foreach (var kvp in blocksMap)
        {
            kvp.Value.LockBlock(value: false, animate: true);
        }
    }
}
