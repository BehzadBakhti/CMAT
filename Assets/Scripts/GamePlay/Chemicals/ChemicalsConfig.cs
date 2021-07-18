using System.Collections.Generic;
using UnityEngine;

namespace Chemicals
{
    public class ChemicalsConfig : ScriptableObject
    {
        public List<SubstanceElement> mainElements;
        public List<Substance> mainCompounds;
    }
}