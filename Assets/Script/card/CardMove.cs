using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;

namespace Client
{

    public class CardMove : MonoBehaviour
    {
        Card card;

        float moveTime;
        float remainMoveTime;
        bool isRotating;
        bool isMoving;
        Vector3 speed;
        Vector3 targetPosition;
        Quaternion startAngle;
        Quaternion targetAngle;

        // Start is called before the first frame update
        void Awake()
        {
            card = GetComponent<Card>();
            moveTime = 0.2f;
        }

        // Update is called once per frame
        void Update()
        {
            SmoothMove();
        }


        private void SmoothMove()
        {
            if (remainMoveTime > 0)
            {
                if (isMoving)
                {
                    gameObject.transform.position += speed * Time.deltaTime;
                }

                if (isRotating)
                {
                    gameObject.transform.rotation = Quaternion.Lerp(targetAngle, startAngle, remainMoveTime / moveTime);
                }
                remainMoveTime -= Time.deltaTime;
            }
            else if (isMoving || isRotating)
            {
                DirectedMove(targetPosition, targetAngle, card.localRotation);
            }
        }

        public void SetPositionAndRotation(Vector3 targetPosition, Quaternion targetAngle, int localRotation)
        {
            card.localRotation = localRotation;
            this.targetAngle = Quaternion.AngleAxis(localRotation, transform.forward) * targetAngle;
            if (this.targetAngle.Equals(gameObject.transform.rotation))
            {
                isRotating = false;
            }
            else
            {
                remainMoveTime = moveTime;
                isRotating = true;
                startAngle = gameObject.transform.rotation;
            }

            if (targetPosition.Equals(gameObject.transform.position))
            {
                isMoving = false;
            }
            else
            {
                remainMoveTime = moveTime;
                isMoving = true;
                speed = (targetPosition - gameObject.transform.position) / moveTime;
                this.targetPosition = targetPosition;
            }
        }

        public void DirectedMove(Vector3 targetPosition, Quaternion targetAngle, int localRotation)
        {
            card.localRotation = localRotation;
            remainMoveTime = 0;
            isMoving = false;
            isRotating = false;
            this.targetPosition = targetPosition;
            startAngle = Quaternion.AngleAxis(localRotation, transform.forward) * targetAngle;
            this.targetAngle = startAngle;
            gameObject.transform.SetPositionAndRotation(targetPosition, targetAngle);
        }
    }
}
