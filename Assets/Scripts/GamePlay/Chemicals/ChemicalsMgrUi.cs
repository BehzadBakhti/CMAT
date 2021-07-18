using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chemicals
{
    public class ChemicalsMgrUi : MonoBehaviour
    {
        [SerializeField] private RectTransform loadedSubstancesParent;
        [SerializeField] private RectTransform baseSubstancesParent;
        [SerializeField] private RectTransform substanceProperties;
        [SerializeField] private RectTransform combinationSlotsParent;
        [SerializeField] private GameObject substanceItemPrefab;
        [SerializeField] private TextMeshProUGUI header;

        private List<Substance> mixingList;

        public void init(List<Substance> loadedSubs, List<Substance> baseSubs)
        {

            for (int i = 0; i < loadedSubs.Count; i++)
            {
                var go = Instantiate(substanceItemPrefab, loadedSubstancesParent);

            }
        }
    }

    public class SubstanceItemUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameTag;
        [SerializeField] private Image icon;
        [SerializeField] private Button btn;

    }
}