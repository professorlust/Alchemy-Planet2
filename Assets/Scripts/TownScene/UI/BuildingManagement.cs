﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;

namespace AlchemyPlanet.TownScene
{
    public class BuildingManagement : Common.UI<BuildingManagement>
    {
        public List<GameObject> buildingImages;
        public List<Text> infoText;
        public Text pageText;
        public Button leftButton;
        public Button rightButton;
        public Button closeButton;
        public Button buildingTap;              // 건물 탭
        public Button interiorTap;              // 인테리어 탭

        [SerializeField] private Sprite clickTap;       // 선택된 탭
        [SerializeField] private Sprite notClickTap;    // 선택되지 않은 탭
        [SerializeField] private RawImage blackMask;    // 마스크
        [SerializeField] private RawImage infoBlackMask;    // 마스크

        private List<Building> ownBuildings;    // 소유중인 건물
        private List<Interior> ownInteriors;    // 소유중인 인테리어
        private Dictionary<string, int> ownInteriorsCount;    // 소유중인 각 인테리어의 갯수
        private int page;                       // 현재 건물이미지 페이지
        private string tapType;                 // 선택된 탭


        private void OnEnable()
        {
            GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            leftButton.onClick.AddListener(() => { ChangePage(false); });
            rightButton.onClick.AddListener(() => { ChangePage(true); });
            closeButton.onClick.AddListener(() => { blackMask.gameObject.SetActive(false); UIManager.Instance.CloseMenu(); });

            buildingImages[0].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(0));
            buildingImages[1].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(1));
            buildingImages[2].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(2));
            buildingImages[3].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(3));
            buildingImages[4].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(4));
            buildingImages[5].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(5));
            buildingImages[6].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(6));
            buildingImages[7].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(7));
            buildingImages[8].GetComponent<Button>().onClick.AddListener(() => OpenBuildingInfo(8));

            blackMask.gameObject.SetActive(true);
            GetOwnBuilding();
            SetBuildingImage();
        }

        void OpenBuildingInfo(int n)
        {
            if (tapType == "Building")
                buildingImages[n].GetComponent<BuildingInfo>().SetInfo(ownBuildings[n + page * 9]);
            else if (tapType == "Interior")
                buildingImages[n].GetComponent<BuildingInfo>().SetInfo(ownInteriors[n + page * 9]);

            buildingImages[n].GetComponent<BuildingInfo>().OpenInfo();
            infoBlackMask.gameObject.SetActive(true);
        }

        public void GetOwnBuilding()   // 소유중인 건물 받아오기
        {
            ownBuildings = new List<Building>();

            foreach (Building building in DataManager.Instance.buildingInfo.Values)
            {
                ownBuildings.Add(building);
            }

            foreach (Building building in DataManager.Instance.CurrentPlayerData.buildings)
            {
                Building abc = ownBuildings.Find(a => a.buildingName == building.buildingName);
                abc = building;
            }

            ownInteriors = new List<Interior>();
            ownInteriorsCount = new Dictionary<string, int>();

            foreach (Interior interior in DataManager.Instance.interiorInfo.Values)
            {
                ownInteriors.Add(interior);
                ownInteriorsCount.Add(interior.interiorName, 0);
            }
            foreach (Interior interior in DataManager.Instance.CurrentPlayerData.interiors)
            {
                if (ownInteriors.Find(a => a.interiorName == interior.interiorName) != null)
                    ownInteriorsCount[interior.interiorName]++;
            }
        }

        void SetBuildingImage() // 소유중인 건물이미지 출력하기
        {
            for (int i = 0; i < 9; i++)
            {
                if (i < ownBuildings.Count - page * 9)
                {
                    if (!buildingImages[i].activeSelf)
                        buildingImages[i].SetActive(true);
                    buildingImages[i].GetComponent<Image>().sprite = ownBuildings[i + page * 9].image;
                    buildingImages[i].name = ownBuildings[i + page * 9].buildingName;
                    if (!infoText[i].gameObject.activeSelf)
                        infoText[i].gameObject.SetActive(true);
                    if (DataManager.Instance.CurrentPlayerData.buildings.Contains(ownBuildings[i + page * 9]))
                    {
                        if (ownBuildings[i + page * 9].material1Count <= DataManager.Instance.CurrentPlayerData.inventory[ownBuildings[i + page * 9].material1Name] &&
                          ownBuildings[i + page * 9].material2Count <= DataManager.Instance.CurrentPlayerData.inventory[ownBuildings[i + page * 9].material2Name] &&
                          ownBuildings[i + page * 9].material3Count <= DataManager.Instance.CurrentPlayerData.inventory[ownBuildings[i + page * 9].material3Name] &&
                          ownBuildings[i + page * 9].money <= DataManager.Instance.CurrentPlayerData.unicoin)
                            infoText[i].text = "Lv. " + ownBuildings[i + page * 9].buildingLevel.ToString() + "    강화가능";
                        else
                            infoText[i].text = "Lv. " + ownBuildings[i + page * 9].buildingLevel.ToString() + "    강화불가능";
                    }

                    else
                    {
                        if (ownBuildings[i + page * 9].material1Count <= DataManager.Instance.CurrentPlayerData.inventory[ownBuildings[i + page * 9].material1Name] &&
                      ownBuildings[i + page * 9].material2Count <= DataManager.Instance.CurrentPlayerData.inventory[ownBuildings[i + page * 9].material2Name] &&
                      ownBuildings[i + page * 9].material3Count <= DataManager.Instance.CurrentPlayerData.inventory[ownBuildings[i + page * 9].material3Name] &&
                      ownBuildings[i + page * 9].money <= DataManager.Instance.CurrentPlayerData.unicoin)
                            infoText[i].text = "제작가능";
                        else
                            infoText[i].text = "제작불가능";
                    }
                }
                else
                {
                    buildingImages[i].gameObject.SetActive(false);
                    infoText[i].gameObject.SetActive(false);
                }
            }
            tapType = "Building";
            pageText.text = page.ToString() + " / " + (ownBuildings.Count / 9 + 1).ToString();
            buildingTap.image.sprite = clickTap;
            interiorTap.image.sprite = notClickTap;
        }

        void SetInteriorImage() // 소유중인 건물이미지 출력하기
        {
            for (int i = 0; i < 9; i++)
            {
                if (i < ownInteriors.Count - page * 9)
                {
                    if (!buildingImages[i].activeSelf)
                        buildingImages[i].SetActive(true);
                    buildingImages[i].GetComponent<Image>().sprite = ownInteriors[i + page * 9].image;
                    buildingImages[i].name = ownInteriors[i + page * 9].interiorName;

                    if (ownInteriorsCount[ownInteriors[i + page * 9].interiorName] > 0)
                    {
                        if (ownInteriors[i + page * 9].material1Count <= DataManager.Instance.CurrentPlayerData.inventory[ownInteriors[i + page * 9].material1Name] &&
                       ownInteriors[i + page * 9].material2Count <= DataManager.Instance.CurrentPlayerData.inventory[ownInteriors[i + page * 9].material2Name] &&
                       ownInteriors[i + page * 9].material3Count <= DataManager.Instance.CurrentPlayerData.inventory[ownInteriors[i + page * 9].material3Name] &&
                       ownInteriors[i + page * 9].money <= DataManager.Instance.CurrentPlayerData.unicoin)
                            infoText[i].text = ownInteriorsCount[ownInteriors[i + page * 9].interiorName].ToString() + "개 소유중    제작가능";
                        else
                            infoText[i].text = ownInteriorsCount[ownInteriors[i + page * 9].interiorName].ToString() + "개 소유중    제작불가능";
                    }

                    else
                    {
                        if (ownInteriors[i + page * 9].material1Count <= DataManager.Instance.CurrentPlayerData.inventory[ownInteriors[i + page * 9].material1Name] &&
                      ownInteriors[i + page * 9].material2Count <= DataManager.Instance.CurrentPlayerData.inventory[ownInteriors[i + page * 9].material2Name] &&
                      ownInteriors[i + page * 9].material3Count <= DataManager.Instance.CurrentPlayerData.inventory[ownInteriors[i + page * 9].material3Name] &&
                      ownInteriors[i + page * 9].money <= DataManager.Instance.CurrentPlayerData.unicoin)
                            infoText[i].text = "제작가능";
                        else
                            infoText[i].text = "제작불가능";
                    }
                }
                else
                {
                    buildingImages[i].gameObject.SetActive(false);
                    infoText[i].gameObject.SetActive(false);
                }
            }
            tapType = "Interior";
            pageText.text = page.ToString() + " / " + (ownInteriors.Count / 9 + 1).ToString();
            interiorTap.image.sprite = clickTap;
            buildingTap.image.sprite = notClickTap;
        }

        void ChangePage(bool over)
        {
            if (tapType == "Building")
            {
                if (over)
                    page += ownBuildings.Count > (page + 1) * 9 ? 1 : 0;
                else
                    page -= page > 0 ? 1 : 0;
                SetBuildingImage();
            }
            else if (tapType == "Interior")
            {
                if (over)
                    page += ownInteriors.Count > (page + 1) * 9 ? 1 : 0;
                else
                    page -= page > 0 ? 1 : 0;
                SetInteriorImage();
            }
        }
    }
}
