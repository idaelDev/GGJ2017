using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage the conversation and redirect dialog in function of player's answer
/// </summary>
public class ConversationManager : Singleton<ConversationManager> {

    const int END_TOPICS_POS = 0;
    const int END_TOPICS_NEU = 1;
    const int END_TOPICS_NEG = 2;
    const int MALUS_STOP_CONV_POS = 3;
    const int MALUS_STOP_CONV_NEU = 4;
    const int MALUS_STOP_CONV_NEG = 5;
    const int STOP_3_FAIL_POS = 6;
    const int STOP_3_FAIL_NEU = 7;
    const int STOP_3_FAIL_NEG = 8;

    /// <summary>
    /// The state of the negociator. ARBITRARY AND REALLY MESSY
    /// 0 = neutral
    /// neg = upset
    /// pos = happy
    /// </summary>
    public int NegociatorState { get; private set; }

    public delegate void EndGameDelegate();
    public event EndGameDelegate endGameEvent;

    private int currentTopicID = 0;
    private TopicObject topic;

    private int phraseInTopic = 0;

    public DialogPrinterScript printer;

    private int lastState = 0;
    private int stopCounter = 0;
    private int leaveSteps = 0; //how much time the player have to use the "leave" button

    private int globalScore;

    public AudioSource audio;

    // Use this for initialization
    void Start ()
    {
        printer.endOfPrintEvent += NextPhrase;
        MainMenu.instance.startGameEvent += NextPhrase;
    }

    void NextPhrase()
    {
        if (phraseInTopic == 0)
        {
            topic = DialogStore.Instance.GetTopic(currentTopicID, NegociatorState);
        }
        if(currentTopicID >= DialogStore.Instance.MaxTopicId)
        {
            ///GAME OVER
            Debug.Log("GAME OVER");
            DialogObject dial;
            if (NegociatorState < 0)
            {
                dial = DialogStore.Instance.GetBonus(END_TOPICS_NEG);
            }
            else if (NegociatorState > 0)
            {
                dial = DialogStore.Instance.GetBonus(END_TOPICS_POS);
            }
            else
            {
                dial = DialogStore.Instance.GetBonus(END_TOPICS_NEU);
            }

            switchToEnd();
            printer.PrintDialog(dial.Text, dial.TimeOnScreen, NegociatorState);
            ///
        }
        else
        {
            if (phraseInTopic == topic.Count)
            {
                printer.setupAnswers(topic.NegativeAnswer, topic.NeutralAnswer, topic.PositiveAnswer, topic.LeaveAnswer);
                phraseInTopic = 0;
            }
            else
            {
                DialogObject dial = topic.getDialog(phraseInTopic);
                printer.PrintDialog(dial.Text, dial.TimeOnScreen, NegociatorState);
                phraseInTopic++;
            }
        }
    }

    private void switchToEnd()
    {
        printer.endOfPrintEvent -= NextPhrase;
        printer.endOfPrintEvent += EndGame;
    }

    private void EndGame()
    {
        endGameEvent();
        audio.Play();
    }

    public void Answer(int type, bool endByTimer)
    {
        printer.HideAnswer();
        NegociatorState += type;
        globalScore += type;
        //Compute stop conditions
        if (NegociatorState == lastState)
        {
            stopCounter++;
        }
        if (stopCounter >= 3)
        {
            ///END GAME
            DialogObject dial;
            if (NegociatorState < 0)
            {
                dial = DialogStore.Instance.GetBonus(STOP_3_FAIL_NEG);
            }
            else if (NegociatorState > 0)
            {
                dial = DialogStore.Instance.GetBonus(STOP_3_FAIL_POS);
            }
            else
            {
                dial = DialogStore.Instance.GetBonus(STOP_3_FAIL_NEU);
            }

            switchToEnd();
            printer.PrintDialog(dial.Text, dial.TimeOnScreen, NegociatorState);
            ///
        }
        else
        {
            stopCounter = 0;
            if (endByTimer)
            {

                ////ADD BONUS PHRASE HERE
                printer.PrintDialog(topic.NegativeTransition, 1, NegociatorState);
                if (NegociatorState < 0)
                {
                    globalScore--;
                }
            }
            else
            {
                if (type < 0)
                {
                    printer.PrintDialog(topic.NegativeTransition, 1, NegociatorState);
                    if (NegociatorState < 0)
                    {
                        globalScore--;
                        if (NegociatorState < -1)
                        {
                            leaveSteps++;
                            NegociatorState = -1;
                        }
                    }
                }
                else if (type > 0)
                {
                    printer.PrintDialog(topic.PositiveTransition, 1, NegociatorState);
                    if (NegociatorState > 0)
                    {
                        globalScore++;
                        if (NegociatorState > 1)
                        {
                            leaveSteps++;
                            NegociatorState = 1;
                        }
                    }
                }
                else
                {
                    printer.PrintDialog(topic.NeutralTransition, 1, NegociatorState);
                }
            }
            currentTopicID++;

            lastState = NegociatorState;
        }

    }

    public void GoAnswer()
    {
        printer.HideAnswer();
        printer.PrintDialog(topic.LeaveTransition, 1, NegociatorState);
        leaveSteps--;
        if(leaveSteps <= 0)
        {
            Debug.Log("GAME OVER");
            switchToEnd();
        }
        else
        {
            ///END GAME
            DialogObject dial;
            if (NegociatorState < 0)
            {
                dial = DialogStore.Instance.GetBonus(MALUS_STOP_CONV_NEG);
            }
            else if (NegociatorState > 0)
            {
                dial = DialogStore.Instance.GetBonus(MALUS_STOP_CONV_POS);
            }
            else
            {
                dial = DialogStore.Instance.GetBonus(MALUS_STOP_CONV_NEU);
            }
            globalScore--;
            printer.PrintDialog(dial.Text, dial.TimeOnScreen, NegociatorState);
            ///
        }
    }

}


public enum NegociatorState
{
    UPSET = -1,
    NEUTRAL = 0,
    HAPPY = 1
}
