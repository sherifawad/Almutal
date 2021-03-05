using AlmutalCore.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore
{
    public class Algorithm
    {
        public float Width { get; private set; }
        public float Length { get; private set; }
        public List<Node> Gaps { get; private set; } = new List<Node>();

        private readonly List<Box> Boxes;
        public List<Node> Sheets { get; private set; }

        private Node CreateNode(int id)
        {
            if (Length > Width)
                return new Node { Length = Width, Width = Length, Id = id };
            else
                return new Node { Length = Length, Width = Width, Id = id };
        }

        public Algorithm(float width, float length, List<Box> boxes)
        {
            Width = width;
            Length = length;
            Boxes = boxes;

            Sheets = new List<Node>();
            var id = 0;
            // Sort boxes into descending order based on volume
            Boxes.ForEach(x => x.Area = (x.Length * x.Width));

            Boxes = Boxes.OrderByDescending(x => x.Area).ToList();

            Boxes.ForEach(x =>
            {
                x.Id = id;
                id++;

            });

            //// Initialize root node
            //rootNode = new Node { Length = Length, Width = Width, Id = 0 };
            ////Sheets.Add(rootNode);
        }


        //public void Packer()
        //{



        //    Pack();
        //    //Display();
        //    //Console.ReadLine();
        //}

        private void Display(List<Box> boxes)
        {
            foreach (var box in boxes)
            {
                var positionx = box.Position != null ? box.Position.X.ToString() : String.Empty;
                var positiony = box.Position != null ? box.Position.Y.ToString() : String.Empty;
                Console.WriteLine("Length : " + box.Length + " Width : " + box.Width +
                    " Pos_y  : " + positiony + " Pos_x : " + positionx +
                    " Id  : " + box.Id + " ParentID : " + box.ParentId
                    );
            }
        }

        public List<Box> Pack()
        {
            var boxes = new List<Box>();
            var id = 0;
            do
            {
                var rootNode = CreateNode(id);
                Sheets.Add(rootNode);
                id++;
                foreach (var box in Boxes.ToList())
                {
                    if (box.Used)
                        continue;
                    var boxWidth = box.Width;
                    var boxLength = box.Length;
                    if (box.Length > box.Width)
                    {
                        box.Width = boxLength;
                        box.Length = boxWidth;
                    }
                    var node = FindNode(rootNode, box.Width, box.Length);
                    if (node != null)
                    {
                        box.Position = SplitNode(node, box.Width, box.Length);

                        box.ParentId = rootNode.Id;
                        box.Used = true;
                        boxes.Add(box);
                        if (boxes.Count == Boxes.Count)
                            break;
                    }
                }

                if (boxes.Count == Boxes.Count)
                    break;
            } while (true);
            Display(boxes);
            return boxes;
        }

        private Node FindNode(Node rootNode, float boxWidth, float boxLength)
        {

            if (rootNode.IsOccupied)
            {
                var nextNode = FindNode(rootNode.RightNode, boxWidth, boxLength);
                if (nextNode == null)
                    nextNode = FindNode(rootNode.BottomNode, boxLength, boxWidth);
                return nextNode;
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

        private Node SplitNode(Node node, float boxWidth, float boxLength)
        {
            node.IsOccupied = true;
            node.BottomNode = new Node { Y = node.Y, X = node.X + boxWidth, Length = node.Length, Width = node.Width - boxWidth };
            node.RightNode = new Node { Y = node.Y + boxLength, X = node.X, Length = node.Length - boxLength, Width = boxWidth };
            return node;
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
