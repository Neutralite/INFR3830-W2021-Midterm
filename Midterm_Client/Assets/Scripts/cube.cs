using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cube : MonoBehaviour
{
    public InputField chatBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!chatBox.isFocused)
        {
            transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 2f,
                                0, Input.GetAxis("Vertical") * Time.deltaTime * 2f);
        }

    }
}
