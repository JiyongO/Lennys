using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Unit : MonoBehaviour
    {
        bool isInit;
        Node curNode;
        Node targetNode;

        public bool move;
        public float lerpSpeed = 1;
        float baseSpeed;
        bool initLerp;
        Vector3 targetPos;
        Vector3 startPos;
        float t;
        GameManager gameManager;
        int t_x;
        int t_y;

        public void Init(GameManager gm)
        {
            gameManager = gm;
            PlaceOnNode();
            isInit = true;
        }

        void Start()
        {

        }

        void PlaceOnNode()
        {
            curNode = gameManager.spawnNode;
            transform.position = gameManager.spawnPosition;
        }
        // Update is called once per frame
        void Update()
        {
            if (!isInit)
                return;
            if (!move)
                return;
            if (!initLerp)
            {
                initLerp = true;
                startPos = transform.position;
                t = 0;
                Pathfind();
                Vector3 tp = gameManager.GetWorldPosFromNode(targetNode);
                targetPos = tp;
                float d = Vector3.Distance(targetPos, startPos);
                baseSpeed = lerpSpeed / d;
            }
            else
            {
                t += Time.deltaTime * baseSpeed;
                if (t > 1)
                {
                    t = 1;
                    initLerp = false;
                    curNode = targetNode;
                }

                Vector3 tp = Vector3.Lerp(startPos, targetPos, t);
                transform.position = tp;
            }
        }

        void Pathfind()
        {
            t_x = curNode.x;
            t_y = curNode.y;

            Node nextDown = gameManager.GetNode(t_x, t_y - 1);
            if (nextDown == null)
                return;
            if (!nextDown.isEmpty)
            {
                t_y = curNode.y;
            }
            else
            {
                t_y -= 1;
            }

            targetNode = gameManager.GetNode(t_x, t_y);
        }
    }
}
