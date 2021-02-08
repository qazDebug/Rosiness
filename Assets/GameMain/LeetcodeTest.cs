/****************************************************
	文件：LeetcodeTest.cs
	作者：世界和平
	日期：2020/12/3 18:20:45
	功能：Nothing
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeetcodeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TreeNode treeNode = new TreeNode(-10)
        {
            left = new TreeNode(9),
            right = new TreeNode(20) {
                left = new TreeNode(15),
                right = new TreeNode(7)
            }
        };
        MaxPathSum(treeNode);
        Debug.Log(ans);

        int[] preorder = { 3, 9, 20, 15, 7 };
        int[] inorder = { 9, 3, 15, 20, 7 };
        TreeNode treeNode1 = BuildTree(preorder, inorder);
   }

    /* 基本的 二 叉树节点 */
    class TreeNode
    {
        public int val;
        public TreeNode left, right;
        public TreeNode(int val)
        {
            this.val = val;
        }
    }

    void traverse(TreeNode root)
    {
        traverse(root.left);
        traverse(root.right);
    }

    /** 给定一个非空二叉树，返回其最大路径和。
        本题中，路径被定义为一条从树中任意节点出发，沿父节点-子节点连接，达到任意节点的序列。该路径至少包含一个节点，且不一定经过根节点
    示例 1：

        输入：[1,2,3]

               1
              / \
             2   3

        输出：6
   示例 2：

        输入：[-10,9,20,null,null,15,7]

           -10
           / \
          9  20
            /  \
           15   7

        输出：42
    */
    int ans = int.MinValue;
    int MaxPathSum(TreeNode root)
    {
        if (root == null) return 0;
        int left = Mathf.Max(0, MaxPathSum(root.left));
        int right = Mathf.Max(0, MaxPathSum(root.right));

        ans = Mathf.Max(ans, left + right + root.val);
        return Mathf.Max(left, right) + root.val;
    }

    /**
     * 根据一棵树的前序遍历与中序遍历构造二叉树。     
        注意:
        你可以假设树中没有重复的元素。
        例如，给出

        前序遍历 preorder = [3,9,20,15,7]
        中序遍历 inorder = [9,3,15,20,7]
        返回如下的二叉树：
            3
           / \
          9  20
            /  \
           15   7
     */
    int pre = 0;
    int inNum = 0;
    TreeNode BuildTree(int[] preorder, int[] inorder)
    {
        return BuildTree(preorder,inorder,int.MaxValue);
    }

    TreeNode BuildTree(int[] preorder, int[] inorder, int stop)
    {
        if (pre >= preorder.Length) return null;
        if(inorder[inNum] == stop)
        {
            inNum++;
            return null;
        }

        int curVal = preorder[pre++];
        TreeNode root = new TreeNode(curVal);
        root.left = BuildTree(preorder, inorder, curVal);
        root.right = BuildTree(preorder, inorder, stop);
        return root;

    }
}
