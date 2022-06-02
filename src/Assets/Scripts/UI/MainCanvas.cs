using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MainCanvas : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameManager game_manager;
    [SerializeField]
    private CanvasGroup level_completed_text_group;
    [SerializeField]
    private CanvasGroup next_button_group;
    [SerializeField]
    private CanvasGroup level_failed_text_group;
    [SerializeField]
    private CanvasGroup retry_button_group;

    private Vector2 level_completed_text_first_pos;
    private Vector2 next_button_first_pos;
    private RectTransform level_completed_text_rect;
    private RectTransform next_button_rect;

    private Vector2 level_failed_text_first_pos;
    private Vector2 retry_button_first_pos;
    private RectTransform level_failed_text_rect;
    private RectTransform retry_button_rect;
    #endregion
    private void Start()
    {
        level_completed_text_rect = level_completed_text_group.GetComponent<RectTransform>();
        next_button_rect = next_button_group.GetComponent<RectTransform>();

        level_completed_text_first_pos = level_completed_text_rect.anchoredPosition;
        next_button_first_pos= next_button_rect.anchoredPosition;

        level_failed_text_rect = level_failed_text_group.GetComponent<RectTransform>();
        retry_button_rect = retry_button_group.GetComponent<RectTransform>();

        level_failed_text_first_pos = level_failed_text_rect.anchoredPosition;
        retry_button_first_pos = retry_button_rect.anchoredPosition;
        HideElements();
    }
    #region Commands
    public void LevelCompletedScreen()
    {
        next_button_group.alpha = 1;
        level_completed_text_group.alpha = 1;

        level_completed_text_rect.DOAnchorPos(level_completed_text_first_pos,0.1f).SetEase(Ease.Linear);
        next_button_rect.DOAnchorPos(next_button_first_pos, 0.1f).SetEase(Ease.Linear);

    }
    public void LevelFailedScreen()
    {
        retry_button_group.alpha = 1;
        level_failed_text_group.alpha = 1;

        level_failed_text_rect.DOAnchorPos(level_failed_text_first_pos, 0.1f).SetEase(Ease.Linear);
        retry_button_rect.DOAnchorPos(retry_button_first_pos, 0.1f).SetEase(Ease.Linear);

    }
    public void HideElements()
    {
        level_completed_text_rect.anchoredPosition = Vector2.up * 150;
        next_button_rect.anchoredPosition = Vector2.down * 150;

        next_button_group.alpha = 0;
        level_completed_text_group.alpha = 0;

        level_failed_text_rect.anchoredPosition = Vector2.up * 150;
        retry_button_rect.anchoredPosition = Vector2.down * 150;

        retry_button_group.alpha = 0;
        level_failed_text_group.alpha = 0;
    }
    public void NextLevel()
    {
        game_manager.NextLevel();
        HideElements();
    }

    public void RetryLevel()
    {
        game_manager.RetryLevel();
        HideElements();
    }
    #endregion
}
