using System.Globalization;
using System.Threading;
using UnityEngine;

public class StupidInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
	}
}
