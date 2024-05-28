using System.Collections.Generic;
using System.Drawing;

namespace Lab8TA
{
    internal class RedBlackTree
    {
        public RedBlackNode root;

        public void LeftRotation(RedBlackNode node)
        {
            if (node == null || node.right == null)
            {
                return;
            }

            RedBlackNode rightChild = node.right;
            node.right = rightChild.left;
            if (rightChild.left != null)
            {
                rightChild.left.parent = node;
            }

            rightChild.parent = node.parent;
            if (node.parent == null)
            {
                root = rightChild;
            }
            else if (node == node.parent.left)
            {
                node.parent.left = rightChild;
            }
            else
            {
                node.parent.right = rightChild;
            }

            rightChild.left = node;
            node.parent = rightChild;
        }

        public void RightRotation(RedBlackNode node)
        {
            if (node == null || node.left == null)
            {
                return;
            }

            RedBlackNode leftChild = node.left;
            node.left = leftChild.right;
            if (leftChild.right != null)
            {
                leftChild.right.parent = node;
            }

            leftChild.parent = node.parent;
            if (node.parent == null)
            {
                root = leftChild;
            }
            else if (node == node.parent.right)
            {
                node.parent.right = leftChild;
            }
            else
            {
                node.parent.left = leftChild;
            }

            leftChild.right = node;
            node.parent = leftChild;
        }

        public void Add(int value)
        {
            RedBlackNode newNode = new RedBlackNode(value);

            if (root == null)
            {
                root = newNode;
                root.color = Colors.Black;
            }
            else
            {
                AddRecursively(root, value);
            }
        }

        public void AddRecursively(RedBlackNode node, int value)
        {

                if (root == null)
                {
                    root = new RedBlackNode(value);
                    root.color = Colors.Black;
                    return;
                }

                RedBlackNode newNode = new RedBlackNode(value);
                RedBlackNode parent = null;

                while (node != null)
                {
                    parent = node;
                    if (value < node.Key)
                    {
                        if (node.left == null)
                        {
                            node.left = newNode;
                            newNode.parent = node;
                            break;
                        }
                        else
                        {
                            node = node.left;
                        }
                    }
                    else if (value > node.Key)
                    {
                        if (node.right == null)
                        {
                            node.right = newNode;
                            newNode.parent = node;
                            break;
                        }
                        else
                        {
                            node = node.right;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                newNode.color = Colors.Red;
                Helper(newNode);
            }

        

        public void Helper(RedBlackNode node)
        {
           while (node != null && node != root && node.parent != null && node.parent.color == Colors.Red)
            {
                if (node.parent == node.parent.parent.left)
                {
                    RedBlackNode uncle = node.parent.parent.right;
                    if (uncle != null && uncle.color == Colors.Red)
                    {
                        node.parent.color = Colors.Black;
                        uncle.color = Colors.Black;
                        node.parent.parent.color = Colors.Red;
                        node = node.parent.parent;
                    }
                    else
                    {
                        if (node == node.parent.right)
                        {
                            node = node.parent;
                            LeftRotation(node);
                        }
                        node.parent.color = Colors.Black;
                        node.parent.parent.color = Colors.Red;
                        RightRotation(node.parent.parent);
                    }
                }
                else
                {
                    RedBlackNode uncle = node.parent.parent.left;
                    if (uncle != null && uncle.color == Colors.Red)
                    {
                        node.parent.color = Colors.Black;
                        uncle.color = Colors.Black;
                        node.parent.parent.color = Colors.Red;
                        node = node.parent.parent;
                    }
                    else
                    {
                        if (node == node.parent.left)
                        {
                            node = node.parent;
                            RightRotation(node);
                        }
                        node.parent.color = Colors.Black;
                        node.parent.parent.color = Colors.Red;
                        LeftRotation(node.parent.parent);
                    }
                }
                
            }
            root.color = Colors.Black;
        }

        public RedBlackNode RemoveRecursively(RedBlackNode node, int value)
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
                if (node.left == null || node.right == null)
                {
                    RedBlackNode child;
                    if (node.left != null)
                    {
                        child = node.left;
                    }
                    else
                    {
                        child = node.right;
                    }

                    if (child == null)
                    {
                        child = node;
                        node = null;
                    }
                    else
                    {
                        node.Key = child.Key;
                        node.left = node.right = null;
                    }

                    if (child.color == Colors.Black)
                    {
                        FixDelete(child);
                    }
                }
                else
                {
                    RedBlackNode successor = FindNext(node.right);
                    node.Key = successor.Key;
                    node.right = RemoveRecursively(node.right, successor.Key);
                }
            }

            return node;
        }

        private void FixDelete(RedBlackNode node)
        {
            while (node != root && node.color == Colors.Black)
            {
                if (node == node.parent.left)
                {
                    RedBlackNode sibling = node.parent.right;
                    if (sibling.color == Colors.Red)
                    {
                        sibling.color = Colors.Black;
                        node.parent.color = Colors.Red;
                        LeftRotation(node.parent);
                        sibling = node.parent.right;
                    }

                    if (sibling.left.color == Colors.Black && sibling.right.color == Colors.Black)
                    {
                        sibling.color = Colors.Red;
                        node = node.parent;
                    }
                    else
                    {
                        if (sibling.right.color == Colors.Black)
                        {
                            sibling.left.color = Colors.Black;
                            sibling.color = Colors.Red;
                            RightRotation(sibling);
                            sibling = node.parent.right;
                        }
                        sibling.color = node.parent.color;
                        node.parent.color = Colors.Black;
                        sibling.right.color = Colors.Black;
                        LeftRotation(node.parent);
                        node = root;
                    }
                }
                else
                {
                    if (node == node.parent.right)
                    {
                        RedBlackNode sibling = node.parent.left;
                        if (sibling.color == Colors.Red)
                        {
                            sibling.color = Colors.Black;
                            node.parent.color = Colors.Red;
                            RightRotation(node.parent);
                            sibling = node.parent.left;
                        }

                        if (sibling.right.color == Colors.Black && sibling.left.color == Colors.Black)
                        {
                            sibling.color = Colors.Red;
                            node = node.parent;
                        }
                        else
                        {
                            if (sibling.left.color == Colors.Black)
                            {
                                sibling.right.color = Colors.Black;
                                sibling.color = Colors.Red;
                                LeftRotation(sibling);
                                sibling = node.parent.left;
                            }
                            sibling.color = node.parent.color;
                            node.parent.color = Colors.Black;
                            sibling.left.color = Colors.Black;
                            RightRotation(node.parent);
                            node = root;
                        }
                    }
                }
            }

            node.color = Colors.Black;
        }

        public void Remove(int value)
        {
            root = RemoveRecursively(root, value);
        }

        private RedBlackNode FindNext(RedBlackNode root)
        {
            while (root.left != null)
            {
                root = root.left;
            }
            return root;
        }


        public bool SearchRecursively(RedBlackNode root, int value)
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

        public List<int> FindPathRecursively(RedBlackNode root, int value, List<int> path)
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

    public class RedBlackNode
    {
        public int Key;
        public RedBlackNode left, right, parent;
        public Colors color;
        public RedBlackNode(int item)
        {
            Key = item;
            left = right = null;
            this.color = Colors.Red;
        }
    }

    public enum Colors
    {
        Red,
        Black
    }
}
