using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public class StageSystem : MonoBehaviour
    {
        private InputManager inputManager;
        private GridSystem gridSystem;

        [SerializeField] Vector3Int activeStagePlane;

        // Start is called before the first frame update
        void Start()
        {
            inputManager = InputManager.Instance;
            gridSystem = GridSystem.Instance;
            inputManager.OnChangeLevel.AddListener(ChangeActiveStage);


            if (!IsActiveStagePlaneValid())
            {
                activeStagePlane = new Vector3Int(-1, 1, -1);
            }
            SetActiveStage(activeStagePlane);
        }

        void SetToProperIndex(int index, int dimension)
        {
            switch (dimension)
            {
                case 1:
                    if (index < 0) activeStagePlane.y += gridSystem.GetDimensionsVector()[dimension];
                    else activeStagePlane.y %= gridSystem.GetDimensionsVector()[dimension];
                    return;
                case 2:
                    if (index < 0) activeStagePlane.z += gridSystem.GetDimensionsVector()[dimension];
                    else activeStagePlane.z %= gridSystem.GetDimensionsVector()[dimension];
                    return;
                default:
                    if (index < 0) activeStagePlane.x += gridSystem.GetDimensionsVector()[dimension];
                    else activeStagePlane.x %= gridSystem.GetDimensionsVector()[dimension];
                    return;
            }
        }


        private bool IsActiveStagePlaneValid()
        {
            int isPlaneIndicator = 0;
            if (activeStagePlane.x < 0) isPlaneIndicator++;
            if (activeStagePlane.y < 0) isPlaneIndicator++;
            if (activeStagePlane.x < 0) isPlaneIndicator++;
            return isPlaneIndicator == 2;
        }

        void ChangeActiveStage(bool next)
        {
            if (activeStagePlane.x >= 0)
            {
                if (next) activeStagePlane.x++;
                else activeStagePlane.x--;
                SetToProperIndex(activeStagePlane.x, 0);

                SetActiveStage(activeStagePlane);
            }
            if (activeStagePlane.y >= 0)
            {
                if (next) activeStagePlane.y++;
                else activeStagePlane.y--;
                SetToProperIndex(activeStagePlane.y, 1);

                SetActiveStage(activeStagePlane);
            }
            if (activeStagePlane.z >= 0)
            {
                if (next) activeStagePlane.z++;
                else activeStagePlane.z--;
                SetToProperIndex(activeStagePlane.z, 2);

                SetActiveStage(activeStagePlane);
            }
        }

        void SetActiveStage(Vector3Int activeStage)
        {
            Vector3Int dimensionVector = gridSystem.GetDimensionsVector();
            AGridCell[,,] gridCellArray = gridSystem.GetGridcellArray();

            for (int x = 0; x < dimensionVector.x; x++)
            {
                for (int y = 0; y < dimensionVector.y; y++)
                {
                    for (int z = 0; z < dimensionVector.z; z++)
                    {
                        if (activeStage.x != -1)
                        {
                            if (x == activeStage.x)
                            {
                                gridCellArray[x, y, z].EnableCollider();
                            }
                            else
                            {
                                gridCellArray[x, y, z].DisableCollider();
                            }
                        }
                        if (activeStage.y != -1)
                        {
                            if (y == activeStage.y)
                            {
                                gridCellArray[x, y, z].EnableCollider();
                            }
                            else
                            {
                                gridCellArray[x, y, z].DisableCollider();
                            }
                        }
                        if (activeStage.z != -1)
                        {
                            if (z == activeStage.z)
                            {
                                gridCellArray[x, y, z].EnableCollider();
                            }
                            else
                            {
                                gridCellArray[x, y, z].DisableCollider();
                            }
                        }
                    }
                }
            }

        }
    }
}
