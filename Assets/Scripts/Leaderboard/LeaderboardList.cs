using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class LeaderboardList : MonoBehaviour
{
    [SerializeField] private LeaderboardItem LeaderboardItemPrefab;
    [SerializeField] private GameObject LeaderboardItemsContainer;
    private LeaderboardService leaderboardService;
    private bool newlyAddedScoreMarked;
    private LeaderboardItem newestLeaderboardItem;

    void Awake()
    {
        leaderboardService = new LeaderboardService(Application.persistentDataPath);
        leaderboardService.LoadAllData();
    }

    private void Start()
    {
        leaderboardService.GetData().OrderByDescending(data => data.score).ToList().ForEach(RenderScoreItemsInList);
        ScrollNewRecordIntoView();
    }

    private void ScrollNewRecordIntoView()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(LeaderboardItemsContainer.GetComponent<RectTransform>());

        float containerHeight = LeaderboardItemsContainer.GetComponent<RectTransform>().rect.height;
        Vector2 scrollPosition = LeaderboardItemsContainer.GetComponent<RectTransform>().anchoredPosition;
        scrollPosition.y += containerHeight / leaderboardService.GetData().Count() * newestLeaderboardItem.transform.GetSiblingIndex();
        LeaderboardItemsContainer.GetComponent<RectTransform>().anchoredPosition = scrollPosition;
    }

    private void RenderScoreItemsInList(ScoreData data)
    {
        newlyAddedScoreMarked = false;
        LeaderboardItem leaderboardItem = Instantiate(LeaderboardItemPrefab, LeaderboardItemsContainer.transform);
        leaderboardItem.SetNameText(data.name);
        leaderboardItem.SetScoreText(data.score);
        if (!newlyAddedScoreMarked && IsNewlyAddedScore(data))
        {
            newlyAddedScoreMarked = true;
            leaderboardItem.MarkAsNew();
            newestLeaderboardItem = leaderboardItem;
        }
    }

    private bool IsNewlyAddedScore(ScoreData data)
    {
        ScoreData newestScore = leaderboardService.GetNewestScoreData();
        return AreScoresEqual(newestScore, data);
    }

    private bool AreScoresEqual(ScoreData a, ScoreData b)
    {
        return a.name == b.name && a.score == b.score;
    }
}
