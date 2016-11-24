using UnityEngine;
using System.Collections;

public class ThreadedJob
{
    private bool m_isDone = false;
    private object m_handle = new object();
    private System.Threading.Thread m_thread = null;

    public bool IsDone
    {
        get
        {
            bool temp;
            lock (m_handle)
            {
                temp = m_isDone;
            }
            return temp;
        }
        set
        {
            lock (m_handle)
            {
                m_isDone = value;
            }
        }
    }

    public virtual void Start()
    {
        m_thread = new System.Threading.Thread(Run);
        m_thread.Start();
    }

    public virtual void Abort()
    {
        m_thread.Abort();
    }

    protected virtual void ThreadedFunction() { }
    protected virtual void OnFinished() { }

    public virtual bool Update()
    {
        if (IsDone)
        {
            OnFinished();
            return true;
        }
        return false;
    }

    public IEnumerator WaitFor()
    {
        while (!Update())
        {
            yield return null;
        }
    }

    private void Run()
    {
        ThreadedFunction();
        IsDone = true;
    }
}
