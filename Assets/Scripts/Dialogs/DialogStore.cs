using UnityEngine;
using System.Collections.Generic;

public class DialogStore : Singleton<DialogStore>
{
    private const int ID_TOPIC = 0;
    private const int ID_TIME = 1;
    private const int ID_TEXT = 2;
    private const int ID_ANSWER_NEUTRAL = 3;
    private const int ID_ANSWER_POS = 4;
    private const int ID_ANSWER_NEG = 5;
    private const int ID_ANSWER_LEAVE = 6;

    public TextAsset NeutralDialogsAsset;
    public TextAsset PositivDialogsAsset;
    public TextAsset NegativDialogsAsset;
    public TextAsset BonusDialogAsset;

    private Dictionary<int, TopicObject> neutralTopics;
    private Dictionary<int, TopicObject> positivTopics;
    private Dictionary<int, TopicObject> negativTopics;
    private Dictionary<int, DialogObject> bonus;
    public int MaxTopicId { get; private set; }

    void Awake()
    {
        neutralTopics = new Dictionary<int, TopicObject>();
        positivTopics = new Dictionary<int, TopicObject>();
        negativTopics = new Dictionary<int, TopicObject>();
        bonus = new Dictionary<int, DialogObject>();
        LoadDatas(NeutralDialogsAsset, ref neutralTopics);
        LoadDatas(PositivDialogsAsset, ref positivTopics);
        LoadDatas(NegativDialogsAsset, ref negativTopics);
        loadBonuses();
    }


    public void LoadDatas(TextAsset file, ref Dictionary<int, TopicObject> store)
    {
        //Three data storage from the three CSV sheet
        string[,] datas = CSVReader.SplitCsvGrid(file.text);

        int raw = datas.GetUpperBound(1);
        int lastid = -1;
        TopicObject topic = new TopicObject(0);
        for (int i = 0; i < raw; i++)
        {
            int topicId = int.Parse(datas[ID_TOPIC, i]);
            if(topicId > MaxTopicId)
            {
                MaxTopicId = topicId;
            }
            //Chexk if we are in a new topic
            if(topicId != lastid)
            {
                topic = new TopicObject(topicId);
                lastid = topicId;
            }
            string dial = datas[ID_TEXT, i];
            double time = double.Parse(datas[ID_TIME, i]);
            //Create a new dialog and add it to topic;
            topic.AddDialog(new DialogObject(dial, time));
            if (datas[ID_ANSWER_NEUTRAL, i] != "")
            {
                topic.addAnswers(datas[ID_ANSWER_POS, i], datas[ID_ANSWER_NEG, i], datas[ID_ANSWER_NEUTRAL, i], datas[ID_ANSWER_LEAVE, i]);
                //increment i to get the next without go throught the loop again
                i++;
                topic.addTransitions(datas[ID_ANSWER_POS, i], datas[ID_ANSWER_NEG, i], datas[ID_ANSWER_NEUTRAL, i], datas[ID_ANSWER_LEAVE, i]);
                store.Add(topicId, topic);

            }
        }
    }

    void loadBonuses()
    {
        string[,] datas = CSVReader.SplitCsvGrid(BonusDialogAsset.text);
        int raw = datas.GetUpperBound(1);
        for (int i = 0; i < raw; i++)
        {
            bonus.Add(int.Parse(datas[0, i]), new DialogObject(datas[2, i], int.Parse(datas[1, i])));
        }
    }

    public TopicObject GetTopic(int id, int state)
    {
        if(state < 0)
            return negativTopics[id];
        else if(state > 0)
            return positivTopics[id];
        else
            return neutralTopics[id];
    }
}
