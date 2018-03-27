using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationHex : MonoBehaviour {

    private NavigationMesh mesh;
    private Vector2 pos;
    private List<NavigationHex> neighbours;

    public void setPos() {

    }

    public void addNeighbour() {
    }

    public IEnumerator<NavigationHex> getNeighbourIterator() {
        return neighbours.GetEnumerator();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mesh.setOccupied(pos, collision);
    }

    public override bool Equals(object other)
    {
        if (other is NavigationHex)
        {
            NavigationHex otherHex = (NavigationHex) other;
            return otherHex.pos == pos;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return pos.GetHashCode();
    }
}
