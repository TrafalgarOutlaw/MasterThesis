using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Grid;
using VRTRPG.Place;

namespace VRTRPG.Movement
{
    public class MoveableUnitWalker : AGridMoveable
    {
        private bool isMoving = false;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Vector3 targetDir;
        private float startTime;
        [SerializeField] Animator animator;
        [SerializeField] Transform visualTransform;
        [SerializeField] float movementSpeed;

        public override int walkDistance { get; protected set; }

        new void Start()
        {
            base.Start();
            walkDistance = 4;
        }

        void Update()
        {
            if (isMoving)
            {
                if (Vector3.Distance(transform.localPosition, targetPosition) < .1f)
                {
                    visualTransform.position.Scale(new Vector3(1, 0, 1));
                    animator.SetBool("IsWalking", false);
                    isMoving = false;
                    return;
                }
                transform.localPosition += targetDir * Time.deltaTime * movementSpeed;
            }
        }

        public override void StartMovePhase()
        {
            movementSystem.StartMovePhase(this);
        }

        public override HashSet<AGridCell> GetAvailableCells()
        {
            HashSet<AGridCell> walkableCells = new HashSet<AGridCell>();
            // CurrentCell = transform.parent.GetComponent<AGridCell>();
            foreach (var cell in CurrentCell.GetNeighbor())
            {
                GetAvailableCellsRecursive(cell, walkDistance).ForEach(value =>
                {
                    walkableCells.Add(value);
                });
            }
            return walkableCells;
        }

        private List<AGridCell> GetAvailableCellsRecursive(AGridCell gridCell, int walkDistance)
        {
            List<AGridCell> walkableCells = new List<AGridCell>();
            if (walkDistance <= 0) return walkableCells;

            AGridCell cellAbove = GridSystem.Instance.GetGridCell(gridCell.Index + Vector3Int.up);

            Field field = null;

            if ((cellAbove != null && !cellAbove.CanBuild())
                || (gridCell.IncludedGameobjects == null || gridCell.IncludedGameobjects.Count == 0)
                || !gridCell.IncludedGameobjects.Exists(go =>
                {
                    if (go.TryGetComponent<Field>(out field))
                    {
                        if (field.isWalkable) return true;
                    }
                    return false;
                })
                || gridCell.IncludedGameobjects.Exists(go =>
                {
                    if (go.TryGetComponent<AGridMoveable>(out AGridMoveable moveable))
                    {
                        return true;
                    }
                    return false;
                })
                || gridCell.Index.y != CurrentCell.Index.y
                || gridCell == CurrentCell)
            {
                return walkableCells;
            }
            walkableCells.Add(gridCell);
            foreach (var cell in gridCell.GetNeighbor())
            {
                walkableCells.AddRange(GetAvailableCellsRecursive(cell, walkDistance - 1));
            }
            return walkableCells;
        }

        public override void MoveTo(AGridCell cell)
        {
            CurrentCell.RemoveIncludedObject(gameObject);

            animator.SetBool("IsWalking", true);
            isMoving = true;
            transform.parent = cell.transform;
            startPosition = transform.localPosition;
            targetPosition = Vector3.zero;
            targetDir = targetPosition - startPosition;


            animator.speed = movementSpeed;

            targetDir = targetDir.normalized;
            visualTransform.LookAt(cell.WorldPosition);

            CurrentCell = transform.parent.GetComponent<AGridCell>();
            CurrentCell.IncludedGameobjects.Add(gameObject);
        }

        public override void DoMove()
        {
            movementSystem.ShowWalkableCells();
        }
    }
}