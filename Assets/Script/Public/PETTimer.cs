/****************************************************
	文件：PETimer.cs
	作者：SIKI学院——Plane
	邮箱: 1785275942@qq.com
	日期：2019/01/24 8:26   	
	功能：通用的 延时执行工具类,建议一个项目只创建一个对象,节省开销,类似于luaEnv
    备注：原作者是SIKI学院——Plane老师,非常好用的一个计时器,可以在不继承MonoBehavior的情况下使用
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
    //可以设置 自己想要的打印日志的方式  以及执行回调的方式  
    private Action<string> taskLog;
    private Action<Action<int>, int> taskHandle;

    //三个锁 解决多线程的数据问题
    private static readonly string lockTid = "lockTid";
    private static readonly string lockTime = "lockTime";
    private static readonly string lockFrame = "lockFrame";

    //两组集合 任务数组  记录新添任务时的临时数组 记录将要删除的任务的临时数组  用临时数组可以让逻辑更加清晰 
    private List<PETimeTask> tmpTimeLst = new List<PETimeTask>();
    private List<PETimeTask> taskTimeLst = new List<PETimeTask>();
    private List<int> tmpDelTimeLst = new List<int>();

    private int frameCounter;
    private List<PEFrameTask> tmpFrameLst = new List<PEFrameTask>();
    private List<PEFrameTask> taskFrameLst = new List<PEFrameTask>();
    private List<int> tmpDelFrameLst = new List<int>();

    private int tid;//任务id  可以通过此ID删除 或者 修改任务
    private Timer srvTimer;
    private double nowTime;
    //用于计算 现在距离1970.... 的总时长  以此作为nowtime
    private DateTime startDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

    /// <summary>
    /// 创建PETimer对象，可以通过填入一个时间间隔，来实现多线程处理
    /// </summary>
    /// <param name="interval">update的调用间隔，会直接影响帧任务的执行速度</param>
    public PETimer(int interval = 0)
    {
        tmpTimeLst.Clear();
        taskTimeLst.Clear();

        tmpFrameLst.Clear();
        taskFrameLst.Clear();

        if (interval != 0)
        {
            //AutoReset自动重置  这个属性有啥具体影响不知，填true就好
            srvTimer = new Timer(interval)
            {
                AutoReset = true
            };
            //两个参数的用途不知， 每过一个 interval的时长 就会执行一次 elapsed里的事件
            srvTimer.Elapsed += (object sender, ElapsedEventArgs args) =>
            {
                Update();
            };
            //需要开启一下 跟Thread.Start()同理
            srvTimer.Start();
        }
    }
    /// <summary>
    /// 如果没有在创建PETimer对象时指定 间隔 ，使用多线程处理。那么就需要找一个真正会每帧执行的地方，调用update，比如unity的update
    /// </summary>
    public void Update()
    {
        //检查是否有任务已经到了要执行的时间
        CheckTimeTask();
        CheckFrameTask();

        //检查是否有要删除的任务
        DelTimeTask();
        DelFrameTask();
    }
    private void CheckTimeTask()
    {
        if (tmpTimeLst.Count > 0)
        {
            lock (lockTime)
            {
                //加入缓存区中的定时任务
                for (int tmpIndex = 0; tmpIndex < tmpTimeLst.Count; tmpIndex++)
                {
                    taskTimeLst.Add(tmpTimeLst[tmpIndex]);
                }
                tmpTimeLst.Clear();
            }
        }

        //遍历检测任务是否达到条件
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
                }//一般不会出错 ，但如果这个时间工具类用于服务器的话 还是尽量保守一些
                catch (Exception e)
                {
                    LogInfo(e.ToString());
                }

                //移除已经完成的任务
                if (task.count == 1)
                {
                    taskTimeLst.RemoveAt(index);
                    //因为还在遍历taskTimeLst这个数组，这里移除了一个元素，导致数组元素的索引变化了，因此需要index--来校正一下
                    index--;
                }
                else
                {
                    //count默认为1 代表任务是只执行一次的  如果参数给0 代表是无限次，给其他值代表执行对应的次数
                    if (task.count != 0)
                    {
                        task.count -= 1;
                    }
                    //更新一下 下次任务执行的时间
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
                //加入缓存区中的定时任务
                for (int tmpIndex = 0; tmpIndex < tmpFrameLst.Count; tmpIndex++)
                {
                    taskFrameLst.Add(tmpFrameLst[tmpIndex]);
                }
                tmpFrameLst.Clear();
            }
        }
        frameCounter += 1;
        //遍历检测任务是否达到条件
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

                //移除已经完成的任务
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

                    for (int j = 0; j < tmpTimeLst.Count; j++)//未添加到taskTimieLst中的任务
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
    /// 添加定时任务
    /// </summary>
    /// <param name="callback">执行的行动</param>
    /// <param name="delay">延时时间</param>
    /// <param name="timeUnit">时间单位</param>
    /// <param name="count">循环次数</param>
    /// <returns>任务编号</returns>
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
    /// 提供外部接口
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
            //如果怕int不够用 可以直接改成long long
        }
        return tid;
    }
    /// <summary>
    ///得到当前时间的字符串，按  时：分：秒 的格式
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
    /// 如果要打印日志，那么需要设置一下用什么来打印日志 比如unity的print debug  控制台的console.write
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

    #region 数据结构
    class PETimeTask
    {
        public int tid;
        public Action<int> callback;
        //执行函数的时间  now+delay  如果当前时间>destTime 就执行里面的回调
        public double destTime;//单位：毫秒
        public double delay;
        //执行次数
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
