using System;
using System.Collections;
using System.Collections.Generic;
using GameEnum;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnakeController : MonoBehaviour
{
    [Header("꼬리 프리팹")] // 임시로 여기에 둠
    [SerializeField] private Tail tailPrefab;
    
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

    public bool isAlive { get => _isAlive; set => _isAlive = value; }


    private Head _head;
    public Head head { get => _head; private set => _head = value; }

    /*private Tail _firstTail;
    public Tail firstTail { get => _firstTail; private set => _firstTail = value; }*/
    
    public LinkedList<Tail> tails;

    private Coroutine moveCoroutine;

    private void Awake()
    {
        isAlive = true;
        direction = Vector2Int.right;
        tails = new LinkedList<Tail>();
    }

    private void Start()
    {
        head = GetComponentInChildren<Head>();
        tails.AddLast(GetComponentInChildren<Tail>());
        moveCoroutine = StartCoroutine(MoveRoutine());
        SubscribeEvents();
    }

    private void Update()
    {
        InputDirection();
        
        /*if(Input.GetKeyDown(KeyCode.Space))
            AddTail();*/
    }

    private void OnDestroy()
    {
        isAlive = false;
        tails.Clear();
        UnsubscribeEvents();
        StopCoroutine(moveCoroutine);
    }

    private IEnumerator MoveRoutine()
    {
        while (isAlive) 
        {
            yield return moveSpeed.GetDelay();
            Move();
            head.CheckEatOrDie();
        }
    }

    private void Move()
    {
        // Todo : 머리 이동, 꼬리 이동
        // 현재 transform 값에 vector2Int값을 더해주면 됨
        Vector3 prevPos = head.transform.position;
        head.gameObject.transform.position += new Vector3(direction.x, direction.y, 0);
        
        // 후에 생긴 꼬리도 따라오는지 검증 필요
        foreach (var tail in tails)
        {
            (tail.transform.position, prevPos) = (prevPos, tail.transform.position);
        }

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

    private void AddTail()
    {
        Tail newTail = Instantiate(tailPrefab, gameObject.transform);
        newTail.transform.position = tails.Last.Value.transform.position;
        tails.AddLast(newTail);
    }

    private void DieSnake()
    {
        isAlive = false;
        Debug.Log("게임 오버");
    }

    private void CheckEatFood()
    {
        
    }

    private void SubscribeEvents()
    {
        if (head == null)
        {
            Debug.LogError("Head is null");
            return;
        }
        TempEventManager.Instance.OnFoodEaten += AddTail;
        TempEventManager.Instance.OnSnakeDied += DieSnake;
    }

    private void UnsubscribeEvents()
    {
        if (head == null)
            return;
        TempEventManager.Instance.OnFoodEaten -= AddTail;
        TempEventManager.Instance.OnSnakeDied -= DieSnake;
    }




}
