using JYW.Buffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYW.UI
{
    public class BuffPresenter : MonoBehaviour
    {
        [SerializeField]
        private Character bindTarget;

        [SerializeField]
        private BuffUi buffUiPrefab;

        private Dictionary<Buff, BuffUi> buffUis = new();

        private void OnEnable()
        {
            bindTarget.Buffs.BuffAdded += AddBuffUi;
            bindTarget.Buffs.BuffRemoved += RemoveBuffUi;
        }
        private void OnDisable()
        {
            bindTarget.Buffs.BuffAdded -= AddBuffUi;
            bindTarget.Buffs.BuffRemoved -= RemoveBuffUi;
        }

        private void AddBuffUi(Buff buff)
        {
            BuffUi buffUi = Instantiate(buffUiPrefab, transform);
            buffUi.Set(buff);
            buffUis.Add(buff, buffUi);
            //Debug.Log("?");
        }

        private void RemoveBuffUi(Buff buff)
        {
            //Debug.Log(buffUis.Count);

            buffUis[buff].Remove();
            //Destroy(buffUis[buff].gameObject);
            buffUis.Remove(buff);
            
            //foreach (var item in buffUis)
            //{
            //    Debug.Log(item.Value.ToString());
            //}
        }
    }
}