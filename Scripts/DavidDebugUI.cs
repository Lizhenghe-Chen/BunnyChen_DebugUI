using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace BunnyChenAI.BunnyChen_DebugUI.Scripts
{
    public class DavidDebugUI : MonoBehaviour
    {
        [SerializeField] private RectTransform content;

        /// <summary>
        /// make sure each unit has height 100
        /// </summary>
        [SerializeField] private RectTransform unitTemplate;

        // public int unitWidth = 50;
        // public int unitHeight = 100;


        /// <summary>
        /// please add and assign the unit in other scripts, see the example in <see cref="DavidDebugUIExample"/>
        /// </summary>
        [Header("For Debug Only")] [ReadOnly] public List<UnitData> unitDataList = new();

        private readonly Dictionary<string, UnitData> _unitDataDict = new();

        [Serializable]
        public class UnitData
        {
            public string title;
            public float value;
            public float min = -50;
            public float max = 50;
            public RectTransform line;

            /// <summary>
            ///  the speed of the line, or the startSpeed of the particle system, default is<c>50</c>
            /// </summary>
            public int lineSpeed = 50;

            /// <summary>
            ///  the max length of the line, or the startLifetime of the particle system, default is <c>10</c>
            /// </summary>
            public int maxLength = 10;

            /// <summary>
            /// the rate of the line, or the rate over time of the particle system, default is <c>10</c>
            /// </summary>
            public int dotRate = 10;

            /// <summary>
            /// draw the line between the dots
            /// </summary>
            public bool showLine = true;

            [FormerlySerializedAs("unitGameObject")]
            public RectTransform unitRectTransform;

            public TMPro.TextMeshProUGUI titleText;

            // 缓存 ParticleSystem 引用
            public ParticleSystem particleSystem;
        }

        private void Awake()
        {
            foreach (var t in _unitDataDict)
            {
                var unit = Instantiate(unitTemplate, content);
                t.Value.line = unit.Find("LineParent/LineValue(Particle)").GetComponent<RectTransform>();
                t.Value.unitRectTransform = unit;
                t.Value.titleText = unit.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                // 缓存 ParticleSystem
                t.Value.particleSystem = t.Value.line.GetComponent<ParticleSystem>();
                var mainModule = t.Value.particleSystem.main;
                mainModule.startColor = GenerateRandomVividColor();
                SetUnitTitle(t.Value, t.Key, mainModule.startColor.color);
                // unit.sizeDelta = new Vector2(0, 0);
            }

            // GetComponent<RectTransform>().sizeDelta = new Vector2(0 * _unitDataDict.Count, 0);
            Destroy(content.GetChild(0).gameObject); //  transform.GetChild(0).gameObject.SetActive(false);
        }

        public void AddData(string title, UnitData unitData)
        {
            unitDataList.Add(unitData);
            _unitDataDict.Add(title, unitData);
        }

        public void UpdateGraph(string title, float value, float min = 0, float max = 0)
        {
            if (!_unitDataDict.TryGetValue(title, out var value1)) return;
            value1.value = value;
            if (!Mathf.Approximately(min, 0)) value1.min = min;
            if (!Mathf.Approximately(max, 0)) value1.max = max;
            UpdateUnitUI(value1);
        }

        // private void FixedUpdate()
        // {
        //     foreach (var t in _unitDataDict)
        //     {
        //         UpdateUnitUI(t.Value);
        //     }
        // }

        /// <summary>
        /// the max unit.line.anchoredPosition.y is unitHeight, and the min unit.line.anchoredPosition.y is 0,
        /// So the value of the unit will be mapped to the range of 0 to unitHeight
        /// </summary>
        /// <param name="unit"></param>
        private void UpdateUnitUI(UnitData unit)
        {
            //map the value to the range of 0 to unitHeight
            var value = Mathf.InverseLerp(unit.min, unit.max, unit.value) * unit.unitRectTransform.sizeDelta.y;
            unit.line.anchoredPosition = new Vector2(0, value);
            // 优化：直接用缓存的 ParticleSystem
            ChangeParticleSystem(unit.particleSystem, unit.lineSpeed, unit.maxLength, unit.dotRate, unit.showLine);
            SetUnitTitle(unit, unit.title + "\n" + unit.value.ToString("F2"));
        }

        /// <summary>
        /// set particle system emission speed,max particles, and rate over time
        /// </summary>
        /// <param name="particleSystem"></param>
        /// <param name="speed"></param>
        /// <param name="maxLength"></param>
        /// <param name="dotRate"></param>
        /// <param name="showLine"></param>
        private static void ChangeParticleSystem(ParticleSystem particleSystem, float speed, float maxLength,
            int dotRate, bool showLine)
        {
            var emission = particleSystem.emission;
            emission.rateOverTime = dotRate;
            var mainModule = particleSystem.main;
            mainModule.startSpeed = speed;
            mainModule.startLifetime = maxLength;
            //set trails
            var trails = particleSystem.trails;
            trails.enabled = showLine;
        }

        private static void SetUnitTitle(UnitData unitData, string unitTitle, Color color = default)
        {
            if (!unitData.titleText) return;
            unitData.titleText.text = unitTitle;
            if (color != default)
            {
                unitData.titleText.color = color;
            }
        }

        /// <summary>
        /// Use this method to generate a random vivid color
        /// Convert HSV to RGB
        /// </summary>
        /// <returns> a random vivid color</returns>
        private static Color GenerateRandomVividColor()
        {
            return Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        }
    }
}