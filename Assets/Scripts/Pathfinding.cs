using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private class Node
    {
        public Node parent;
        public Vector3 position;
        public float f, g, h;


        public Node(Node parent, Vector3 position)
        {
            this.parent = parent;
            this.position = position;
            g = 0;
            h = 0;
            f = g + h;
        }

        public Node(Vector3 position)
        {
            this.position = position;
            g = 0;
            h = 0;
            f = g + h;
        }

    }

    public static List<Vector3> AStar(Vector3 start, Vector3 end)
    {
        //Debug.Log("Starting Astar");
        Node startNode = new Node(start);
        Node endNode = new Node(end);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);
        // int loopCounter = 0;
        //Debug.Log("Entering while loop");
        while (openList.Count > 0)
        {

            //loopCounter++;

            Node currentNode = openList[0];
            int currentIndex = 0;
            //Debug.Log("Finding node with smallest f");
            for (int i = 0; i < openList.Count; i++)
            {
                
                if (openList[i].f < currentNode.f)
                {
                    currentNode = openList[i];
                    currentIndex = i;
                }
            }

            openList.RemoveAt(currentIndex);
            closedList.Add(currentNode);

            //if we find the goal
            if (currentNode.position == endNode.position)
            {
                //Debug.Log("found goal");
                List<Vector3> path = new List<Vector3>();
                Node current = currentNode;
                //int count = 0;
                while (current != null)
                {
                    path.Add(current.position);
                    current = current.parent;
                }

                path.Reverse();
                return path;
            }

           // Debug.Log("Creating neighbors");
            List<Vector3> neighbors = new List<Vector3>();
            neighbors.Add(new Vector3(0, 1, 0));
            neighbors.Add(new Vector3(0, -1, 0));
            neighbors.Add(new Vector3(1, 0, 0));
            neighbors.Add(new Vector3(-1, 0, 0));

           // Debug.Log("Validating neighbors");
            List<Node> children = new List<Node>();
            foreach (Vector3 newPosition in neighbors)
            {

                Vector3 node_position = new Vector3(currentNode.position.x + newPosition.x, currentNode.position.y + newPosition.y, 0);

                if (node_position.x > (GridManager.instance.GetOriginPos().x + GridManager.instance.GetWidth())
                    || node_position.x < (GridManager.instance.GetOriginPos().x)
                    || node_position.y > (GridManager.instance.GetOriginPos().y + GridManager.instance.GetHeight())
                    || node_position.y < (GridManager.instance.GetOriginPos().y))
                {
                    //Debug.Log("Neighbor not inside of grid");
                    continue;
                }

                //Make sure terrain isnt occupied
                if (GridManager.instance.IsOccupied(node_position))
                {
                    //Debug.Log("neighbor's position is occupied by another structure");
                    continue;
                }

                
                Node newNode = new Node(currentNode, node_position);

                children.Add(newNode);

            }

            //Debug.Log("looping through children");
            foreach (Node child in children)
            {

                bool skipChild = false;
                //skip child if its on the closed list
                //Debug.Log("checking child against closedlist");
                foreach (Node closedChild in closedList)
                {
                    
                    if (child.position == closedChild.position)
                    {
                        skipChild = true;
                        break;
                    }
                }

                if (skipChild == true)
                {
                    //Debug.Log("Child found in closedlist..skipped");
                    continue;
                }

                //assign f, g and h values (using manhatten distance metric)
                child.g = currentNode.g + 1;
                child.h = Mathf.Abs(currentNode.position.x - endNode.position.x) + Mathf.Abs(currentNode.position.y - endNode.position.y);
                child.f = child.g + child.h;

                //skip child if already in open list and has larger g value
                //Debug.Log("checking child against openlist");
                foreach (Node openNode in openList)
                {
                    if (child.position == openNode.position && child.g >= openNode.g)
                    {
                        skipChild = true;
                        break;
                    }
                }

                if (skipChild == true)
                {
                    //Debug.Log("Child with larger g found in openlist..skipped");
                    continue;
                }

                openList.Add(child);
            }



        }

        Debug.Log("Could not find a path. Returning Null");
        return null;
    }

}
