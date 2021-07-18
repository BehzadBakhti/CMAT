using System;
using System.Collections.Generic;

namespace Chemicals
{
    public class ChemicalsManager

    {
        private List<Substance> _compounds;
        private SubstanceMixer _mixer;
        public List<Substance> GetData()
        {
            if (_compounds == null)
            {
                _compounds = new List<Substance>();
                //TODO:  load data from file or DB Deserialized from ChemicalsData Class
            }
            return _compounds;
        }


        public Substance Mix(List<Substance> input)
        {
            _mixer ??= new SubstanceMixer();
            return _mixer.Mix(input);
        }

    }

    [Serializable]
    public class ChemicalsData
    {
        public List<Substance> compounds;
    }
}
