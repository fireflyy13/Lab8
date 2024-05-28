using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8TA
{
    internal class BalancedTree
    {
        public BalancedNode root;

        public BalancedTree(List<int> values)
        {
            foreach (int value in values)
            {
                Add(value);
            }
        }

        public BalancedNode BuildTreeHelper(List<int> SortedValues, int start, int end)
        {
            if (start > end)
            {
                return null;
            }

            int middle = (start + end) / 2;
            BalancedNode node = new BalancedNode(SortedValues[middle]);
            node.left = BuildTreeHelper(SortedValues, start, middle - 1);
            node.right = BuildTreeHelper(SortedValues, middle + 1, end);
            return node;
        }

        public BalancedNode BuildTree(List<int> SortedValues)
        {
            root = BuildTreeHelper(SortedValues, 0, SortedValues.Count - 1);
            return root;
        }

        public int GetHeight(BalancedNode node)
        {
            if (node == null)
            {
                return 0;
            }

            return Math.Max(GetHeight(node.left), GetHeight(node.right)) + 1;
        }
        public void AdjustHeight(BalancedNode node)
        {
            if (node == null)
            {
                return;
            }

            int leftHeight = GetHeight(node.left);
            int rightHeight = GetHeight(node.right);

            node.height = Math.Max(leftHeight, rightHeight) + 1;
        }


        public int BalanceFactor(BalancedNode node)
        {
            if (node == null)
            {
                return 0;
            }
            return GetHeight(node.right) - GetHeight(node.left);
        }

        public BalancedNode RightRotation(BalancedNode node)
        {
            BalancedNode temp = node.left;
            node.left = temp.right;
            temp.right = node;
            return temp;

        }

        public BalancedNode LeftRotation(BalancedNode node)
        {
            BalancedNode temp = node.right;
            node.right = temp.left;
            temp.left = node;
            return temp;
        }

        public BalancedNode Balance(BalancedNode node)
        {
            int balanceFactor = BalanceFactor(node);

            if (balanceFactor > 1)
            {
                if (BalanceFactor(node.right) < 0)
                {
                    node.right = RightRotation(node.right);
                }
                return LeftRotation(node);
            }
            else if (balanceFactor < -1)
            {
                if (BalanceFactor(node.left) > 0)
                {
                    node.left = LeftRotation(node.left);
                }
                return RightRotation(node);
            }

            return node;
        }

        public void Add(int value)
        {
            root = AddRecursively(root, value);
        }

        public BalancedNode AddRecursively(BalancedNode node, int value)
        {
            if (node == null)
            {
                return new BalancedNode(value);
            }

            if (value < node.Key)
            {
                node.left = AddRecursively(node.left, value);
            }
            else if (value > node.Key)
            {
                node.right = AddRecursively(node.right, value);
            }

            return Balance(node);
        }

        public BalancedNode RemoveRecursively(BalancedNode node, int value)
        {
            if (node == null)
            {
                return null;
            }

            if (value < node.Key)
            {
                node.left = RemoveRecursively(node.left, value);
            }
            else if (value > node.Key)
            {
                node.right = RemoveRecursively(node.right, value);
            }
            else
            {
                if (node.left == null)
                {
                    return node.right;
                }
                else if (node.right == null)
                {
                    return node.left;
                }

                BalancedNode successor = FindNext(node.right);
                node.Key = successor.Key;
                node.right = RemoveRecursively(node.right, successor.Key);
            }

            return Balance(node);
        }


        private BalancedNode FindNext(BalancedNode root)
        {
            while (root.left != null)
            {
                root = root.left;
            }
            return root;
        }

        public void Remove(int value)
        {
            root = RemoveRecursively(root, value);
        }


        public bool SearchRecursively(BalancedNode root, int value)
        {
            if (root == null)
            {
                return false;
            }

            else if (root.Key == value)
            {
                return true;
            }

            else if (value < root.Key)
            {
                return SearchRecursively(root.left, value);
            }

            return SearchRecursively(root.right, value);
        }

        public bool Search(int value)
        {
            return SearchRecursively(root, value);
        }


        public List<int> FindPathRecursively(BalancedNode root, int value, List<int> path)
        {
            if (root == null)
            {
                return path;
            }

            path.Add(root.Key);

            if (root.Key > value)
            {
                FindPathRecursively(root.left, value, path);
            }
            else if (root.Key < value)
            {
                FindPathRecursively(root.right, value, path);
            }

            return path;
        }

        public List<int> FindPath(int value)
        {
            List<int> path = new List<int>();

            if (!Search(value))
            {
                Add(value);
            }

            return FindPathRecursively(root, value, path);
        }

    }

    class BalancedNode
    {
        public int Key;
        public BalancedNode left, right;
        public int height;

        public BalancedNode(int item)
        {
            Key = item;
            left = right = null;
            height = 1;
        }
    }
}
