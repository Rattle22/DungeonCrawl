using RatStudios.Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMesh
{
    public enum HexDir
    {
        TopLeft,
        TopRight,
        Left,
        Right,
        BottomLeft,
        BottomRight
    }

    private struct NavigationHex
    {

        private NavigationMesh mesh;
        public NavigationMesh Mesh {
            get { return mesh; }
            set { mesh = value; }
        }
        private Vector2 pos;
        public Vector2 Position {
            get { return pos; }
        }
        private List<NavigationHex> neighbours;

        public NavigationHex(Vector2 pos, NavigationMesh mesh)
        {
            this.pos = pos;
            this.mesh = mesh;
            neighbours = new List<NavigationHex>();
        }

        public IEnumerator<NavigationHex> GetNeighbourEnumerator() {
            return neighbours.GetEnumerator();
        }

        public void RemoveNeighbour(NavigationHex neighbour) {
            neighbours.Remove(neighbour);
        }

        /// <summary>
        /// Removes all links between this hex and other hexes.
        /// </summary>
        public void Remove() {
            foreach (NavigationHex n in neighbours) {
                n.RemoveNeighbour(this);
            }

            neighbours.Clear();
        }

        /// <summary>
        /// Creates a two-way link between the given and this hex.
        /// </summary>
        /// <param name="h"></param>
        public void AddNeighbour(NavigationHex h) {
            h.LinkNeighbour(this);
            LinkNeighbour(h);
        }

        /// <summary>
        /// Creates a one-way Link from this hex to the given hex.
        /// </summary>
        /// <param name="h"></param>
        private void LinkNeighbour(NavigationHex h) {
            if (!neighbours.Contains(h)) {
                neighbours.Add(h);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is NavigationHex) {
                NavigationHex n = (NavigationHex) obj;
                return n.pos == pos;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return pos.GetHashCode();
        }
    }

    //How far each hex middle is from the center of the containing square, bar row adjustments
    private float xOffset;
    private float yOffset;

    private static float hexWidth = 0.5f;
    private float hexHeight;
    private float hexHeightMod;
    private static Dictionary<HexDir, Vector2> dirDict = new Dictionary<HexDir, Vector2>();

    private Dictionary<Vector2, NavigationHex> map = new Dictionary<Vector2, NavigationHex>();

    static NavigationMesh()
    {
        dirDict.Add(HexDir.Left, Vector2.left);
        dirDict.Add(HexDir.Right, Vector2.right);

        Quaternion rot = Quaternion.AngleAxis(60, Vector3.forward);
        dirDict.Add(HexDir.TopRight, (rot * Vector3.right).normalized);
        rot = Quaternion.AngleAxis(120, Vector3.forward);
        dirDict.Add(HexDir.TopLeft, (rot * Vector3.right).normalized);

        rot = Quaternion.AngleAxis(240, Vector3.forward);
        dirDict.Add(HexDir.BottomLeft, (rot * Vector3.right).normalized);
        rot = Quaternion.AngleAxis(300, Vector3.forward);
        dirDict.Add(HexDir.BottomRight, (rot * Vector3.right).normalized);
    }

    public NavigationMesh()
    {
        foreach (Vector2 v in dirDict.Values)
        {
            Debug.Log(v);
        }
    }

    private void Start()
    {
        hexHeight = hexWidth / (Mathf.Cos(30 * Mathf.Deg2Rad));
        hexHeightMod = hexHeight * 0.75f;

        /*foreach (Vector2 pos in map.Keys) {
            //One adjecent hex is always at the same y, but one is either at y + 1 or y - 1, depending on the row
            int dy = 1;
            if (pos.y % 2 == 1) {
                dy = -1;
            }

            NavigationHex hex;
            NavigationHex neigh;
            Action add = () => { neigh.addNeighbour(hex); };
        }*/
    }

    public static NavigationMesh generateFromDoor(Door door, GameObject instance)
    {
        NavigationMesh navMesh = new NavigationMesh();
        GameObject parent = new GameObject();

        List<NavigationHex> validPos = new List<NavigationHex>();
        Queue<NavigationHex> toCheck = new Queue<NavigationHex>();

        Vector2 actPos = door.transform.position;
        navMesh.xOffset = actPos.x;
        navMesh.yOffset = actPos.y;

        toCheck.Enqueue(new NavigationHex(new Vector2(0,0), navMesh));

        int iterations = 0;
        int maxIt = 5000;
        Vector2 v;
        RaycastHit2D[] trash = new RaycastHit2D[1];

        //TODO: Works well enough for now, but could use some work. Especially: 
        while (toCheck.Count > 0 && iterations < maxIt)
        {
            iterations++;

            NavigationHex currentHex = toCheck.Dequeue();
            v = currentHex.Position;

            Vector2[] diffs = new Vector2[] {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, 1),
                new Vector2(0, -1),
                new Vector2(1, -1),
                new Vector2(-1, 1)
            };

            foreach (Vector2 diff in diffs)
            {
                //The coordinate in the hex grid
                Vector2 hexPos = v + diff;
                //The coordinate in world space
                //TODO: Rename square to world space
                Vector2 squarePos = navMesh.hexToSquare(hexPos);

                NavigationHex hex = new NavigationHex(hexPos, navMesh);

                if (!validPos.Contains(hex))
                {
                    //TODO: Define Hex in here, instead of stupidly passing it
                    Collider2D[] cols = Physics2D.OverlapCircleAll(squarePos, hexWidth, (1 << 8));

                    if (cols.Length == 0)
                    {
                        toCheck.Enqueue(hex);
                    }

                    GameObject obj = GameObject.Instantiate(instance);
                    obj.transform.position = squarePos;
                    obj.transform.SetParent(parent.transform);
                    validPos.Add(hex);
                    hex.AddNeighbour(currentHex);
                }
                else {
                    hex.AddNeighbour(currentHex);
                }
            }
        }

        Debug.Log("Iteration: " + iterations);
        if (iterations == maxIt)
        {
            Debug.Log("Iteration overflow!");
        }

        //navMesh.SetMesh();

        return navMesh;
    }

    public Vector2 hexToSquare(Vector2 pos)
    {
        return hexToSquare(pos.x, pos.y);
    }

    public Vector2 hexToSquare(float x, float y)
    {
        Vector2 pos = new Vector2(xOffset, yOffset);
        Vector2 xInc;
        Vector2 yInc;
        dirDict.TryGetValue(HexDir.Right, out xInc);
        dirDict.TryGetValue(HexDir.TopRight, out yInc);
        pos += x * xInc * hexWidth;
        pos += y * yInc * hexWidth;

        return pos;

    }

    public void squareToHex() {
    }

    public void setOccupied(Vector2 hexPos, bool occupied)
    {

    }
}