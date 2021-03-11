using AlmutalCore.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using AlmutalCore.Helpers;

namespace AlmutalCore
{
    public class Algorithm
    {
        public double Width { get; private set; }
        public double Length { get; private set; }
        public double Kerf { get; private set; }

        private readonly List<Box> Boxes;

        private List<Box> Free = new List<Box>();

        private Node CreateNode(int id)
        {
            if (Length > Width)
                return new Node { Length = Width, Width = Length, Id = id };
            else
                return new Node { Length = Length, Width = Width, Id = id };
        }

        public Algorithm(double width, double length, List<Box> boxes, double kerf)
        {
            Width = width;
            Length = length;
            Boxes = boxes;
            Kerf = kerf;
            Boxes = Sorter(Boxes);

        }

        private List<Box> Sorter(List<Box> boxes)
        {
            if (boxes == null)
                return null;
            var result = new List<Box>();
            var id = 0;

            foreach (var box in boxes)
            {
                //double tmp = 0;
                //tmp = box.Width;
                //if (box.Length > box.Width)
                //{
                //    result.Add(new Box
                //    {
                //        Width = box.Length,
                //        Length = tmp,
                //        Id = id,
                //        Area = box.Width * box.Length,
                //    });
                //}
                //else
                //{
                    result.Add(new Box
                    {
                        Width = box.Width,
                        Length = box.Length,
                        Id = id,
                        //Area = box.Width * box.Length
                    });
                //}


                id++;
            }
            result = result.OrderByDescending(x => Math.Max(x.Width, x.Length)).ToList();
            //result = result.OrderByDescending(x => x.Area).ToList();
            return result;
        }

        private void Display(List<Box> boxes)
        {
            foreach (var box in boxes)
            {
                var positionx = box.Position != null ? box.Position.X.ToString() : String.Empty;
                var positiony = box.Position != null ? box.Position.Y.ToString() : String.Empty;
                Console.WriteLine(" Pos_y  : " + positiony + " Pos_x : " + positionx 
                    );
            }
        }

        public List<StockSheet> Pack()
        {
            var sheets = new List<StockSheet>();
            var free = new List<Node>();
            var id = 0;
            var noOfCuttedBoxes = 0;
            do
            {
                var boxes = new List<Box>();

                var rootNode = CreateNode(id);
                //Sheets.Add(rootNode);
                id++;
                foreach (var box in Boxes)
                {
                    if (box.Used)
                        continue;
                    var node = FindNode(rootNode, box.Width, box.Length);
                    if (node != null)
                    {
                        if (node.rotated)
                        {
                            double tmp = 0;
                            tmp = box.Width;
                            box.Width = box.Length;
                            box.Length = tmp;
                        }
                        //mainNodes.Add(node);
                        box.Position = SplitNode(node, box.Width, box.Length);

                        box.ParentId = rootNode.Id;
                        box.Used = true;
                        box.Color = RandomStringColors.GenerateColor();
                        boxes.Add(box);
                        noOfCuttedBoxes += 1;
                        if (noOfCuttedBoxes == Boxes.Count)
                            break;
                    }
                    else
                    {
                    }
                }
                var sheet = new StockSheet
                {
                    Id = rootNode.Id,
                    CuttedPanels = boxes
                };
                sheets.Add(sheet);
                Display(boxes);

                if (noOfCuttedBoxes == Boxes.Count)
                    break;

                //var rightMostNode = RightMostLeaf(rootNode);
                //if(rightMostNode != null)
                //    free.Add(rightMostNode);
            } while (true);
            return sheets;
        }

        private Node FindNode(Node rootNode, double boxWidth, double boxLength)
        {

            if (rootNode.IsOccupied)
            {
                var rightArea = rootNode.RightNode.Width * rootNode.RightNode.Length;
                var bottomArea = rootNode.BottomNode.Width * rootNode.BottomNode.Length;
                if (rightArea > bottomArea)
                {
                    var nextNode = FindNode(rootNode.RightNode, boxWidth, boxLength);
                    if (nextNode == null)
                    {

                        nextNode = FindNode(rootNode.RightNode, boxLength, boxWidth);
                        if (nextNode != null) 
                        {
                            nextNode.rotated = true; 
                        }

                        if (nextNode == null)
                        {
                            nextNode = FindNode(rootNode.BottomNode, boxWidth, boxLength);

                            //if (nextNode == null)
                            //{
                            //    nextNode = FindNode(rootNode.BottomNode, boxLength, boxWidth);
                            //    if (nextNode != null) { nextNode.rotated = true; }

                            //}
                        }

                    }

                    

                    return nextNode;

                }
                else 
                {
                    var nextNode = FindNode(rootNode.BottomNode, boxWidth, boxLength);
                    if (nextNode == null)
                    {

                        nextNode = FindNode(rootNode.BottomNode, boxLength, boxWidth);
                        if (nextNode != null) { nextNode.rotated = true; }

                        if (nextNode == null)
                        {
                            nextNode = FindNode(rootNode.RightNode, boxWidth, boxLength);

                            //if (nextNode == null)
                            //{
                            //    nextNode = FindNode(rootNode.BottomNode, boxLength, boxWidth);
                            //    if (nextNode != null) { nextNode.rotated = true; }

                            //}
                        }

                    }
                    return nextNode;

                }


            }
            else if (boxWidth <= rootNode.Width && boxLength <= rootNode.Length)
            {
                return rootNode;
            }
            else
            {
                return null;
            }
        }

        private Node RightMostLeaf(Node root)
        {
            if (root == null)
                return null;

            while (root.RightNode != null)
                root = root.RightNode;

            return root;
        }

        private Node BottomMostLeaf(Node root)
        {
            if (root == null)
                return null;

            while (root.BottomNode != null)
                root = root.BottomNode;

            return root;
        }

        private Node SplitNode(Node node, double boxWidth, double boxLength)
        {
            node.IsOccupied = true;
            node.BottomNode = new Node { Y = node.Y + boxLength, X = node.X, Length = node.Length - boxLength + Kerf, Width = node.Width };
            node.RightNode = new Node { Y = node.Y, X = node.X + boxWidth, Length = boxLength, Width = node.Width - boxWidth + Kerf };


            return node;
        }

        private HeapBlock Intersect(HeapBlock firstHeap, HeapBlock secondHeap)
        {
            var x0 = Math.Max(firstHeap.X0, secondHeap.X0);
            var x1 = Math.Min(firstHeap.X1, secondHeap.X1);
            var y0 = Math.Max(firstHeap.Y0, secondHeap.Y0);
            var y1 = Math.Min(firstHeap.Y1, secondHeap.Y1);
            if (x0 <= x1 && y0 <= y1)
                return new HeapBlock
                {
                    X0 = x0,
                    Y0 = y0,
                    X1 = x1,
                    Y1 = y1
                };

            return null;
        }

        private bool ChunkContains(HeapBlock firstHeap, HeapBlock secondHeap)
        {
            // Determine whether heapBlock0 totally encompasses (ie, contains) heapBlock1.
            return firstHeap.X0 <= secondHeap.X0 && firstHeap.Y0 <= secondHeap.Y0 && secondHeap.X1 <= firstHeap.X1 && secondHeap.Y1 <= firstHeap.Y1;
        }

        private HeapBlock Expand(HeapBlock firstHeap, HeapBlock secondHeap)
        {
            // Extend heapBlock0 and heapBlock1 if they are
            // adjoining or overlapping.
            if (firstHeap.X0 <= secondHeap.X0 && secondHeap.X1 <= firstHeap.X1 && secondHeap.Y0 <= firstHeap.Y1)
            {
                secondHeap.Y0 = Math.Min(firstHeap.Y0, secondHeap.Y0);
                secondHeap.Y1 = Math.Max(firstHeap.Y1, secondHeap.Y1);
            }

            if (firstHeap.Y0 <= secondHeap.Y0 && secondHeap.Y1 <= firstHeap.Y1 && secondHeap.X0 <= firstHeap.X1)
            {
                secondHeap.X0 = Math.Min(firstHeap.X0, secondHeap.X0);
                secondHeap.X1 = Math.Max(firstHeap.X1, secondHeap.X1);
            }

            return secondHeap;
        }

        private void UnionMax(HeapBlock heapBlock0, HeapBlock heapBlock1)
        {
            // Given two heap blocks, determine whether...
            if (heapBlock0 != null && heapBlock1 != null)
            {
                // ...heapBlock0 and heapBlock1 intersect, and if so...
                var i = Intersect(heapBlock0, heapBlock1);
                if (i != null)
                {
                    if (ChunkContains(heapBlock0, heapBlock1))
                    {
                        // ...if heapBlock1 is contained by heapBlock0...
                        heapBlock1 = null;
                    }
                    else if (ChunkContains(heapBlock1, heapBlock0))
                    {
                        // ...or if heapBlock0 is contained by heapBlock1...
                        heapBlock0 = null;
                    }
                    else
                    {
                        // ...otherwise, let's expand both heapBlock0 and
                        // heapBlock1 to encompass as much of the intersected
                        // space as possible.  In this instance, both heapBlock0
                        // and heapBlock1 will overlap.
                        Expand(heapBlock0, heapBlock1);
                        Expand(heapBlock1, heapBlock0);
                    }
                }
            }
        }

        private void UnionAll()
        {

        }

        private Node growRight(float w, float h, Node rootNode)
        {
            rootNode = new Node()
            {
                IsOccupied = true,
                X = 0,
                Y = 0,
                Width = rootNode.Width + w,
                Length = rootNode.Length,
                BottomNode = rootNode,
                RightNode = new Node() { X = rootNode.Width, Y = 0, Width = w, Length = rootNode.Length }
            };

            Node node = FindNode(rootNode, w, h);
            if (node != null)
            {
                return SplitNode(node, w, h);
            }
            else
            {
                return null;
            }
        }

        private Node growDown(float w, float h, Node rootNode)
        {
            rootNode = new Node()
            {
                IsOccupied = true,
                X = 0,
                Y = 0,
                Width = rootNode.Width,
                Length = rootNode.Length + h,
                BottomNode = new Node() { X = 0, Y = rootNode.Length, Width = rootNode.Width, Length = h },
                RightNode = rootNode
            };
            Node node = FindNode(rootNode, w, h);
            if (node != null)
            {
                return SplitNode(node, w, h);
            }
            else
            {
                return null;
            }
        }


        #region Growing Box


        //private Node GrowNode(float w, float h)
        //{
        //    bool canGrowDown = (w <= rootNode.Width);
        //    bool canGrowRight = (h <= rootNode.Length);

        //    bool shouldGrowRight = canGrowRight && (rootNode.Length >= (rootNode.Width + w));
        //    bool shouldGrowDown = canGrowDown && (rootNode.Width >= (rootNode.Length + h));

        //    if (shouldGrowRight)
        //    {
        //        return growRight(w, h);
        //    }
        //    else if (shouldGrowDown)
        //    {
        //        return growDown(w, h);
        //    }
        //    else if (canGrowRight)
        //    {
        //        return growRight(w, h);
        //    }
        //    else if (canGrowDown)
        //    {
        //        return growDown(w, h);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //private Node growRight(float w, float h)
        //{
        //    rootNode = new Node()
        //    {
        //        IsOccupied = true,
        //        X = 0,
        //        Y = 0,
        //        Width = rootNode.Width + w,
        //        Length = rootNode.Length,
        //        BottomNode = rootNode,
        //        RightNode = new Node() { X = rootNode.Width, Y = 0, Width = w, Length = rootNode.Length }
        //    };

        //    Node node = FindNode(rootNode, w, h);
        //    if (node != null)
        //    {
        //        return SplitNode(node, w, h);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //private Node growDown(float w, float h)
        //{
        //    rootNode = new Node()
        //    {
        //        IsOccupied = true,
        //        X = 0,
        //        Y = 0,
        //        Width = rootNode.Width,
        //        Length = rootNode.Length + h,
        //        BottomNode = new Node() { X = 0, Y = rootNode.Length, Width = rootNode.Width, Length = h },
        //        RightNode = rootNode
        //    };
        //    Node node = FindNode(rootNode, w, h);
        //    if (node != null)
        //    {
        //        return SplitNode(node, w, h);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        #endregion
    }
}
