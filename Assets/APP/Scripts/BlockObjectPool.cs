using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObjectPool : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Block blockPrefab = null;

    [Header("Settings")]
    [SerializeField] int initialCount = 5;

    [Header("Testing only")]
    [SerializeField] int stackCount = 0;

    //Privates
    private Stack<Block> _blockStack = null;

    internal void Initialize()
    {
        _blockStack = new Stack<Block>();

        for (int i = 0; i < initialCount; i++)
        {
            CreateAndAddBlock();
        }
    }

    internal void Deinitialize()
    {

    }

    private void CreateAndAddBlock()
    {
        Block block = Instantiate<Block>(blockPrefab, this.transform);
        _blockStack.Push(block);

        UpdateStackCount();
    }

    public Block GetBlock()
    {
        if(_blockStack.Count == 0)
        {
            CreateAndAddBlock();
        }
        
        Block block = _blockStack.Pop();
        block.gameObject.SetActive(true);

        UpdateStackCount();

        return block;
    }

    public void ReturnBlock(Block block)
    {
        block.gameObject.SetActive(false);
        _blockStack.Push(block);

        UpdateStackCount();
    }

    private void UpdateStackCount()
    {
        this.stackCount = _blockStack.Count;
    }
}
