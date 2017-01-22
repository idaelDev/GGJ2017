using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage the conversation and redirect dialog in function of player's answer
/// </summary>
public class ConversationManager : Singleton<ConversationManager> {

    const int END_TOPICS_POS = 0; //done
    const int END_TOPICS_NEU = 1; //done
    const int END_TOPICS_NEG = 2; //done
    const int MALUS_STOP_CONV_POS = 3;//done
    const int MALUS_STOP_CONV_NEU = 4;//done
    const int MALUS_STOP_CONV_NEG = 5;//done
    const int STOP_3_FAIL_POS = 6;//done
    const int STOP_3_FAIL_NEU = 7;//done
    const int STOP_3_FAIL_NEG = 8;//done
    const int PLAYER_END_POS = 9;//done
    const int PLAYER_END_NEU = 10;//done
    const int PLAYER_END_NEG = 11;//done
    const int END_TIMER_POS = 12;
    const int END_TIMER_NEU = 13;
    const int END_TIMER_NEG = 14;


    public DialogPrinterScript printer;

#region Delegates
    public delegate void EndGameDelegate();
    public event EndGameDelegate endGameEvent;

    public delegate void StateChangeDelegate(int val);
    public event StateChangeDelegate stateChangeEvent; 
#endregion

    /// <summary>
    /// The state of the negociator. ARBITRARY AND REALLY MESSY
    /// 0 = neutral
    /// neg = upset
    /// pos = happy
    /// </summary>
    private int negociatorState;
    public int NegociatorState { get { return negociatorState; } private set { negociatorState = value; stateChangeEvent(negociatorState); } }

    private int currentTopicID = 0;
    private TopicObject topic;

    private int phraseInTopic = 0;

    private int previousState = 0;
    private int stateRepetitionCount = 0;
    private int numberOfStepForLeaving = 0; //how much time the player have to use the "leave" button

    private int globalScore;

    // Use this for initialization
    void Start ()
    {
        printer.endOfPrintEvent += NextPhrase;
        MainMenu.instance.startGameEvent += NextPhrase;
    }

    void NextPhrase()
    {
        NegociatorState = Mathf.Clamp(NegociatorState, -1, 1);
        if (phraseInTopic == 0)
        {
            topic = DialogStore.Instance.GetTopic(currentTopicID, NegociatorState);
        }
        if(currentTopicID >= DialogStore.Instance.MaxTopicId)
        {
            ///GAME OVER
            Debug.Log("GAME OVER");
            switchToEnd();
            LaunchBonusPhrases(END_TOPICS_NEU, END_TOPICS_POS, END_TOPICS_NEG);

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

    public void Answer(int type, bool endByTimer)
    {
        //SetGraphics
        printer.HideAnswer();

        //Update the grandmaState and Score
        NegociatorState += type;
        globalScore += type;

        //Compute stop conditions
        if (NegociatorState == previousState || NegociatorState > 1 || NegociatorState < -1)
        {
            stateRepetitionCount++;
        }
        else
        {
            stateRepetitionCount = 0;
        }

        //..

        if (stateRepetitionCount >= 3)
        {

            switchToEnd();
            LaunchBonusPhrases(STOP_3_FAIL_NEU, STOP_3_FAIL_POS, STOP_3_FAIL_NEG);
        }
        else
        {

            if (endByTimer)
            {

                LaunchBonusPhrases(END_TIMER_NEU, END_TIMER_POS, END_TIMER_NEG);
                if (NegociatorState < 0)
                {
                    globalScore -= 2;
                }
            }
            else
            {
                if (type < 0)
                {
                    printer.PrintDialog(topic.NegativeTransition, 4, NegociatorState);
                    if (NegociatorState < 0)
                    {
                        globalScore--;
                        if (NegociatorState < -1)
                        {
                            numberOfStepForLeaving++;
                        }
                    }
                }
                else if (type > 0)
                {
                    printer.PrintDialog(topic.PositiveTransition, 4, NegociatorState);
                    if (NegociatorState > 0)
                    {
                        globalScore++;
                        if (NegociatorState > 1)
                        {
                            numberOfStepForLeaving++;
                        }
                    }
                }
                else
                {
                    printer.PrintDialog(topic.NeutralTransition, 4, NegociatorState);
                }
            }
            currentTopicID++;

            previousState = Mathf.Clamp(NegociatorState, -1, 1);
        }

    }

    public void GoAnswer()
    {
        printer.HideAnswer();
        printer.PrintDialog(topic.LeaveTransition, 4, NegociatorState);
        numberOfStepForLeaving--;
        if (numberOfStepForLeaving <= 0)
        {
            Debug.Log("GAME OVER");
            switchToEnd();
            LaunchBonusPhrases(PLAYER_END_NEU, PLAYER_END_POS, PLAYER_END_NEG);
        }
        else
        {
            ///END GAME
            LaunchBonusPhrases(MALUS_STOP_CONV_NEU, MALUS_STOP_CONV_POS,MALUS_STOP_CONV_NEG);
        }
    }


    /// <summary>
    /// Launch special phrases function of the state
    /// </summary>
    /// <param name="neu"></param>
    /// <param name="pos"></param>
    /// <param name="neg"></param>
    private void LaunchBonusPhrases(int neu, int pos, int neg)
    {
        DialogObject dial;
        if (NegociatorState < 0)
        {
            dial = DialogStore.Instance.GetBonus(neg);
        }
        else if (NegociatorState > 0)
        {
            dial = DialogStore.Instance.GetBonus(pos);
        }
        else
        {
            dial = DialogStore.Instance.GetBonus(neu);
        }
        printer.PrintDialog(dial.Text, dial.TimeOnScreen, NegociatorState);
    }

    private void switchToEnd()
    {
        printer.endOfPrintEvent -= NextPhrase;
        printer.endOfPrintEvent += EndGame;
    }

    public void EndGame()
    {
        endGameEvent();
        if(!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
    }

   
    public int GetScoreEvaluation()
    {
        if(globalScore > 2)
        {
            return 1;
        }
        else if(globalScore < -2)
        {
            return -1;
        }
        return 0;
    }
    

}


public enum NegociatorState
{
    UPSET = -1,
    NEUTRAL = 0,
    HAPPY = 1
}
