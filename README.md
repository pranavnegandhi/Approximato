# Approximato - the Forgiving Time Manager

Approximato is a time manager for people who like a bit of elbow room. It is based upon the Pomodoro technique, but allows extending the focus period with extra time when needed (like when you're within touching distance of completing a task). This extra time is tracked separately from the focus period, and can be useful in estimating future tasks, or in identifying hours of peak productivity during the day.

The application user interface consists of a main window and a context menu that is accessed from the Notification Tray on the Windows Task Bar. The window shows 2 progress bars. The larger progress bar is used to display the time elapsed during a focus period, while the smaller one is used to indicate time elapsed during a break.

To begin a new Pomodoro, right click on the application icon and click on Start. The application is pre-programmed with 4 focus sessions of 25 minutes, each followed by a 5 minute break. The last break is 15 minutes long by default. When the 25 minutes of a focus session have elapsed, the application transitions into a break. An alarm bell is rung and a notification message is shown to indicate this state change. When the break completes, the application transitions back into a new focus period. The alarm bell is run twice and a notification message is displayed to indicate this.

After the fourth focus period, the application is transitioned into a longer break.

This behaviour is similar to how the standard Pomodoro technique is usually implemented.