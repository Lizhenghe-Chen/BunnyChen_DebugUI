using System;
using System.Collections;
using UnityEngine;

/* ······················································································ */
/* :██████╗ ██╗   ██╗███╗   ██╗███╗   ██╗██╗   ██╗     ██████╗██╗  ██╗███████╗███╗   ██╗: */
/* :██╔══██╗██║   ██║████╗  ██║████╗  ██║╚██╗ ██╔╝    ██╔════╝██║  ██║██╔════╝████╗  ██║: */
/* :██████╔╝██║   ██║██╔██╗ ██║██╔██╗ ██║ ╚████╔╝     ██║     ███████║█████╗  ██╔██╗ ██║: */
/* :██╔══██╗██║   ██║██║╚██╗██║██║╚██╗██║  ╚██╔╝      ██║     ██╔══██║██╔══╝  ██║╚██╗██║: */
/* :██████╔╝╚██████╔╝██║ ╚████║██║ ╚████║   ██║       ╚██████╗██║  ██║███████╗██║ ╚████║: */
/* :╚═════╝  ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═══╝   ╚═╝        ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═══╝: */
/* ······················································································ */
namespace BunnyChenAI.BunnyChen_DebugUI.Scripts
{
    public class DavidDebugUIExample : MonoBehaviour
    {
        // Start is called before the first frame update
        private DavidDebugUI _davidDebugUI;
        [SerializeField] private Rigidbody ballRigidbody;

        private void OnValidate()
        {
            gameObject.name = nameof(DavidDebugUIExample);
        }

        private void Awake()
        {
            _davidDebugUI = FindObjectOfType<DavidDebugUI>();
            _davidDebugUI.AddData("Mouse X", new DavidDebugUI.UnitData()
            {
                title = "Mouse X",
                value = 0,
                min = 0,
                max = Screen.width,
            });
            _davidDebugUI.AddData("Mouse Y", new DavidDebugUI.UnitData()
            {
                title = "Mouse Y",
                value = 0,
                min = 0,
                max = Screen.height
            });
            _davidDebugUI.AddData("Ball Position X", new DavidDebugUI.UnitData()
            {
                title = "Ball Position X",
                value = 0,
                min = -10,
                max = 10,
                lineSpeed = 10,
                dotRate = 5,
            });
            _davidDebugUI.AddData("Ball Position Y", new DavidDebugUI.UnitData()
            {
                title = "Ball Position Y",
                value = 0,
                min = -10,
                max = 10,
                lineSpeed = 10,
                dotRate = 5,
            });
            _davidDebugUI.AddData("Ball Position Z", new DavidDebugUI.UnitData()
            {
                title = "Ball Position Z",
                value = 0,
                min = -10,
                max = 10,
                lineSpeed = 10,
                dotRate = 5,
            });
            _davidDebugUI.AddData("Ball Velocity", new DavidDebugUI.UnitData()
            {
                title = "Ball Velocity",
                value = 0,
                min = 0,
                max = 25
            });
        }

        private void Start()
        {
            UseCoroutine();
        }


        // private void Update()
        // {
        //     UpdateBallData();
        //     UpdateMouseData();
        // }
        private void UseCoroutine()
        {
            StartCoroutine(UpdateGraphCoroutine(0.1f));
            return;

            IEnumerator UpdateGraphCoroutine(float f)
            {
                while (true)
                {
                    UpdateBallData();
                    UpdateMouseData();
                    yield return new WaitForSeconds(f);
                }
                // ReSharper disable once IteratorNeverReturns
            }
        }


        private void UpdateMouseData()
        {
            _davidDebugUI.UpdateGraph("Mouse X", Input.mousePosition.x, 0, Screen.width);
            _davidDebugUI.UpdateGraph("Mouse Y", Input.mousePosition.y, 0, Screen.height);
        }

        private void UpdateBallData()
        {
            _davidDebugUI.UpdateGraph("Ball Position X", ballRigidbody.position.x);
            _davidDebugUI.UpdateGraph("Ball Position Y", ballRigidbody.position.y);
            _davidDebugUI.UpdateGraph("Ball Position Z", ballRigidbody.position.z);
            _davidDebugUI.UpdateGraph("Ball Velocity", ballRigidbody.velocity.magnitude);
        }
    }
}