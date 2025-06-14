using NSJ_MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public class SaveHandler : MonoBehaviour, ISaveProvidable
    {
        ISavable _savable;

        private void Awake()
        {
            _savable = GetComponent<ISavable>();

            SubscribeSaveEvent();
        }

        private void OnDestroy()
        {
            UnsubscribeSaveEvent();
        }

        public void SaveModel()
        {
            if (_savable == null)
                return;

            // Json �ޱ�
            string modelJson = _savable.Save();

            // �� Json (���� ����) �̸� �ѱ��
            if (modelJson == string.Empty)
                return;

            // Json ����
            SaveManager.Instance.AddSaveModelData(modelJson);
        }

        public void LoadModel()
        {
            if (_savable == null)
                return;

            // Json�ҷ�����
            GameData data = SaveManager.Instance.Data;

            List<string> saveEntrys = data.Models;

            // Json �ѱ��
            string modelJson = _savable.Load(saveEntrys);
            SaveManager.Instance.RemoveModelData(modelJson);
        }

        private void SubscribeSaveEvent() 
        {
            SaveManager.Instance.OnSaveBeforeEvent += SaveModel;
        }
        private void UnsubscribeSaveEvent()
        {
            if(SaveManager.Instance == null) return;
            SaveManager.Instance.OnSaveBeforeEvent -= SaveModel;
        }


    }
}