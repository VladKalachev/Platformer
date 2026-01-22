using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PixelCrew.Model.Definitions.Localization
{
    public class localeItems : ScriptableObject
    {
        // en https://docs.google.com/spreadsheets/d/e/2PACX-1vSsyxtZPV_j571OfhgLx93fFBVS1AbRqa6EMupb4gtNqXysKGOkWm147p7DqRjTU2RFd2haYOkB0i1K/pub?gid=1551408748&single=true&output=tsv
        // ru https://docs.google.com/spreadsheets/d/e/2PACX-1vSsyxtZPV_j571OfhgLx93fFBVS1AbRqa6EMupb4gtNqXysKGOkWm147p7DqRjTU2RFd2haYOkB0i1K/pub?gid=0&single=true&output=tsv

        [SerializeField] private string _url;
        [SerializeField] private LocaleItem[] _localeItems;

        private UnityWebRequest _request;
        
        [ContextMenu("Update locale")]
        public void LoadLocale()
        {
            if (_request != null) return;
            
            _request = UnityWebRequest.Get(_url);
            _request.SendWebRequest().completed += OnDataLoaded;
        }

        private void OnDataLoaded(AsyncOperation operation)
        {
            if (operation.isDone)
            {
                var rows =  _request.downloadHandler.text.Split('\n');
                var items = new List<LocaleItem>();
                foreach (var row in rows)
                {
                    AddLocaleItem(row, items);
                }

                _localeItems = items.ToArray();
            }
        }

        private void AddLocaleItem(string row, List<LocaleItem> localeItems)
        {
            try
            {
               var parts = row.Split('\t');
               localeItems.Add(new LocaleItem{Key = parts[0], Value = parts[1]});
            }
            catch (Exception e)
            {
                Debug.LogError($"Can't parse row: {row}. \n {e}");
            }
        }

        [Serializable]
        private class LocaleItem
        {
            public string Key;
            public string Value;
        }
    }
}