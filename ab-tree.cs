using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment2bro
{
    //Simple operation class that contains the informarion of a line
    class Operation
    {
        public string operation;
        public int number;

        //Constructor initializes the variables
        public Operation(string operation, int number)
        {
            this.operation = operation;
            this.number = number;
        }
    }

    //Read the file and create all the tree stacks
    class TreeManager
    {
        TreeStack currentStack;
        int counter = 0;
        string line;
        bool flag = true;
        String path;

        //Constructor 
        public TreeManager(String path)
        {
            this.path = path;
        }

        //Takes values until "#", then redirects the data for the tree operations and keep stats
        public void run()
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string firstChar;
            string[] splitLine = new string[2];

            //Read line by line
            while ((line = file.ReadLine()) != null)
            {
                splitLine = line.Split(' ');
                firstChar = splitLine[0];

                //According to the first char of every line, perform an action
                switch (firstChar)
                {
                    case "#":

                        //if it is the first line just create a new stack
                        if (flag)
                        {
                            currentStack = new TreeStack(int.Parse(splitLine[1]));
                            flag = false;
                        }
                        else
                        {
                            //TODO run tree builder  
                            currentStack = new TreeStack(int.Parse(splitLine[1]));
                        }

                        break;

                    case "I":
                        currentStack.Push(new Operation("I", int.Parse(splitLine[1])));
                        break;

                    case "D":
                        currentStack.Push(new Operation("D", int.Parse(splitLine[1])));
                        break;
                }

                counter++;
            }

            TwoThreeTree tree = new TwoThreeTree();

            foreach (var o in currentStack.operations)
            {
                if (o.operation == "I")
                    tree.insert(o.number);
                else if (o.operation == "D")
                    tree.delete(o.number);
                /*
                Console.Clear();
                tree.root.PrintNode(1);*/

            }

            //tree.delete(6326);
            //tree.delete(4742);
            tree.root.PrintNode(1);
            file.Close();
            Console.ReadLine();
        }
    }

    //Stack class for the tree operation that is going to be performed
    class TreeStack
    {
        public List<Operation> operations;
        int pTop;
        int size;

        //Constructor
        public TreeStack(int size)
        {
            this.operations = new List<Operation>();
            this.size = size;
            pTop = -1;
        }

        //Adds a operation to the stack
        public void Push(Operation operation)
        {
            operations.Add(operation);
            pTop++;
        }

        //Retrieves the last inserted operation
        public Operation Pop()
        {
            pTop--;
            return (Operation)operations[pTop + 1];
        }

        //Checks whether the stack is empty or not 
        public bool isEmpty()
        {
            bool b = pTop == -1 ? true : false;
            return b;
        }

    }

    //Represents a node of 2,3 Tree
    public class TwoThreeTreeNode
    {
        public bool isLeaf;
        public int[] elements;                 //elements[0] : leftmost element , elements[1] : middle element , elements[2] : rightmost element
        public TwoThreeTreeNode[] children;    //children[0] : leftmost child , children[1] : middle child , children[2] : rightmost child
        public TwoThreeTreeNode parent;        //parent node
        public int numberOfElements;           //this variable indicates the number of the elements that stored in the node 
        //in case this node isn't a leaf, numberOfElement + 1 gives the number of children

        public TwoThreeTreeNode()
        {
            this.elements = new int[2];
            this.children = new TwoThreeTreeNode[4];
            this.numberOfElements = 0;
            this.isLeaf = false;
            this.parent = null;
        }

        public void PrintNode(int indent)
        {
            const int width = 3;
            int i = 0;

            // print indent spaces
            for (i = 0; i < indent; i++)
                Console.Write(" ");
            Console.Write("<-");
            // print the data from this node
            for (i = 0; i < this.elements.Length; i++)
            {
                if (true)
                    Console.Write(this.elements[i] + "-");

            }

            Console.Write(">");
            // print endl at end of the root data
            Console.WriteLine();

            // recursively print children
            for (i = 0; i < this.children.Length; i++)
            {
                if (this.children[i] != null)
                    this.children[i].PrintNode(indent + 14);
            }

        }
    }

    //Represents a 2,3 Tree
    public class TwoThreeTree
    {
        public TwoThreeTreeNode root;
        int n;                          //number of elements that's been being processed by the tree

        //Constructor that initializes the root of the tree
        public TwoThreeTree()
        {
            this.root = new TwoThreeTreeNode();
            this.n = 0;
            this.root.isLeaf = true;
            this.root.numberOfElements = 0;
            this.root.parent = null;
        }

        //Insert an element to the tree
        public void insert(int element)
        {
            TwoThreeTreeNode twoTreeNode = this.FindSubtreeLeaf(this.root, element);

            //If it is root just add the element to the root
            if (twoTreeNode.numberOfElements == 0 && twoTreeNode.parent == null)
            {
                twoTreeNode.elements[0] = element;
                twoTreeNode.numberOfElements++;
            }
            else if (twoTreeNode.numberOfElements == 1)
            {
                if (twoTreeNode.elements[0] < element)
                {
                    twoTreeNode.elements[1] = element;
                }
                else
                {
                    twoTreeNode.elements[1] = twoTreeNode.elements[0];
                    twoTreeNode.elements[0] = element;
                }
                twoTreeNode.numberOfElements++;
            }
            else if (twoTreeNode.numberOfElements == 2)
            {
                this.split(twoTreeNode, element);
            }
        }

        //Recursively split to balance the tree
        public void split(TwoThreeTreeNode twoThreeNode, int element)
        {

            TwoThreeTreeNode p;

            if (twoThreeNode.parent == null)
            {
                p = new TwoThreeTreeNode();
                p.numberOfElements = 0;
            }
            else
            {
                p = twoThreeNode.parent;
            }

            TwoThreeTreeNode n1 = new TwoThreeTreeNode();
            TwoThreeTreeNode n2 = new TwoThreeTreeNode();

            //finding the smallest, middle and large elements
            int small, middle, large;

            if (element < twoThreeNode.elements[0])
            {
                small = element;
                middle = twoThreeNode.elements[0];
                large = twoThreeNode.elements[1];
            }
            else if (element > twoThreeNode.elements[1])
            {
                small = twoThreeNode.elements[0];
                middle = twoThreeNode.elements[1];
                large = element;
            }
            else
            {
                small = twoThreeNode.elements[0];
                middle = element;
                large = twoThreeNode.elements[1];
            }

            //set smallest and largest keys to the n1 and n2 respectively
            n1.elements[0] = small;
            n2.elements[0] = large;

            //Make p the parent node of n1 and n2
            n1.parent = p;
            n2.parent = p;


            if (p.numberOfElements == 0)
            {
                p.children[0] = n1;
                p.children[1] = n2;
                this.root = p;
                n1.numberOfElements++;
                n2.numberOfElements++;
                n1.isLeaf = true;
                n2.isLeaf = true;
            }
            else if (p.numberOfElements == 1)
            {
                if (n2.elements[0] < p.elements[0])
                {
                    p.children[2] = p.children[1];
                    p.children[0] = n1;
                    p.children[1] = n2;
                }
                else
                {
                    p.children[1] = n1;
                    p.children[2] = n2;
                }
                n1.isLeaf = true;
                n2.isLeaf = true;
                n1.numberOfElements++;
                n2.numberOfElements++;
            }
            else
            {
                if (n2.elements[0] < p.elements[0] && n2.elements[0] < p.elements[1])
                {
                    p.children[3] = p.children[2];
                    p.children[2] = p.children[1];
                    p.children[0] = n1;
                    p.children[1] = n2;
                }
                else if (n1.elements[0] > p.elements[0] && n1.elements[0] > p.elements[1])
                {
                    p.children[2] = n1;
                    p.children[3] = n2;
                }

                else
                {
                    p.children[3] = p.children[2];
                    p.children[1] = n1;
                    p.children[2] = n2;
                }
            }


            //if it is not a leaf check
            if (twoThreeNode.isLeaf == false)
            {
                twoThreeNode.children[0].parent = n1;
                twoThreeNode.children[1].parent = n1;
                twoThreeNode.children[2].parent = n2;
                twoThreeNode.children[3].parent = n2;
                n1.children[0] = twoThreeNode.children[0];
                n1.children[1] = twoThreeNode.children[1];
                n2.children[0] = twoThreeNode.children[2];
                n2.children[1] = twoThreeNode.children[3];
                n1.isLeaf = false;
                n2.isLeaf = false;

            }

            if (p.numberOfElements == 2)
            {
                this.split(p, middle);
                if (n1.children[0] != null || n2.children[0] != null)
                {
                    if (n1.children[0].isLeaf || n2.children[0].isLeaf)
                    {
                        n1.isLeaf = false;
                        n2.isLeaf = false;
                    }

                }
                else
                {
                    n1.isLeaf = true;
                    n2.isLeaf = true;
                }



                n1.parent.isLeaf = false;
                n2.parent.isLeaf = false;
                n1.numberOfElements++;
                n2.numberOfElements++;
            }

            else if (p.numberOfElements == 1)
            {
                if (p.elements[0] < middle)
                {
                    p.elements[1] = middle;
                }
                else
                {
                    p.elements[1] = p.elements[0];
                    p.elements[0] = middle;
                }

                p.numberOfElements++;
            }

            else
            {
                p.elements[0] = middle;
                p.numberOfElements++;
            }


        }

        public void delete(int element)
        {
            if (element == 3055)
            {
                this.root.PrintNode(1);
            }
            //variables;
            int inOrderInd;
            TwoThreeTreeNode leafNode;
            TwoThreeTreeNode inOrderSuccessor;
            int tempElement;

            //return the node that contains the element
            TwoThreeTreeNode node = this.FindNode(this.root, element);
            
            //Check the node
            if (node != null)
            {
                //index of the element in that node
                int ind = node.elements[0] == element ? 0 : 1;
                //if node isn't in the leaves
                if (node.isLeaf == false)
                {
                    //find the inorder succsesor of the element
                    inOrderSuccessor = this.InOrderSuccessor(element, node);

                    //normally inorder successor must be at the first index but just to be sure checking the values
                    if (inOrderSuccessor.numberOfElements == 2)
                    {
                        inOrderInd = 0;
                    }
                    else
                    {
                        inOrderInd = inOrderSuccessor.elements[0] != 0 ? 0 : 1;
                    }

                    //swap the item with the inorder successor
                    tempElement = node.elements[ind];
                    node.elements[ind] = inOrderSuccessor.elements[inOrderInd];
                    inOrderSuccessor.elements[inOrderInd] = tempElement;

                    leafNode = inOrderSuccessor;
                }

                //if node in the leaf
                else
                {
                    leafNode = node;
                }

                //delete the element
                if (element == leafNode.elements[0])
                {
                    //move the second element to the first then assign the second to 0
                    leafNode.elements[0] = leafNode.elements[1];
                    leafNode.elements[1] = 0;

                    //decrease the number of elements
                    leafNode.numberOfElements--;
                }
                else if (element == leafNode.elements[1])
                {
                    //if it is second element just delete it, don't move anything
                    leafNode.elements[1] = 0;
                    leafNode.numberOfElements--;
                }
                else
                {
                    //Due to previous step, first or second element must be match, but just to be sure
                    Console.WriteLine("There must be a mistake, element couldn't be found in the node!");
                }

                //after deleting the element, if the node is empty, fix the tree
                if (leafNode.numberOfElements == 0)
                {
                    Fix(leafNode);
                }
            }
            else
            {
                Console.WriteLine(element + " Couldn't found in the tree, thus process terminated");
            }
        }

        void Redistrubute(TwoThreeTreeNode node, TwoThreeTreeNode p, int situation)
        {

            //there is two different situation if the parent has two nodes
            if (p.numberOfElements == 1)
            {
                if (situation == 0)
                {
                    node.elements[0] = p.elements[0];
                    p.elements[0] = p.children[0].elements[1];
                    p.children[0].elements[1] = 0;
                    node.numberOfElements++;
                    p.children[0].numberOfElements--;
                   
                    if (node.isLeaf == false)
                    {
                        node.children[1] = node.children[0];
                        node.children[0] = p.children[0].children[2];
                        p.children[0].children[2] = null;
                        node.children[0].parent = node;
                    }
                }
                else
                {
                    
                    node.elements[0] = p.elements[0];
                    p.elements[0] = p.children[1].elements[0];
                    p.children[1].elements[0] = p.children[1].elements[1];
                    p.children[1].elements[1] = 0;
                    node.numberOfElements++;
                    p.children[1].numberOfElements--;

                    
                    if (node.isLeaf == false)
                    {
                        node.children[1] = p.children[1].children[0];
                        p.children[1].children[0] = p.children[1].children[1];
                        p.children[1].children[1] = p.children[1].children[2];
                        p.children[1].children[2] = null;
                        node.children[1].parent = node;
                    }
                }
            }

             //there is six different situation if the parent has three nodes
            else if (p.numberOfElements == 2)
            {
                //Check for which children is empty
                if (p.children[0].numberOfElements == 0)
                {
                    //situation can be 1 or 2
                    if (situation == 1)
                    {
                        node.elements[0] = p.elements[0];
                        p.elements[0] = p.children[1].elements[0];
                        p.children[1].elements[0] = p.children[1].elements[1];
                        p.children[1].elements[1] = 0;
                        p.children[1].numberOfElements--;
                        node.numberOfElements++;

                        if (node.isLeaf == false)
                        {
                            node.children[1] = p.children[1].children[0];
                            p.children[1].children[0] = p.children[1].children[1];
                            p.children[1].children[1] = p.children[1].children[2];
                            p.children[1].children[2] = null;

                            node.children[1].parent = node;
                        }
                    }
                    else
                    {
                        node.elements[0] = p.elements[0];
                        p.elements[0] = p.children[1].elements[0];
                        p.children[1].elements[0] = p.elements[1];
                        p.elements[1] = p.children[2].elements[0];
                        p.children[2].elements[0] = p.children[2].elements[1];
                        p.children[2].elements[1] = 0;
                        p.children[2].numberOfElements--;
                        node.numberOfElements++;

                        if (node.isLeaf == false)
                        {
                            node.children[1] = p.children[1].children[0];
                            p.children[1].children[0] = p.children[1].children[1];
                            p.children[1].children[1] = p.children[2].children[0];
                            p.children[2].children[0] = p.children[2].children[1];
                            p.children[2].children[1] = p.children[2].children[2];
                            p.children[2].children[2] = null;

                            node.children[1].parent = node;
                            p.children[1].children[1].parent = p.children[1];
                        }
                    }
                }
                else if (p.children[1].numberOfElements == 0)
                {
                    //situation can be 0 or 2
                    if (situation == 0)
                    {
                        node.elements[0] = p.elements[0];
                        p.elements[0] = p.children[0].elements[1];
                        p.children[0].elements[1] = 0;
                        p.children[0].numberOfElements--;
                        node.numberOfElements++;

                        if (node.isLeaf == false)
                        {
                            node.children[1] = node.children[0];
                            node.children[0] = p.children[0].children[2];
                            p.children[0].children[2] = null;

                            node.children[0].parent = node;
                        }
                    }
                    else
                    {
                        node.elements[0] = p.elements[1];
                        p.elements[1] = p.children[2].elements[0];
                        p.children[2].elements[0] = p.children[2].elements[1];
                        p.children[2].elements[1] = 0;
                        p.children[2].numberOfElements--;
                        node.numberOfElements++;

                        if (node.isLeaf == false)
                        {
                            node.children[1] = p.children[2].children[0];
                            p.children[2].children[0] = p.children[2].children[1];
                            p.children[2].children[1] = p.children[2].children[2];
                            p.children[2].children[2] = null;

                            node.children[1].parent = node;
                        }
                    }
                }
                else if (p.children[2].numberOfElements == 0)
                {
                    //situation can be 0 or 1
                    if (situation == 0)
                    {
                        node.elements[0] = p.elements[1];
                        p.elements[1] = p.children[1].elements[0];
                        p.children[1].elements[0] = p.elements[0];
                        p.elements[0] = p.children[0].elements[1];
                        p.children[0].elements[1] = 0;
                        p.children[0].numberOfElements--;
                        node.numberOfElements++;

                        if (node.isLeaf == false)
                        {
                            node.children[1] = node.children[0];
                            node.children[0] = p.children[1].children[1];
                            p.children[1].children[1] = p.children[1].children[0];
                            p.children[1].children[0] = p.children[0].children[2];
                            p.children[0].children[2] = null;

                            node.children[0].parent = node;
                            p.children[1].children[0].parent = p.children[1];
                        }
                    }
                    else
                    {
                        node.elements[0] = p.elements[1];
                        p.elements[1] = p.children[1].elements[1];
                        p.children[1].elements[1] = 0;
                        p.children[1].numberOfElements--;
                        node.numberOfElements++;

                        if (node.isLeaf == false)
                        {
                            node.children[1] = node.children[0];
                            node.children[0] = p.children[1].children[2];
                            p.children[1].children[2] = null;

                            node.children[0].parent = node;
                        }
                    }
                }
            }
        }

        void Merge(TwoThreeTreeNode node)
        {
            if (node.parent.numberOfElements == 1)
            {
                        //empty node at the left
                        if (node.parent.children[0] == node)
                        {
                            node.parent.children[1].elements[1] = node.parent.children[1].elements[0];
                            node.parent.children[1].elements[0] = node.parent.elements[0];
                            node.parent.elements[0] = 0;

                            node.parent.children[1].numberOfElements++;
                            node.parent.numberOfElements--;

                            //remove the node
                            node.parent.children[0] = node.parent.children[1];
                            node.parent.children[1] = null;

                            //if node is internal
                            if (node.isLeaf == false)
                            {
                                node.parent.children[0].children[2] = node.parent.children[0].children[1];
                                node.parent.children[0].children[1] = node.parent.children[0].children[0];
                                node.parent.children[0].children[0] = node.children[0];
                                //update the parent
                                node.parent.children[0].children[0].parent = node.parent.children[0];
                            }
                        }
                        //empty node at the right
                        else
                        {
                            node.parent.children[0].elements[1] = node.parent.elements[0];
                            node.parent.elements[0] = 0;
                            node.parent.children[0].numberOfElements++;
                            node.parent.numberOfElements--;
                            node.parent.children[1] = null;

                            //if node is internal
                            if (node.isLeaf == false)
                            {
                                node.parent.children[0].children[2] = node.children[0];
                                node.parent.children[0].children[2].parent = node.parent.children[0];
                            }
                        }
            }
                    else if (node.parent.numberOfElements == 2)
                    {
                        //empty node at the left
                        if (node.parent.children[0] == node)
                        {
                            node.parent.children[1].elements[1] = node.parent.children[1].elements[0];
                            node.parent.children[1].elements[0] = node.parent.elements[0];
                            node.parent.elements[0] = node.parent.elements[1];
                            node.parent.elements[1] = 0;
                            node.parent.numberOfElements--;
                            node.parent.children[1].numberOfElements++;

                            node.parent.children[0] = node.parent.children[1];
                            node.parent.children[1] = node.parent.children[2];
                            node.parent.children[2] = null;

                            //if node is internal
                            if (node.isLeaf == false)
                            {
                                node.parent.children[0].children[2] = node.parent.children[0].children[1];
                                node.parent.children[0].children[1] = node.parent.children[0].children[0];
                                node.parent.children[0].children[0] = node.children[0];

                                node.parent.children[0].children[0].parent = node.parent.children[0];
                            }

                        }

                        //empty node at the middle
                        else if (node.parent.children[1] == node)
                        {
                            node.parent.children[0].elements[1] = node.parent.elements[0];
                            node.parent.elements[0] = node.parent.elements[1];
                            node.parent.elements[1] = 0;
                            node.parent.children[0].numberOfElements++;
                            node.parent.numberOfElements--;

                            node.parent.children[1] = node.parent.children[2];
                            node.parent.children[2] = null;

                            //if node is internal
                            if (node.isLeaf == false)
                            {
                                node.parent.children[0].children[2] = node.children[0];
                                node.parent.children[0].children[2].parent = node.parent.children[0];
                            }
                        }

                        //empty node at the right
                        else
                        {
                            node.parent.children[1].elements[1] = node.parent.elements[1];
                            node.parent.elements[1] = 0;
                            node.parent.numberOfElements--;
                            node.parent.children[1].numberOfElements++;
                            node.parent.children[2] = null;

                            //if node is internal
                            if (node.isLeaf == false)
                            {
                                node.parent.children[1].children[2] = node.children[0];
                                node.parent.children[1].children[2].parent = node.parent.children[1];
                            }
                        }
                    }
        }

        //completes the deletion when node n is empty by either
        // removing the root, redistributing values, or merging nodes.
        // if n is internal, it has only one child
        void Fix(TwoThreeTreeNode node)
        {
            TwoThreeTreeNode p;

            if (node.parent == null) //No parent means it is root node
            {
                if (node.children[0] == null) { return; }
                //remove root and set the children as a root
                this.root = node.children[0];
                this.root.parent = null;
                //this.root.children[2] = node.children[2]; 
            } 

            else
            {
                p = node.parent;

                //situation represents both the possibility of merge and the sibling with two elements
                //if it returns -1, means there is no siblings with two elements
                //if it is bigger than -1, means that index has the two elements
                int situation = this.HasChildWithTwoElement(p);

                //check for redistrubution is possible or not
                if (situation > -1)
                {
                    //redistrubute
                    this.Redistrubute(node, p, situation);
                }

                //redistrubuting not possible, thus merge
                //for merging we have 5 different situation; 2 for parent with one element, 3 for parent with two element
                else
                {
                    this.Merge(node);
                }

                if (node.parent.numberOfElements == 0)
                {
                     this.Fix(node.parent);
                }
            }
        }

        //Look for the right or the closest siblings with two elements, if not returns -1
        int HasChildWithTwoElement(TwoThreeTreeNode node)
        {
            int result = -1;

            //check the number of the children equality to 2
            if (node.numberOfElements == 1)
            {
                for (int i = 0; i < node.children.Length; i++)
                {
                    if (node.children[i] != null)
                    {
                        if (node.children[i].numberOfElements == 2)
                        {
                            result = i;
                            break;
                        }
                    }
                }
            }

            //number of the children equality to 3
            else
            {
                if (node.children[0].numberOfElements == 0)
                {
                    for (int i = 0; i < node.children.Length; i++)
                    {
                        if (node.children[i] != null)
                        {
                            if (node.children[i].numberOfElements == 2)
                            {
                                result = i;
                                break;
                            }
                        }
                    }
                }
                
                // I should take from the right first so the loop counter is decreasing
                else
                {
                    for (int i = node.children.Length - 1; i >= 0; i--)
                    {
                        if (node.children[i] != null)
                        {
                            if (node.children[i].numberOfElements == 2)
                            {
                                result = i;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public TwoThreeTreeNode InOrderSuccessor(int element, TwoThreeTreeNode node)
        {
            TwoThreeTreeNode next;

            // When this method is called, key will equal smallValue or largeValue, and we must do a comparison.
            // We check if location is a three node and, if it is, we compare key to smallValue. If equal, go down middleChild.
            if (node.children[0] != null && node.children[1] != null && node.children[2] != null)
            {
                if (node.elements[0] == element)
                {
                    next = node.children[1];
                }
                else
                {
                    next = node.children[2];
                }
            }
            else
            {
                next = node.children[1];
            }
            
            // Continue down left branches until we encounter a leaf.
            while (next.isLeaf == false)
            {
                next = next.children[0];
            }

            return next;
        }

        //Search for the subtree that contains the element, If it finds returns the node that contains e, 
        //if not it returns the node that element can be placed in
        public TwoThreeTreeNode FindSubtreeLeaf(TwoThreeTreeNode twoThreeNode, int element)
        {
            if (twoThreeNode.isLeaf)
                return twoThreeNode;

            else
            {
                if (element <= twoThreeNode.elements[0])
                    return FindSubtreeLeaf(twoThreeNode.children[0], element);

                else if (twoThreeNode.numberOfElements < 2 || element <= twoThreeNode.elements[1])
                    return FindSubtreeLeaf(twoThreeNode.children[1], element);

                else
                    return FindSubtreeLeaf(twoThreeNode.children[2], element);
            }
        }

        public TwoThreeTreeNode FindNode(TwoThreeTreeNode twoThreeNode, int element)
        {
            bool isFound = false;
            if (twoThreeNode != null)
            {
                for (int i = 0; i < twoThreeNode.numberOfElements; i++)
                {
                    if (twoThreeNode.elements[i] == element)
                        isFound = true;
                }

                if (isFound == true)
                    return twoThreeNode;

                else if (twoThreeNode.numberOfElements == 1)
                {
                    if (element < twoThreeNode.elements[0])
                        return FindNode(twoThreeNode.children[0], element);
                    else
                        return FindNode(twoThreeNode.children[1], element);
                }
                else if (twoThreeNode.numberOfElements == 2)
                {
                    if (element < twoThreeNode.elements[0])
                    {
                        return FindNode(twoThreeNode.children[0], element);
                    }
                    else if (element > twoThreeNode.elements[1])
                    {
                        return FindNode(twoThreeNode.children[2], element);
                    }
                    else
                    {
                        return FindNode(twoThreeNode.children[1], element);
                    }
                }
            }
            
            return null;
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            TreeManager treeManager = new TreeManager("d:\\data.txt");
            treeManager.run();
        }
    }
}
