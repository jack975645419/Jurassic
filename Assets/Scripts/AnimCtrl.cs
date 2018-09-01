using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl : MonoBehaviour {
    public PseudoInput m_PseudoInput = new PseudoInput();
    public PseudoInput m_ModifiedInput = null;

    // Use this for initialization
    public virtual void Start () {
		
	}

    // Update is called once per frame
    public virtual void Update () {
        PreProcessPseudoInput();

    }

    public virtual void PreProcessPseudoInput()
    {
        m_ModifiedInput = m_PseudoInput.GetCopy();
    }
}
