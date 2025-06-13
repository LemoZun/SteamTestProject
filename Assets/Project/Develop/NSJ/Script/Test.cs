using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NSJTest
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private ScoreView view1;
        [SerializeField] private ScoreView view2;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                 view1.ExchangeViewModel(view2);
           
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                view1.RemoveViewModel();
            }
        }
    }

}
