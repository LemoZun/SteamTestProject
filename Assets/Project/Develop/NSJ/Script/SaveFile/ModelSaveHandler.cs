using NSJ_MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public class ModelSaveHandler 
    {
        private readonly BaseModel _model;

        public ModelSaveHandler(BaseModel model)
        {
            _model = model; 
        }

        public string Save<TModel>() where TModel : BaseModel
        {
            // 세이브 안한다고 표시할 시 패스
            if (_model.CanSave == false)
                return string.Empty;

            string json = ToJson(_model);
            SaveEntry entry = new SaveEntry
            {
                SaveID = $"{typeof(TModel)}",
                Json = json
            };
            // SaveEntry 저장 로직
            string saveJson = ToJson(entry);

            return saveJson;
            // 여기에 SaveEntry를 저장하는 로직을 추가합니다.
            // 현재는 테스트 게임매니저에 넣었지만 이후 SaveManager로 변경할 예정입니다.
            //SaveManager.Instance.AddSaveModelData(saveJson);
        }

        public string Load<TModel>(List<string> saveEntrys) where TModel : BaseModel, ICopyable<TModel>
        {
            string returnJson = string.Empty;

            TModel loadData = null;

            // SaveEntry를 찾아서 로드하는 로직
            foreach (string entryJson in saveEntrys)
            {
                SaveEntry saveEntry = FromJson<SaveEntry>(entryJson);
                // entryJson.SaveID가 현재 모델의 SaveID와 일치하는지 확인합니다.
                if (saveEntry.SaveID == $"{typeof(TModel)}")
                {
                    // entryJson.Json을 사용하여 모델을 로드합니다.
                    loadData = FromJson<TModel>(saveEntry.Json);
                    // 사용한 모델 데이터 삭제
                    returnJson = entryJson;
                    break;
                }
            }
            // 모델의 속성에 복사합니다.
            AllCopyFrom(loadData);

            return returnJson;
        }
        /// <summary>
        /// 모델의 모든 데이터를 복사하는 메서드입니다.
        /// </summary>
        private void AllCopyFrom<T>(T loadData) where T : BaseModel, ICopyable<T>
        {
            if (loadData == null)
            {
                // 저장 데이터가 없는 경우 로드되지 않음 표시
                Debug.Log($"{typeof(T)} : 로딩할 데이터 없음");
                _model.IsLoaded = false;
            }
            else
            {
                _model.CanSave = loadData.CanSave;
                _model.IsLoaded = true;
                _model.HasViewID = loadData.HasViewID;
                _model.ViewID = loadData.ViewID;
                ((ICopyable<T>)_model).CopyFrom(loadData);
            }
        }
        private string ToJson<T>(T instance) where T : class
        {
            string json = JsonUtility.ToJson(instance);
            return json;
        }

        private T FromJson<T>(string json)
        {
            T model = JsonUtility.FromJson<T>(json);
            return model;
        }


    }
}