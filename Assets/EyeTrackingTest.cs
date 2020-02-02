using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class EyeTrackingTest : MonoBehaviour
{

    private Renderer _myRend;
    private GazeAware _gazeAware;
    List<Color> _colors;

    // Start is called before the first frame update
    void Start()
    {
        _myRend = gameObject.GetComponent<Renderer>();
        _gazeAware = gameObject.GetComponent<GazeAware>();
        _colors = new List<Color>
        {
            Color.red,
            Color.blue,
            Color.cyan,
            Color.green,
            Color.magenta,
            Color.yellow
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (_gazeAware.HasGazeFocus)
        {
            _myRend.material.color = Color.magenta;//_colors[( Random.Range(0,_colors.Count))];
        }
        else
        {
            _myRend.material.color = Color.blue;
        }

        Debug.Log(TobiiAPI.GetGazePoint().Viewport.ToString());
        
        if (TobiiAPI.GetFocusedObject() != null)
        {
            Debug.Log(TobiiAPI.GetFocusedObject().name);

        }

    }
}
