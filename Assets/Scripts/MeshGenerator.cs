using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;
    public void GenerateMesh(int[,] map, float sqaureSize)
    {
        squareGrid = new SquareGrid(map, sqaureSize);
    }

    private void OnDrawGizmos()
    {
        if(squareGrid != null)
            for(int ii=0; ii < squareGrid.squares.GetLength(0); ii++)
                for(int jj=0; jj < squareGrid.squares.GetLength(1); jj++)
                {
                    Gizmos.color = (squareGrid.squares[ii, jj].topLeft.active) ? Color.red : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[ii, jj].topLeft.position, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squares[ii, jj].topRight.active) ? Color.red : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[ii, jj].topRight.position, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squares[ii, jj].bottomRight.active) ? Color.red : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[ii, jj].bottomRight.position, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squares[ii, jj].bottomLeft.active) ? Color.red : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[ii, jj].bottomLeft.position, Vector3.one * 0.4f);
                    
                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(squareGrid.squares[ii, jj].centerTop.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[ii, jj].centerRight.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[ii, jj].centerBottom.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[ii, jj].centerLeft.position, Vector3.one * 0.15f);

                }
    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];
            for(int ii=0; ii < nodeCountX; ii++)
            {
                for(int jj=0; jj < nodeCountY; jj++) {
                    Vector3 pos = new Vector3(-mapWidth / 2 + ii * squareSize / 2, 0, -mapHeight + jj * squareSize);
                    controlNodes[ii, jj] = new ControlNode(pos, map[ii, jj] == 1, squareSize);
                }
            }
            squares = new Square[nodeCountX-1, nodeCountY-1];
            for(int ii=0; ii<nodeCountX-1; ii++)
                for(int jj=0; jj<nodeCountY-1; jj++)
                    squares[ii, jj] = new Square(controlNodes[ii, jj+1], controlNodes[ii+1, jj+1], controlNodes[ii+1, jj], controlNodes[ii, jj]);
        }
    }
    public class Square
    {
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node centerTop, centerRight, centerBottom, centerLeft;
        
        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomLeft, ControlNode bottomRight)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;   
            this.bottomLeft = bottomLeft;
            this.bottomRight = bottomRight;

            centerTop = topLeft.right;
            centerRight = bottomRight.above;
            centerBottom = bottomLeft.right;
            centerLeft = bottomLeft.above;
        }
    }
    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 pos)
        {
            position = pos;
        }
    }

    public class ControlNode : Node
    {
        public bool active;
        public Node above, right;

        public ControlNode(Vector3 pos, bool active, float squareSize) : base(pos)
        {
            this.active = active;
            above = new Node(position + Vector3.forward * squareSize / 2);
            right = new Node(position + Vector3.right * squareSize / 2);
        }
    }
}
