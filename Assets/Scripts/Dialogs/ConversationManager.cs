using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage the conversation and redirect dialog in function of player's answer
/// </summary>
public class ConversationManager : Singleton<ConversationManager> {

    /// <summary>
    /// The state of the negociator. ARBITRARY AND REALLY MESSY
    /// 0 = neutral
    /// neg = upset
    /// pos = happy
    /// </summary>
    public int NegociatorState { get; private set; }

    private int currentTopicID = 0;
    private TopicObject topic;

    private int phraseInTopic = 0;

    public DialogPrinterScript printer;

    private int lastState = 0;
    private int stopCounter = 0;
    private int leaveSteps = 0; //how much time the player have to use the "leave" button

    private int globalScore;

	// Use this for initialization
	void Start ()
    {
        printer.endOfPrintEvent += NextPhrase;
        MainMenu.instance.startGameEvent += NextPhrase;
    }

    void NextPhrase()
    {
        if(phraseInTopic == 0)
        {
            if (currentTopicID < DialogStore.Instance.MaxTopicId)
            {
                topic = DialogStore.Instance.GetTopic(currentTopicID, NegociatorState);
            }
            else
            {
                Debug.Log("GAME OVER");
            }
        }

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

    public void Answer(int type)
    {
        NegociatorState += type;
        globalScore += type;
        if(type < 0)
        {
            printer.PrintDialog(topic.NegativeTransition, 1, NegociatorState);
            if(NegociatorState < 0)
            {
                globalScore--;
            }
        }
        else if(type > 0)
        {
            printer.PrintDialog(topic.PositiveTransition, 1, NegociatorState);
            if (NegociatorState < 0)
            {
                globalScore++;
            }
        }
        else
        {
            printer.PrintDialog(topic.NeutralTransition, 1, NegociatorState);
        }
        currentTopicID++;

        lastState = NegociatorState;
        //Compute stop conditions
        if(NegociatorState == lastState)
        {
            stopCounter++;
            if(stopCounter >= 3)
            {
                Debug.Log("GAME OVER");
            }
        }
        else
        {
            stopCounter = 0;
        }

    }

    public void GoAnswer()
    {
        printer.PrintDialog(topic.LeaveTransition, 1, NegociatorState);
        leaveSteps--;
        if(leaveSteps <= 0)
        {
            Debug.Log("GAME OVER");
        }
    }

}


public enum NegociatorState
{
    UPSET = -1,
    NEUTRAL = 0,
    HAPPY = 1
}
