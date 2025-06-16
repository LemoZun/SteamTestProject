using NSJ_MVVM;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NSJ_SaveUtility
{
    public class SaveManager : SingleTon<SaveManager>
    {
        public GameData Data;

        public List<string> Models => Data.Models;

        public event UnityAction OnSaveBeforeEvent;
        public event UnityAction OnSaveEvent;
        public event UnityAction<List<string>> OnLoadEvent;


        /// <summary>
        /// ���� ���̺� �̺�Ʈ�� �����մϴ�
        /// </summary>
        public static void RegisterModel<TModel>(TModel model) where TModel : BaseModel
        {
            SetSingleton();
            model.SubscribeSaveEvent<TModel>();
        }

        /// <summary>
        /// ���� ���̺� �̺�Ʈ�� �����մϴ�
        /// </summary>
        public static void UnRegisterModel<TModel>(TModel model) where TModel : BaseModel
        {
            model.UnsubscribeSaveEvent<TModel>();
        }


        /// <summary>
        /// �����͸� �����մϴ�
        /// </summary>
        /// <param name="saveNumber"></param>
        /// <returns></returns>
        public bool SaveData(int saveNumber = int.MinValue)
        {
            Data.Models.Clear();
            OnSaveBeforeEvent?.Invoke();

            if (saveNumber == int.MinValue)
            {
                saveNumber = Data.SaveNumber;
            }

            DateTime now = DateTime.Now;
            Data.LastSaveTime = now.ToString("yyyy-MM-dd HH:mm:ss");

            bool success = SaveUtility.Save(ref Data, saveNumber);


            if (success)
            {
                OnSaveEvent?.Invoke();
            }

            return success;
        }

        /// <summary>
        /// �����͸� �ε��մϴ�
        /// </summary>
        /// <param name="saveNumber"></param>
        /// <returns></returns>
        public bool LoadData(int saveNumber)
        {
            Data = SaveUtility.Load(saveNumber, out bool success);

            if (success == false)
            {
                if (Data == null)
                {
                    Data = new GameData();
                    Data.SaveNumber = saveNumber;
                }
            }

            OnLoadEvent?.Invoke(Data.Models);

            return success;
        }

        /// <summary>
        /// ���̺�ID ��Ī ���з����� ������ �ε� ���нÿ� ���� Ÿ�� ĳ���� �ε�� �޼ҵ�
        /// </summary>
        public bool TryReload<TModel>(TModel model, out TModel returnModel) where TModel : BaseModel, ICopyable<TModel>
        {
            returnModel = null;

            foreach (string json in Models)
            {
                SaveEntry entry = FromJson<SaveEntry>(json);
                TModel loadModel = FromJson<TModel>(entry.Json);

                if (loadModel != null) 
                {
                    // ��ġ�Ѵٰ� �Ǵ�
                    returnModel = loadModel;
                    RemoveModelData(json);
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// �� �����͸� �߰��մϴ�.
        /// </summary>
        public void AddSaveModelData(string saveJson)
        {
            Data.Models.Add(saveJson);
        }

        /// <summary>
        /// �� �����͸� �����մϴ�.
        /// </summary>
        public void RemoveModelData(string saveJson)
        {
            Data.Models.Remove(saveJson);
        }

        /// <summary>
        /// Ŭ������ Jsonȭ �մϴ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        private string ToJson<T>(T instance) where T : class
        {
            string json = JsonUtility.ToJson(instance);
            return json;
        }

        /// <summary>
        /// json�� Ŭ����ȭ �մϴ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        private T FromJson<T>(string json)
        {
            T model = JsonUtility.FromJson<T>(json);
            return model;
        }
    }
}