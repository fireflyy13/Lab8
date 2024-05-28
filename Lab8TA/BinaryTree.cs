using System.Collections.Generic;

namespace Lab8TA
{
    class BinaryTree
    {
        public TreeNode root;
        public BinaryTree()
        {
            root = null;
        }

        public void Add(int value)
        {
            if (root == null)
            {
                root = new TreeNode(value);
            }
            else
            {
                AddRecursively(root, value);
            }
        }

        public void AddRecursively(TreeNode root, int value)
        {
            if (root == null)
            {
                new TreeNode(value);
            }

            else if (value < root.Key)
            {
                if (root.left == null)
                {
                    root.left = new TreeNode(value);
                }

                else
                {
                    AddRecursively(root.left, value);
                }
            }

            else if (value > root.Key)
            {
                if (root.right == null)
                {
                    root.right = new TreeNode(value);
                }

                else
                {
                    AddRecursively(root.right, value);
                }
            }

        }

        public TreeNode RemoveRecursively(TreeNode root, int value)
        {
            if (root == null)
            {
                return null;
            }

            if (value < root.Key)
            {
                root.left = RemoveRecursively(root.left, value);
            }

            else if (value > root.Key)
            {
                root.right = RemoveRecursively(root.right, value);
            }

            else
            {
                if (root.left == null)
                {
                    return root.right;
                }
                else if (root.right == null)
                {
                    return root.left;
                }

                TreeNode nextGreatest = FindNext(root.right);
                root.Key = nextGreatest.Key;
                root.right = RemoveRecursively(root.right, nextGreatest.Key);
            }

            return root;
        }

        private TreeNode FindNext(TreeNode root)
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


        public bool SearchRecursively(TreeNode root, int value)
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

        public List<int> FindPathRecursively(TreeNode root, int value, List<int> path)
        {
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

        public List<int> FindPath(int value, TreeNode root)
        {
            List<int> path = new List<int>();
            if (!SearchRecursively(root, value))
            {
                Add(value);
            }

            return FindPathRecursively(root, value, path);
        }


    }

    class TreeNode
    {
        public int Key;
        public TreeNode left, right;

        public TreeNode(int item)
        {
            Key = item;
            left = right = null;
        }

        public TreeNode()
        {

        }
    }
}
