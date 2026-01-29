using UnityEngine;
using System.Collections;
using TMPro; // TextMeshProを使う場合

public class PuzzleTimer : MonoBehaviour
{
    // 参照するコンポーネント
    public ST_PuzzleDisplay puzzleDisplay;
    public TextMeshProUGUI timerText; // 3D/Canvas内のテキストコンポーネント
    public Renderer clickableObjectRenderer; // クリック可能なオブジェクトのレンダラー

    // 内部状態管理
    private bool isShuffling = false;
    private bool isPlaying = false;
    private float startTime;
    private float elapsedTime;

    void Start()
    {
        // 初期状態としてタイマーを非表示またはリセット
        if (timerText != null)
        {
            timerText.text = "0.0";
        }
    }

    // Updateはプレイ中に時間を更新するために使用
    void Update()
    {
        if (isPlaying)
        {
            elapsedTime = Time.time - startTime;
            if (timerText != null)
            {
                // 時間表示を更新 (例: 10.53秒)
                timerText.text = elapsedTime.ToString("F1");
            }
            
            // パズル完了チェックを毎フレーム行う
            if (puzzleDisplay.Complete)
            {
                StopTimer(elapsedTime);
            }
        }
    }

    // 3Dオブジェクトをクリックしたときに呼ばれる
    void OnMouseDown()
    {
        // クリックされたのがタイマーオブジェクト自身、かつゲームが開始前であるか確認
        if (!isPlaying && !isShuffling)
        {
            StartShuffleAndTimer();
        }
    }

    public void StartShuffleAndTimer()
    {
        if (isShuffling || isPlaying) return;

        isShuffling = true;
        Debug.Log("Shuffle Started.");

        // シャッフルコルーチンを開始
        StartCoroutine(StartShufflingAndWaiting());
    }

    IEnumerator StartShufflingAndWaiting()
    {
        // ST_PuzzleDisplayのシャッフルロジック（JugglePuzzle）を呼び出す
        // ただし、JugglePuzzle内でCheckForCompleteが呼ばれているため、そのロジックを再利用
        
        // 既存のJugglePuzzleは内部でCheckForCompleteを開始してしまうため、
        // ここではST_PuzzleDisplayにシャッフルのみを実行する公開メソッドが必要です
        // 例として、JugglePuzzleの処理を直接記述する、または修正後のJugglePuzzleの最後にCheckForCompleteを呼ばないように仮定
        
        // --- ここでシャッフル処理 ---
        // 外部からシャッフルを呼び出す例 (ST_PuzzleDisplayに 'public IEnumerator ShuffleOnly()' が必要)
        yield return StartCoroutine(puzzleDisplay.StartShuffle()); 
        
        // **暫定的な代替案:** シャッフルにかかる時間を仮定して待機 (ST_PuzzleDisplayのJugglePuzzleに依存)
        yield return new WaitForSeconds(3.0f); // JugglePuzzleの処理時間（適当な秒数）を待つ

        // シャッフル終了フラグ
        isShuffling = false;
        
        // タイマー開始
        startTime = Time.time;
        isPlaying = true;
        Debug.Log("Timer Started.");
        
        // パズルが完了していないことを確認し、完成判定を開始
        puzzleDisplay.Complete = false;
        StartCoroutine(puzzleDisplay.CheckForComplete());
    }

    public void StopTimer(float finalTime)
    {
        if (!isPlaying) return;

        isPlaying = false;
        
        // Completion時の処理（ST_PuzzleDisplayから呼ばれる場合を想定）
        Debug.Log($"Puzzle Complete! Total time: {finalTime.ToString("F1")} seconds.");
        
        // タイマー表示を確定
        if (timerText != null)
        {
            timerText.text = finalTime.ToString("F1");
        }
    }
}