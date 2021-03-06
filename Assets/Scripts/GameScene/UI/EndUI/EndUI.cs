﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.GameScene
{
    public class EndUI : Common.UI<EndUI>
    {
        public Button ButtonRestart;
        public Button ButtonWorldMap;
        public Text Score;
        public Text Unicoin;

        protected override void Awake()
        {
            base.Awake();

            Time.timeScale = 0;

            Score.text = GameManager.Instance.Score.ToString("#,##0");
            Unicoin.text = GameManager.Instance.Coin.ToString("#,##0");

            ButtonRestart.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });

            ButtonWorldMap.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                LoadingSceneManager.LoadScene("PlanetSelect");
            });

            SaveGained();
        }

        private void SaveGained()
        {
            var currentPlayerData = DataManager.Instance.CurrentPlayerData;

            currentPlayerData.unicoin += GameManager.Instance.Coin;
            WebSocketManager.Instance.SendUpdateGoods("0", currentPlayerData.player_id, currentPlayerData.unicoin, currentPlayerData.cosmostone, currentPlayerData.oxygentank);

            foreach (var item in GameManager.Instance.dropMaterialList)
            {
                if (currentPlayerData.inventory.ContainsKey(item.Key))
                {
                    currentPlayerData.inventory[item.Key] += item.Value;
                    WebSocketManager.Instance.SendUpdateItem("0", currentPlayerData.player_id, item.Key, item.Value);
                }
                else
                {
                    currentPlayerData.inventory.Add(item.Key, item.Value);
                    WebSocketManager.Instance.SendInsertItem("0", currentPlayerData.player_id, item.Key, item.Value);
                }
            }
        }
    }
}