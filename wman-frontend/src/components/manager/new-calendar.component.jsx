import React from "react";
import Calendar from 'tui-calendar';
import "tui-calendar/dist/tui-calendar.css";
import 'tui-date-picker/dist/tui-date-picker.css';
import 'tui-time-picker/dist/tui-time-picker.css';

export const NewCalendar = () => {

    var calendar = new Calendar('#calendar', {
        defaultView: 'month',
        taskView: true,
        month: { startDayOfWeek: 1 },
        week: { startDayOfWeek: 1 },
        useCreationPopup: true,
        useDetailPopup: true
    });

    const changeToWeekView = () =>{
        calendar.changeView('week',true)
    }
    const changeToMonthView = () =>{
        calendar.changeView('month',true)
    }
    const nextView = () =>{
        calendar.next();
    }
    const previousView = () =>{
        calendar.prev();
    }
    const todayView = () =>{
        calendar.today();
    }

    calendar.createSchedules([
        {
            id: '1',
            calendarId: '1',
            title: 'my schedule',
            jobDescription: 'asdasff',
            category: 'time',
            dueDateClass: '',
            start: '2021-10-19T22:30:00+09:00',
            end: '2021-10-20T02:30:00+09:00'
        },
        {
            id: '2',
            calendarId: '2',
            title: 'second schedule',
            category: 'time',
            dueDateClass: '',
            start: '2021-10-21T17:30:00+09:00',
            end: '2021-10-22T17:31:00+09:00'
        }
    ]);

    return (
        <div>
            <div id="menu">
                <span id="menu-navi">
                    <button type="button" class="btn btn-default btn-sm move-today" onClick={todayView}>Today</button>
                    <button type="button" class="btn btn-default btn-sm move-day" onClick={previousView}>
                        Previous
                    </button>
                    <button type="button" class="btn btn-default btn-sm move-day" onClick={nextView}>
                        Next
                    </button>
                    <button type="button" class="btn btn-default btn-sm move-day" onClick={changeToWeekView}>
                        Week
                    </button>
                    <button type="button" class="btn btn-default btn-sm move-day" onClick={changeToMonthView}>
                        Month
                    </button>
                </span>
                <span id="renderRange" class="render-range"></span>
            </div>

            <div id="calendar"></div>
        </div>
    )
};