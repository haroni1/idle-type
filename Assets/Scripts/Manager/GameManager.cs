using UnityEngine;

public enum GameState
{
    Day,
    Night,
    Shop
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public GameState currentState = GameState.Shop; // 시작은 준비/상점 상태라고 가정

    [Header("Resources")]
    public int rawChickenCount = 0;
    public int money = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 개발용 키보드 전환 (원하면 다른 키로 바꿔도 됨)
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartDay();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            StartNight();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            StartShop();
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Day:
                Debug.Log("=== Day 시작 (버튼으로 전환) ===");
                break;
            case GameState.Night:
                Debug.Log("=== Night 시작 (버튼으로 전환) ===");
                break;
            case GameState.Shop:
                Debug.Log("=== Shop 상태 ===");
                break;
        }
    }

    // ▼ UI 버튼 or 키보드에서 호출할 함수들

    public void StartDay()
    {
        ChangeState(GameState.Day);
    }

    public void StartNight()
    {
        ChangeState(GameState.Night);
    }

    public void StartShop()
    {
        ChangeState(GameState.Shop);
    }

    public void AddChicken(int amount)
    {
        rawChickenCount += amount;
        Debug.Log($"생닭 수: {rawChickenCount}");
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log($"돈: {money}");
    }
}
