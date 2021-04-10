/****************************************************
	�ļ���PETimer.cs
	���ߣ�SIKIѧԺ����Plane
	����: 1785275942@qq.com
	���ڣ�2019/01/24 8:26   	
	���ܣ�ͨ�õ� ��ʱִ�й�����,����һ����Ŀֻ����һ������,��ʡ����,������luaEnv
    ��ע��ԭ������SIKIѧԺ����Plane��ʦ,�ǳ����õ�һ����ʱ��,�����ڲ��̳�MonoBehavior�������ʹ��
*****************************************************/

using System;
using System.Collections.Generic;
using System.Timers;
public enum PETimeUnit
{
    Millisecond,
    Second,
    Minute,
    Hour,
    Day
}
public class PETimer
{
    //�������� �Լ���Ҫ�Ĵ�ӡ��־�ķ�ʽ  �Լ�ִ�лص��ķ�ʽ  
    private Action<string> taskLog;
    private Action<Action<int>, int> taskHandle;

    //������ ������̵߳���������
    private static readonly string lockTid = "lockTid";
    private static readonly string lockTime = "lockTime";
    private static readonly string lockFrame = "lockFrame";

    //���鼯�� ��������  ��¼��������ʱ����ʱ���� ��¼��Ҫɾ�����������ʱ����  ����ʱ����������߼��������� 
    private List<PETimeTask> tmpTimeLst = new List<PETimeTask>();
    private List<PETimeTask> taskTimeLst = new List<PETimeTask>();
    private List<int> tmpDelTimeLst = new List<int>();

    private int frameCounter;
    private List<PEFrameTask> tmpFrameLst = new List<PEFrameTask>();
    private List<PEFrameTask> taskFrameLst = new List<PEFrameTask>();
    private List<int> tmpDelFrameLst = new List<int>();

    private int tid;//����id  ����ͨ����IDɾ�� ���� �޸�����
    private Timer srvTimer;
    private double nowTime;
    //���ڼ��� ���ھ���1970.... ����ʱ��  �Դ���Ϊnowtime
    private DateTime startDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

    /// <summary>
    /// ����PETimer���󣬿���ͨ������һ��ʱ��������ʵ�ֶ��̴߳���
    /// </summary>
    /// <param name="interval">update�ĵ��ü������ֱ��Ӱ��֡�����ִ���ٶ�</param>
    public PETimer(int interval = 0)
    {
        tmpTimeLst.Clear();
        taskTimeLst.Clear();

        tmpFrameLst.Clear();
        taskFrameLst.Clear();

        if (interval != 0)
        {
            //AutoReset�Զ�����  ���������ɶ����Ӱ�첻֪����true�ͺ�
            srvTimer = new Timer(interval)
            {
                AutoReset = true
            };
            //������������;��֪�� ÿ��һ�� interval��ʱ�� �ͻ�ִ��һ�� elapsed����¼�
            srvTimer.Elapsed += (object sender, ElapsedEventArgs args) =>
            {
                Update();
            };
            //��Ҫ����һ�� ��Thread.Start()ͬ��
            srvTimer.Start();
        }
    }
    /// <summary>
    /// ���û���ڴ���PETimer����ʱָ�� ��� ��ʹ�ö��̴߳�����ô����Ҫ��һ��������ÿִ֡�еĵط�������update������unity��update
    /// </summary>
    public void Update()
    {
        //����Ƿ��������Ѿ�����Ҫִ�е�ʱ��
        CheckTimeTask();
        CheckFrameTask();

        //����Ƿ���Ҫɾ��������
        DelTimeTask();
        DelFrameTask();
    }
    private void CheckTimeTask()
    {
        if (tmpTimeLst.Count > 0)
        {
            lock (lockTime)
            {
                //���뻺�����еĶ�ʱ����
                for (int tmpIndex = 0; tmpIndex < tmpTimeLst.Count; tmpIndex++)
                {
                    taskTimeLst.Add(tmpTimeLst[tmpIndex]);
                }
                tmpTimeLst.Clear();
            }
        }

        //������������Ƿ�ﵽ����
        nowTime = GetUTCMilliseconds();
        for (int index = 0; index < taskTimeLst.Count; index++)
        {
            PETimeTask task = taskTimeLst[index];
            if (nowTime < task.destTime)
            {
                continue;
            }
            else
            {
                Action<int> cb = task.callback;
                try
                {
                    if (taskHandle != null && cb != null)
                    {
                        taskHandle(cb, task.tid);
                    }
                    else
                    {
                        if (cb != null)
                        {
                            cb(task.tid);
                        }
                    }
                }//һ�㲻����� ����������ʱ�乤�������ڷ������Ļ� ���Ǿ�������һЩ
                catch (Exception e)
                {
                    LogInfo(e.ToString());
                }

                //�Ƴ��Ѿ���ɵ�����
                if (task.count == 1)
                {
                    taskTimeLst.RemoveAt(index);
                    //��Ϊ���ڱ���taskTimeLst������飬�����Ƴ���һ��Ԫ�أ���������Ԫ�ص������仯�ˣ������Ҫindex--��У��һ��
                    index--;
                }
                else
                {
                    //countĬ��Ϊ1 ����������ִֻ��һ�ε�  ���������0 ���������޴Σ�������ֵ����ִ�ж�Ӧ�Ĵ���
                    if (task.count != 0)
                    {
                        task.count -= 1;
                    }
                    //����һ�� �´�����ִ�е�ʱ��
                    task.destTime += task.delay;
                }
            }
        }
    }
    private void CheckFrameTask()
    {
        if (tmpFrameLst.Count > 0)
        {
            lock (lockFrame)
            {
                //���뻺�����еĶ�ʱ����
                for (int tmpIndex = 0; tmpIndex < tmpFrameLst.Count; tmpIndex++)
                {
                    taskFrameLst.Add(tmpFrameLst[tmpIndex]);
                }
                tmpFrameLst.Clear();
            }
        }
        frameCounter += 1;
        //������������Ƿ�ﵽ����
        for (int index = 0; index < taskFrameLst.Count; index++)
        {
            PEFrameTask task = taskFrameLst[index];
            if (frameCounter < task.destFrame)
            {
                continue;
            }
            else
            {
                Action<int> cb = task.callback;
                try
                {
                    if (taskHandle != null && cb != null)
                    {
                        taskHandle(cb, task.tid);
                    }
                    else
                    {
                        if (cb != null)
                        {
                            cb(task.tid);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogInfo(e.ToString());
                }

                //�Ƴ��Ѿ���ɵ�����
                if (task.count == 1)
                {
                    taskFrameLst.RemoveAt(index);
                    index--;
                }
                else
                {
                    if (task.count != 0)
                    {
                        task.count -= 1;
                    }
                    task.destFrame += task.delay;
                }
            }
        }
    }
    private void DelTimeTask()
    {
        if (tmpDelTimeLst.Count > 0)
        {
            lock (lockTime)
            {
                for (int i = 0; i < tmpDelTimeLst.Count; i++)
                {
                    bool isDel = false;
                    int delTid = tmpDelTimeLst[i];
                    for (int j = 0; j < taskTimeLst.Count; j++)
                    {
                        PETimeTask task = taskTimeLst[j];
                        if (task.tid == delTid)
                        {
                            isDel = true;
                            taskTimeLst.RemoveAt(j);
                            break;
                        }
                    }

                    if (isDel)
                        continue;

                    for (int j = 0; j < tmpTimeLst.Count; j++)//δ��ӵ�taskTimieLst�е�����
                    {
                        PETimeTask task = tmpTimeLst[j];
                        if (task.tid == delTid)
                        {
                            tmpTimeLst.RemoveAt(j);
                            break;
                        }
                    }
                }
                tmpDelTimeLst.Clear();
            }
        }
    }
    private void DelFrameTask()
    {
        if (tmpDelFrameLst.Count > 0)
        {
            lock (lockFrame)
            {
                for (int i = 0; i < tmpDelFrameLst.Count; i++)
                {
                    bool isDel = false;
                    int delTid = tmpDelFrameLst[i];
                    for (int j = 0; j < taskFrameLst.Count; j++)
                    {
                        PEFrameTask task = taskFrameLst[j];
                        if (task.tid == delTid)
                        {
                            isDel = true;
                            taskFrameLst.RemoveAt(j);
                            break;
                        }
                    }

                    if (isDel)
                        continue;

                    for (int j = 0; j < tmpFrameLst.Count; j++)
                    {
                        PEFrameTask task = tmpFrameLst[j];
                        if (task.tid == delTid)
                        {
                            tmpFrameLst.RemoveAt(j);
                            break;
                        }
                    }
                }
                tmpDelFrameLst.Clear();
            }
        }
    }

    #region TimeTask
    /// <summary>
    /// ��Ӷ�ʱ����
    /// </summary>
    /// <param name="callback">ִ�е��ж�</param>
    /// <param name="delay">��ʱʱ��</param>
    /// <param name="timeUnit">ʱ�䵥λ</param>
    /// <param name="count">ѭ������</param>
    /// <returns>������</returns>
    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        if (timeUnit != PETimeUnit.Millisecond)
        {
            switch (timeUnit)
            {
                case PETimeUnit.Second:
                    delay = delay * 1000;
                    break;
                case PETimeUnit.Minute:
                    delay = delay * 1000 * 60;
                    break;
                case PETimeUnit.Hour:
                    delay = delay * 1000 * 60 * 60;
                    break;
                case PETimeUnit.Day:
                    delay = delay * 1000 * 60 * 60 * 24;
                    break;
            }
        }
        int tid = GetTid();
        nowTime = GetUTCMilliseconds();
        lock (lockTime)
        {
            tmpTimeLst.Add(new PETimeTask(tid, callback, nowTime + delay, delay, count));
        }
        return tid;
    }
    /// <summary>
    /// �ṩ�ⲿ�ӿ�
    /// </summary>
    /// <param name="tid"></param>
    public void DeleteTimeTask(int tid)
    {
        lock (lockTime)
        {
            tmpDelTimeLst.Add(tid);
        }
    }
    public bool ReplaceTimeTask(int tid, Action<int> callback, float delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        if (timeUnit != PETimeUnit.Millisecond)
        {
            switch (timeUnit)
            {
                case PETimeUnit.Second:
                    delay = delay * 1000;
                    break;
                case PETimeUnit.Minute:
                    delay = delay * 1000 * 60;
                    break;
                case PETimeUnit.Hour:
                    delay = delay * 1000 * 60 * 60;
                    break;
                case PETimeUnit.Day:
                    delay = delay * 1000 * 60 * 60 * 24;
                    break;
            }
        }
        nowTime = GetUTCMilliseconds();
        PETimeTask newTask = new PETimeTask(tid, callback, nowTime + delay, delay, count);

        bool isRep = false;
        for (int i = 0; i < taskTimeLst.Count; i++)
        {
            if (taskTimeLst[i].tid == tid)
            {
                taskTimeLst[i] = newTask;
                isRep = true;
                break;
            }
        }

        if (!isRep)
        {
            for (int i = 0; i < tmpTimeLst.Count; i++)
            {
                if (tmpTimeLst[i].tid == tid)
                {
                    tmpTimeLst[i] = newTask;
                    isRep = true;
                    break;
                }
            }
        }
        return isRep;
    }
    #endregion

    #region FrameTask
    public int AddFrameTask(Action<int> callback, int delay, int count = 1)
    {
        int tid = GetTid();
        lock (lockTime)
        {
            tmpFrameLst.Add(new PEFrameTask(tid, callback, frameCounter + delay, delay, count));
        }
        return tid;
    }
    public void DeleteFrameTask(int tid)
    {
        lock (lockFrame)
        {
            tmpDelFrameLst.Add(tid);
        }
    }
    public bool ReplaceFrameTask(int tid, Action<int> callback, int delay, int count = 1)
    {
        PEFrameTask newTask = new PEFrameTask(tid, callback, frameCounter + delay, delay, count);

        bool isRep = false;
        for (int i = 0; i < taskFrameLst.Count; i++)
        {
            if (taskFrameLst[i].tid == tid)
            {
                taskFrameLst[i] = newTask;
                isRep = true;
                break;
            }
        }

        if (!isRep)
        {
            for (int i = 0; i < tmpFrameLst.Count; i++)
            {
                if (tmpFrameLst[i].tid == tid)
                {
                    tmpFrameLst[i] = newTask;
                    isRep = true;
                    break;
                }
            }
        }

        return isRep;
    }
    #endregion

    public void SetLog(Action<string> log)
    {
        taskLog = log;
    }
    public void SetHandle(Action<Action<int>, int> handle)
    {
        taskHandle = handle;
    }

    public void Reset()
    {
        tid = 0;

        tmpTimeLst.Clear();
        taskTimeLst.Clear();

        tmpFrameLst.Clear();
        taskFrameLst.Clear();

        taskLog = null;
        taskHandle = null;
        if (srvTimer != null)
        {
            srvTimer.Stop();
        }

    }

    #region Tool Methonds
    private int GetTid()
    {
        lock (lockTid)
        {
            tid += 1;
            //�����int������ ����ֱ�Ӹĳ�long long
        }
        return tid;
    }
    /// <summary>
    ///�õ���ǰʱ����ַ�������  ʱ���֣��� �ĸ�ʽ
    /// </summary>
    /// <returns></returns>
    public string GetLocalTimeStr()
    {
        DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
        string str = GetTimeStr(dt.Hour) + ":" + GetTimeStr(dt.Minute) + ":" + GetTimeStr(dt.Second);
        return str;
    }
    private string GetTimeStr(int time)
    {
        if (time < 10)
            return "0" + time;
        else
            return time.ToString();
    }
    private double GetUTCMilliseconds()
    {
        TimeSpan ts = DateTime.UtcNow - startDateTime;
        return ts.TotalMilliseconds;
    }
    /// <summary>
    /// ���Ҫ��ӡ��־����ô��Ҫ����һ����ʲô����ӡ��־ ����unity��print debug  ����̨��console.write
    /// </summary>
    /// <param name="info"></param>
    private void LogInfo(string info)
    {
        if (taskLog != null)
        {
            taskLog(info);
        }
    }
    #endregion

    #region ���ݽṹ
    class PETimeTask
    {
        public int tid;
        public Action<int> callback;
        //ִ�к�����ʱ��  now+delay  �����ǰʱ��>destTime ��ִ������Ļص�
        public double destTime;//��λ������
        public double delay;
        //ִ�д���
        public int count;

        public PETimeTask(int tid, Action<int> callback, double destTime, double delay, int count)
        {
            this.tid = tid;
            this.callback = callback;
            this.destTime = destTime;
            this.delay = delay;
            this.count = count;
        }
    }

    class PEFrameTask
    {
        public int tid;
        public Action<int> callback;
        public int destFrame;
        public int delay;
        public int count;

        public PEFrameTask(int tid, Action<int> callback, int destFrame, int delay, int count)
        {
            this.tid = tid;
            this.callback = callback;
            this.destFrame = destFrame;
            this.delay = delay;
            this.count = count;
        }
    }
    #endregion
}
