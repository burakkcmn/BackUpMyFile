using System;
using System.Threading;

namespace TimerHelp
{
    public class MyTimer
    {
        private System.Threading.Timer _timer;
        private int _period;
        private int _dueTime;
        private bool isTimerActive;
        private object _state;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timerCallBack">A TimerCallback delegate representing a method to be executed.</param>
        ///                                 Example:
        ///                                     void _timer1CallBack(object state)
        ///                                     {     
        ///                                         ....      
        ///                                     }
        /// <param name="period">The time interval between invocations of callback, in milliseconds. Specify Timeout.Infinite to disable periodic signaling.</param>
        /// <param name="dueTime">The amount of time to delay before callback is invoked, in milliseconds. Specify Timeout.Infinite to prevent the timer from starting. Specify zero (0) to start the timer immediately.</param>
        /// <param name="state">An object containing information to be used by the callback method, or null.</param>
        public MyTimer(TimerCallback timerCallBack, int period = System.Threading.Timeout.Infinite,
            int dueTime = System.Threading.Timeout.Infinite, object state = null)
        {
            this._period = period;
            this._dueTime = dueTime;
            this._state = state;
            _timer = new System.Threading.Timer(timerCallBack, state, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }

        public void DefineCallBack(TimerCallback timerCallBack)
        {
            _timer = new System.Threading.Timer(timerCallBack, _state, this._dueTime, this._period);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period">The time interval between invocations of callback, in milliseconds. Specify Timeout.Infinite to disable periodic signaling.</param>
        public void Start(int period = 0)
        {
            if (_timer != null)
            {
                if (period != 0)
                {
                    _period = period;
                }
                if (_dueTime == System.Threading.Timeout.Infinite)
                {
                    _dueTime = 0;
                }
                _timer.Change(_dueTime, _period);
                isTimerActive = true;
            }
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                isTimerActive = false;
            }
        }

        /// <summary>
        /// Change timer periode
        /// </summary>
        /// <param name="dueTime">The amount of time to delay before the invoking the callback method specified when the Timer was constructed, in milliseconds. Specify Timeout.Infinite to prevent the timer from restarting. Specify zero (0) to restart the timer immediately.</param>
        /// <param name="period">The time interval between invocations of the callback method specified when the Timer was constructed, in milliseconds. Specify Timeout.Infinite to disable periodic signaling.</param>
        public void Change(int dueTime, int period)
        {
            if (_timer != null)
            {
                if (dueTime != System.Threading.Timeout.Infinite)
                {
                    _dueTime = dueTime;
                }
                if (period != System.Threading.Timeout.Infinite)
                {
                    _period = period;
                }
                _timer.Change(_dueTime, _period);
                isTimerActive = true;
            }
        }
        
        /// <summary>
        /// Set timer's period
        /// </summary>
        /// <param name="period">The time interval between invocations of callback, in milliseconds. Specify Timeout.Infinite to disable periodic signaling.</param>
        public void Period(int period)
        {
            if (_timer != null)
            {
                if (period >= 0)
                {
                    _period = period;
                }
                _timer.Change(_dueTime, _period);
                isTimerActive = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dueTime">The amount of time to delay before callback is invoked, in milliseconds. Specify Timeout.Infinite to prevent the timer from starting. Specify zero (0) to start the timer immediately.</param>
        public void DueTime(int dueTime)
        {
            if (_timer != null)
            {
                if (dueTime >= 0)
                {
                    _dueTime = dueTime;
                }
                _timer.Change(_dueTime, _period);
            }
        }

        public bool IsActive()
        {
            if (isTimerActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            _period = 0;
            _dueTime = 0;
            _timer = null;
        }
    }
}
