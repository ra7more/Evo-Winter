﻿using UnityEngine;
using System.Collections;

public class HurtByWeapon : MonoBehaviour
{

    private int camp;
    public GameObject HitPrefab;
    private Vector3 tempPosition;
    private Vector3 lastPosition;

    // Use this for initialization
    void Start()
    {
 
        camp = gameObject.GetComponentInParent<Character>().Camp;
    }

    void LateUpdate()
    {
        //记录下之前的位置
        lastPosition = tempPosition;
        tempPosition = transform.position;
    }

    /// <summary>
    /// 2D碰撞检测
    /// </summary>
    /// <param name="other">与其碰撞的GameObj</param>
    void OnTriggerEnter(Collider other)
    {

        //如果武器无效化,则返回
        if (gameObject.GetComponentInParent<Character>().IsWeaponDmg == 0)
            return;


        if ((other.tag == "Enemy" && camp == 0) || (other.tag == "Player" && camp == 1))
        {
            //若有生命,则减血
            //Debug.Log("Enemy hurt!" + other.GetComponent<Character>().Health);
            Character ch=other.GetComponent<Character>();
            if (ch.IsAlive < 0)
                return;
            ch.Health -= gameObject.GetComponentInParent<Character>().AttackDamage;

            //Debug.Log("AttackHit;" + other.tag + CharacterManager.Instance.CharacterList.IndexOf(other.GetComponent<Character>()));

            //发送命中敌人消息
            gameObject.GetComponentInParent<Character>().Notify("AttackHit;" + other.tag + ";" + CharacterManager.Instance.CharacterList.IndexOf(other.GetComponent<Character>()));

            //Debug.Log("111");
            return;
            //添加打击效果
            Vector2[] points = this.GetComponent<PolygonCollider2D>().points;

            Vector3 origin = lastPosition;//上一帧的位置
            Vector3 end = this.transform.position;//触发时的位置
            //方案二：
            //Vector3 direction = end - origin;//射线方向
            //float distance = Vector3.Distance(origin, end);//射线检测距离
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 newOrigin = origin + new Vector3(points[i].x, points[i].y);
                Vector3 newEnd = end + new Vector3(points[i].x, points[i].y);
                //方案一：判断边界点与物体是否碰撞
                if(other.GetComponent<BoxCollider2D>().OverlapPoint(newOrigin)==false&&other.GetComponent<BoxCollider2D>().OverlapPoint(newEnd)==true)
                {
                   Instantiate(HitPrefab, newEnd, Quaternion.identity);  //在碰撞点实例化打击特效
                   break;
                }
                //方案二：射线检测
                ////this.GetComponent<PolygonCollider2D>().enabled = false;
                //RaycastHit2D hit = Physics2D.Raycast(newOrigin, direction, distance, 1 << LayerMask.NameToLayer("EnemyRB2D"));//发射射线，只检测与"Target"层的碰撞
                ////this.GetComponent<PolygonCollider2D>().enabled = true;
                //Debug.DrawRay(newOrigin, direction, Color.red, 2);//绘制射线
                ////Debug.Assert(hit.collider != null, "未检测到起点");
                //if (hit.collider != null)
                //{
                //    //Debug.Log(hit.point + "不为null");

                //    Instantiate(HitPrefab, hit.point, Quaternion.identity);
                //    break;

                //}
                //else
                //{
                //    // Debug.Log(hit.point + "为null");
                //}
            }
        }


    }
}
