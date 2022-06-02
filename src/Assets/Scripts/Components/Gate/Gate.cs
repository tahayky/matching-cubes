using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    #region Variables
    public GateType gate_type;
    [SerializeField]
    private float width;
    #endregion

    #region Methods
    public SortingType Pass()
    {
        switch (gate_type)
        {
            case GateType.Random:
                return SortingType.Random;
            case GateType.Order:
                return SortingType.Color;
        }
        return SortingType.Color;
    }
    #endregion

}
