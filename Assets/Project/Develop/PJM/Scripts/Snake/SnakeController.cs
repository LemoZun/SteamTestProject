using System;
using System.Collections;
using System.Collections.Generic;
using GameEnum;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnakeController : MonoBehaviour
{
    // Todo : 먹이를 먹었을때, 죽었을 때 이벤트 만들어야함

    [Header("이동하는 주기(단위 : 초)")] 
    [SerializeField] private float _moveSpeed;
    public float moveSpeed
    {
        get => _moveSpeed;
        private set => _moveSpeed = value;
    }

    [Header("이동 방향(4방향)")] 
    [SerializeField] private Vector2Int _direction = Vector2Int.zero;

    public Vector2Int direction
    {
        get => _direction;
        private set => _direction = value;
    }

    [Header("생존 사망 확인")] 
    [SerializeField] private bool _isAlive;

    public bool isAlive
    {
        get => _isAlive;
        set => _isAlive = value;
    }


    public Head head;
    public LinkedList<Tail> tails;

    Coroutine moveCoroutine;

    private void Awake()
    {
        isAlive = true;
        direction = Vector2Int.right;
    }

    private void Start()
    {
        moveCoroutine = StartCoroutine(MoveRoutine());
    }

    private void Update()
    {
        InputDirection();
    }

    private void OnDestroy()
    {
        isAlive = false;
        StopCoroutine(moveCoroutine);
    }

    private IEnumerator MoveRoutine()
    {
        while (isAlive) //죽었을때 조건처리 필요
        {
            yield return new WaitForSeconds(moveSpeed);
            Move();
        }
    }

    private void Move()
    {
        // Todo : 머리 이동, 꼬리 이동
        // 현재 transform 값에 vector2Int값을 더해주면 됨
        head.gameObject.transform.position += new Vector3(direction.x, direction.y, 0);
        
        // 꼬리 리스트 순회하며 이동



    }

    private void InputDirection()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2Int.down) 
            direction = Vector2Int.up;
        if(Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2Int.up)
            direction = Vector2Int.down;
        if(Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2Int.right)
            direction = Vector2Int.left;
        if(Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2Int.left)
            direction = Vector2Int.right;
    }




}
