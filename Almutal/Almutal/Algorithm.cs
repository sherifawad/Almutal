using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DataBase.Models;
using Almutal.Helpers;

namespace Almutal
{
    public class Algorithm
    {
        public double Width { get; private set; }
        public double Length { get; private set; }
        public double Kerf { get; private set; }
        public List<Box> Boxes { get; private set; }


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
                        Title = box.Title
                        //Area = box.Width * box.Length
                    });
                //}

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
            node.BottomNode = new Node { Y = node.Y + boxLength, X = node.X, Length = node.Length - (boxLength + Kerf), Width = node.Width };
            node.RightNode = new Node { Y = node.Y, X = node.X + boxWidth, Length = boxLength, Width = node.Width - (boxWidth + Kerf) };


            return node;
        }

    }
}
