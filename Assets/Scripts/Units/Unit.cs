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
        bool movingLeft;
        public float lerpSpeed = 1;
        public SpriteRenderer ren;
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

            ren.flipX = movingLeft;
        }

        void Pathfind()
        {
            t_x = curNode.x;
            t_y = curNode.y;

            bool downIsAir = IsAir(t_x, t_y - 1);
            bool fowardIsAir = IsAir(t_x, t_y);

            if (downIsAir)
            {
                t_x = curNode.x;
                t_y -= 1;
            }
            else
            {
                if (fowardIsAir)
                {
                    t_x = (movingLeft) ? t_x - 1 : t_x + 1;
                    t_y = curNode.y;
                }
                else
                {
                    int s = 0;
                    bool isValid = false;
                    while (s < 3)
                    {
                        s++;
                        bool f_isAir = IsAir(t_x, t_y + s);
                        if (f_isAir)
                        {
                            isValid = true;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        t_y += s;
                    }
                    else
                    {
                        movingLeft = !movingLeft;
                        t_x = (movingLeft) ? curNode.x - 1 : curNode.y + 1;
                    }
                }
            }
            targetNode = gameManager.GetNode(t_x, t_y);
        }

        bool IsAir(int x, int y)
        {
            Node n = gameManager.GetNode(x, y);
            if (n == null)
                return true;
            return n.isEmpty;
        }
    }
}
